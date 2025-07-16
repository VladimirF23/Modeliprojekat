using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class Season : IdentifiedObject
    {
        private DateTime startDate;
        private DateTime endDate;


        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }


        //ima 0 1 pa onda on cuva ove
        private List<long> seasonDayTypeSchedules = new List<long>();
        public List<long> SeasonDayTypeSchedules { get => seasonDayTypeSchedules; set => seasonDayTypeSchedules = value; }

        public Season(long globalId) : base(globalId)
        {
        }

        //ne znam da li treba == i !=
        public static bool operator ==(Season x, Season y)
        {
            if (Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null))
            {
                return true;
            }
            else if ((Object.ReferenceEquals(x, null) && !Object.ReferenceEquals(y, null)) || (!Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null)))
            {
                return false;
            }
            else
            {
                return x.Equals(y);
            }
        }

        public static bool operator !=(Season x, Season y)
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Season x = obj as Season;
                return (x.startDate == this.startDate &&
                        x.endDate == this.endDate && CompareHelper.CompareLists(x.seasonDayTypeSchedules, this.seasonDayTypeSchedules));
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
                case ModelCode.SEASON_END_DATE:
                case ModelCode.SEASON_START_DATE:
                case ModelCode.SEASON_REF:

                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SEASON_START_DATE:
                    property.SetValue(startDate);
                    break;
                case ModelCode.SEASON_END_DATE:
                    property.SetValue(endDate);
                    break;
                case ModelCode.SEASON_REF:
                    property.SetValue(seasonDayTypeSchedules);
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
                case ModelCode.SEASON_START_DATE:
                    startDate = property.AsDateTime();
                    break;
                case ModelCode.SEASON_END_DATE:
                    endDate = property.AsDateTime();
                    break;

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
                return (seasonDayTypeSchedules.Count > 0 || base.IsReferenced);
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (seasonDayTypeSchedules != null && seasonDayTypeSchedules.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                //Mislim da treba SEASON_REF
                references[ModelCode.SEASON_REF] = SeasonDayTypeSchedules.GetRange(0, SeasonDayTypeSchedules.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                //ZAPAMTI DA SI ZA SEASON UZEO REF1 od SEASON_DAY_TYPE_SCHEDULE
                case ModelCode.SEASON_DAY_TYPE_S_REF1:
                    seasonDayTypeSchedules.Add(globalId);
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
                //season koristi refrence1 od Season_DAY_TYPE
                case ModelCode.SEASON_DAY_TYPE_S_REF1:
                    if (seasonDayTypeSchedules.Contains(globalId))
                        seasonDayTypeSchedules.Remove(globalId);
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


