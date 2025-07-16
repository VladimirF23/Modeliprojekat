using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class DayType : IdentifiedObject
    {
        private List<long> seasonDayTypeSchedules2 = new List<long>();
        public List<long> SeasonDayTypeSchedules2 { get => seasonDayTypeSchedules2; set => seasonDayTypeSchedules2 = value; }

        public DayType(long globalId) : base(globalId)
        {
        }


        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                DayType x = obj as DayType;
                return ( CompareHelper.CompareLists(x.seasonDayTypeSchedules2, this.seasonDayTypeSchedules2));
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation		
        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.DAYTYPE_REF:

                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.DAYTYPE_REF:
                    property.SetValue(seasonDayTypeSchedules2);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {


                //ovde ne treba reference staviti

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation



        #region IReference implementation	
        public override bool IsReferenced
        {
            get
            {
                return (seasonDayTypeSchedules2.Count > 0 || base.IsReferenced);
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (seasonDayTypeSchedules2 != null && seasonDayTypeSchedules2.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                //Mislim da treba SEASON_REF
                references[ModelCode.DAYTYPE_REF] = SeasonDayTypeSchedules2.GetRange(0, SeasonDayTypeSchedules2.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                //ZAPAMTI DA SI ZA SEASON UZEO REF1 od SEASON_DAY_TYPE_SCHEDULE OVDE TREBA 2 
                case ModelCode.SEASON_DAY_TYPE_S_REF2:
                    SeasonDayTypeSchedules2.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                //DayType koristi SEASON_DAY_TYPE_S_REF2
                case ModelCode.SEASON_DAY_TYPE_S_REF2:
                    if (seasonDayTypeSchedules2.Contains(globalId))
                        seasonDayTypeSchedules2.Remove(globalId);
                    else
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation
    }
}
