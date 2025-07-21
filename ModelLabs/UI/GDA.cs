using FTN.Common;
using FTN.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    //GRID DATA ACCESS
    public class GDA: IDisposable
    {
        private ModelResourcesDesc modelResourcesDescription = new ModelResourcesDesc();
        private NetworkModelGDAProxy gdaQueryProxy = null;      // proxy koji komunicira sa NetworkModelService-om

        public void Dispose()
        {
            if (gdaQueryProxy != null)
            {
                gdaQueryProxy.Close();
                gdaQueryProxy = null;
            }
            GC.SuppressFinalize(this);
        }
        private NetworkModelGDAProxy GdaQueryProxy
        {
            get
            {
                /*Iskoristimo ponovo proxy ako je zdrav idalje Zamenimo ga kada se pokvario  izbegavamo da ponovo otvaramo WCF kanale jer to sporo */

                if (gdaQueryProxy == null || gdaQueryProxy.State == CommunicationState.Faulted)
                {
                    if (gdaQueryProxy != null)
                    {
                        gdaQueryProxy.Abort();
                    }

                    gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                    gdaQueryProxy.Open();
                }

                return gdaQueryProxy;
            }
        }


        //da dobijemo propertyValue od specificnog entitija pomocu globalId
        //Procitamo atribute od jednog CIM entity-a
        public ResourceDescription GetValues(long globalId, List<ModelCode> properties)
        {
            var message = "Getting values...";
            CommonTrace.WriteTrace(CommonTrace.TraceError, message);

            ResourceDescription rd = null;

            try
            {
                rd = GdaQueryProxy.GetValues(globalId, properties);

                message = "Getting values  successfully finished.";
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            catch (Exception e)
            {
                message = string.Format("Getting values  for entered id = {0} failed.\n\t{1}", globalId, e.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return rd;

        }

        //Procitamo sve entite  posebnog tipa (npr sve seasone, Reclosere itd...)
        // Returnuje  globalni IDs (long) za dobavljene tipove entitet-a.
        /*
         Primer
         var gids = GetExtentValues(ModelCode.SEASON, new List<ModelCode> { ModelCode.IDOBJ_NAME }, sb);
          Fetchuje sve Season entities iz network model. Retrivuje NAME property.
          Printuje formatirane vrednosti u STRING BUILDER. Returnuje listu svih global IDs season-a
         */
        public List<long> GetExtentValues(ModelCode modelCodeType, List<ModelCode> properties, StringBuilder sb)
        {
            var message = "Getting extent values method started.";
            CommonTrace.WriteTrace(true, message);

            int iteratorId;
            int resourcesLeft;  
            int numberOfResources = 300;            // Batch size
            var ids = new List<long>();             // lista koju vracamo  
            var tempSb = new StringBuilder();       // Da formatiramo citljiv output

            try
            {
                //proxy salje  GDA request da dobijemo sve entitete zeljenog modelCode-a sa izbranim Property-ima, iteratorId nam sluzi da u chunkovima ih dobavljamo
                iteratorId = GdaQueryProxy.GetExtentValues(modelCodeType, properties);     
                
                
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);        //koliko je ostalo resursa jos da dobavimo

                while (resourcesLeft > 0)
                {
                    //fecujemo naredan batch entitet-a (do 300  )
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    foreach (var rd in rds)
                    {
                        if (rd == null)
                            continue;
                        tempSb.Append($"\nEntity with gid: 0x{rd.Id:X16}" + Environment.NewLine);       //formatiramo GID u hexa decim formatu


                        //prolazimo kroz svaki property
                        foreach (var property in rd.Properties)
                        {
                            switch (property.Type)
                            {
                                case PropertyType.Int64:
                                    StringHelper.AppendLong(tempSb, property);
                                    break;
                                case PropertyType.Float:
                                    StringHelper.AppendFloat(tempSb, property);
                                    break;
                                case PropertyType.String:
                                    StringHelper.AppendString(tempSb, property);
                                    break;
                                case PropertyType.Reference:
                                    StringHelper.AppendReference(tempSb, property);
                                    break;
                                case PropertyType.ReferenceVector:
                                    StringHelper.AppendReferenceVector(tempSb, property);
                                    break;
                                case PropertyType.DateTime:
                                    StringHelper.AppendDateTime(tempSb, property);
                                    break;

                                default:
                                    tempSb.Append($"\t{property.Id}: {property.PropertyValue.LongValue}{Environment.NewLine}");
                                    break;
                            }
                        }

                        //cuvamo gid
                        ids.Add(rd.Id);
                    }

                    //updejtujemo kolko nam je preostalo 
                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                //oslobodimo resurse
                GdaQueryProxy.IteratorClose(iteratorId);

                message = "Getting extent values  successfully finished.";
                CommonTrace.WriteTrace(true, message);
            }
            catch (Exception e)
            {
                message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCodeType, e.Message);
                CommonTrace.WriteTrace(true, message + e.StackTrace);
            }

            if (sb != null)
            {
                sb.Append(tempSb.ToString());
            }

            CommonTrace.WriteTrace(CommonTrace.TraceError, "Number of results from GetExtentValues : " + ids.Count.ToString());
            return ids;
        }

        /*
         * sourceGID: the GID (global ID) od izvornog CIM entity
        
          properties: koje propertije uzimamo iz relatovanih entitieta
          
          association: kako su related objects are pvezani (e.g., TERMINAL -> EQUIPMENT, or TRANSFORMER -> WINDINGS).

          sb: a StringBuilder koristi za opcionalno collectovanje formatiranog output-a.

          Returns:listu GIDs (long) od relatiovanih entiteta.
           

        primer:
        var relatedBreakerGIDs = gda.GetRelatedValues( 0x4000000000000023, new List<ModelCode> { ModelCode.IDOBJ_NAME, ModelCode.BREAKER_STATE },
           new Association
            {
                PropertyId = ModelCode.SUBSTATION_BREAKERS,
                Type = TypeOfAssociation.ReferenceVector
            },
            outputBuilder);

         */
        public List<long> GetRelatedValues(long sourceGID, List<ModelCode> properties, Association association, StringBuilder sb)
        {
            var message = "Getting related values method started.";
            CommonTrace.WriteTrace(CommonTrace.TraceError, message);

            int iteratorId = 0;
            int resourcesLeft = 0;
            int numberOfResources = 600;
            var resultIds = new List<long>();
            var tempSb = new StringBuilder();       // builduje human-readable logove

            try
            {
                //GetRelatedValues(...) pita server za sve entitete reletovane sa sourceGlobalId preko asocijacije
                //The result is returned as an iterator ID(used for batched reading).
                iteratorId = GdaQueryProxy.GetRelatedValues(sourceGID, properties, association);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    foreach (var rd in rds)
                    {
                        if (rd == null)
                            continue;
                        //apendujemo gid entiteta
                        tempSb.Append($"\nEntity with gid: 0x{rd.Id:X16}" + Environment.NewLine);

                        foreach (Property property in rd.Properties)
                        {
                            switch (property.Type)
                            {
                                case PropertyType.Int64:
                                    StringHelper.AppendLong(tempSb, property);
                                    break;
                                case PropertyType.Float:
                                    StringHelper.AppendFloat(tempSb, property);
                                    break;
                                case PropertyType.String:
                                    StringHelper.AppendString(tempSb, property);
                                    break;
                                case PropertyType.Reference:
                                    StringHelper.AppendReference(tempSb, property);
                                    break;
                                case PropertyType.ReferenceVector:
                                    StringHelper.AppendReferenceVector(tempSb, property);
                                    break;
                                case PropertyType.DateTime:
                                    StringHelper.AppendDateTime(tempSb, property);
                                    break;
                                default:
                                    tempSb.Append($"\t{property.Id}: {property.PropertyValue.LongValue}{Environment.NewLine}");
                                    break;
                            }
                        }

                        resultIds.Add(rd.Id);
                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                message = "Getting related values successfully finished.";
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            catch (Exception e)
            {
                message =
                    $"Getting related values method  failed for sourceGlobalId = {sourceGID} and association (propertyId = {association.PropertyId}, type = {association.Type}). Reason: {e.Message}";
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            if (sb != null)
            {
                sb.Append(tempSb.ToString());
            }

            return resultIds;
        }

    }
}
