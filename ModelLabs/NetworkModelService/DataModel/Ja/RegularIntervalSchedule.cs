using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System.Collections.Generic;
using System.Linq;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class RegularIntervalSchedule : BasicIntervalSchedule
    {
        //regular time points lista referneci
        private List<long> timePoints = new List<long>();
        public List<long> TimePoints { get => timePoints; set => timePoints = value; }

        public RegularIntervalSchedule(long globalId) : base(globalId)
        {
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegularIntervalSchedule x = (RegularIntervalSchedule)obj;
                return (CompareHelper.CompareLists(x.TimePoints, this.TimePoints, true));
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
                case ModelCode.REGULAR_IS_REF:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.REGULAR_IS_REF:
                    prop.SetValue(timePoints);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        //nista ovde ne treba
        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
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
                return timePoints.Count != 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (timePoints != null && timePoints.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULAR_IS_REF] = timePoints.GetRange(0, timePoints.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                //pazi da dodas referencu od ovog drugog tj od Regular Time Point-a
                case ModelCode.REGULAR_TIME_POINT_REF:
                    timePoints.Add(globalId);
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
                case ModelCode.REGULAR_TIME_POINT_REF:
                    if (timePoints.Contains(globalId))
                        timePoints.Remove(globalId);
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
