using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class RegularTimePoint : IdentifiedObject
    {
        public RegularTimePoint(long globalId) : base(globalId)
        {
        }
        //ovo je kljuc kom intervaluSchedulu ovaj RegularTimePoint pripada
        private long intervalSchedule = 0;

        public long IntervalSchedule { get => intervalSchedule; set => intervalSchedule = value; }



        private int sequenceNumber;

        private float value1;
        private float value2;

        public int SequenceNumber { get => sequenceNumber; set => sequenceNumber = value; }
        public float Value1 { get => value1; set => value1 = value; }
        public float Value2 { get => value2; set => value2 = value; }


        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegularTimePoint x = obj as RegularTimePoint;
                return (x.intervalSchedule == this.intervalSchedule &&
                        x.sequenceNumber == this.sequenceNumber &&
                        x.value1 == this.value1 && x.value2 == this.value2);
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
                
                case ModelCode.REGULAR_TIME_POINT_SQ_NUM:
                case ModelCode.REGULAR_TIME_POINT_REF:          //refrence na (RegularIntervalSchedule)
                case ModelCode.REGULAR_TIME_POINT_VALUE1:
                case ModelCode.REGULAR_TIME_POINT_VALUE2:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                
                case ModelCode.REGULAR_TIME_POINT_SQ_NUM:
                    property.SetValue(sequenceNumber);
                    break;

                case ModelCode.REGULAR_TIME_POINT_REF:
                    property.SetValue(intervalSchedule);
                    break;

                case ModelCode.REGULAR_TIME_POINT_VALUE1:
                    property.SetValue(value1);
                    break;
                case ModelCode.REGULAR_TIME_POINT_VALUE2:
                    property.SetValue(value2);
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
                //pazi na int i na refrence ovde
                case ModelCode.REGULAR_TIME_POINT_SQ_NUM:
                    sequenceNumber = property.AsInt();
                    break;

                case ModelCode.REGULAR_TIME_POINT_REF:
                    intervalSchedule = property.AsReference();
                    break;

                case ModelCode.REGULAR_TIME_POINT_VALUE1:
                    value1 = property.AsFloat();
                    break;

                case ModelCode.REGULAR_TIME_POINT_VALUE2:
                    value2 = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation


        //REFRENCE

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            
            if (intervalSchedule != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULAR_TIME_POINT_REF] = new List<long>();
                references[ModelCode.REGULAR_TIME_POINT_REF].Add(intervalSchedule);

            }

            base.GetReferences(references, refType);
        }
    }
}
