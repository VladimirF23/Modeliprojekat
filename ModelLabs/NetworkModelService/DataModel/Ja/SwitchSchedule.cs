using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class SwitchSchedule : SeasonDayTypeSchedule
    {
        public SwitchSchedule(long globalId) : base(globalId)
        {
        }

        //switch schedule pripada switchu
        private long switch_id=0;

        public long Switch_id { get => switch_id; set => switch_id = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SwitchSchedule x = obj as SwitchSchedule;
                return (x.switch_id == this.switch_id);
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

                case ModelCode.SWITCH_SCHEDULE_REF: //referencana 

                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {

                case ModelCode.SWITCH_SCHEDULE_REF:
                    property.SetValue(switch_id);
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
                //pazi na  i na refrence ovde
                case ModelCode.SWITCH_SCHEDULE_REF:
                    switch_id = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {


            if (switch_id != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SWITCH_SCHEDULE_REF] = new List<long>();
                references[ModelCode.SWITCH_SCHEDULE_REF].Add(switch_id);

            }

            base.GetReferences(references, refType);
        }
    }
}

