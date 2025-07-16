using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class ProtectedSwitch : Switch
    {
        private float breakingCapacity;

        public float BreakingCapcity { get => breakingCapacity; set => breakingCapacity = value; }

        public ProtectedSwitch(long globalId) : base(globalId)
        {
        }


        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                ProtectedSwitch x = obj as ProtectedSwitch;
                return (x.breakingCapacity == this.breakingCapacity);
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
                case ModelCode.PROTECTED_SWITCH_BREAKING_CAPACITY:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PROTECTED_SWITCH_BREAKING_CAPACITY:
                    property.SetValue(breakingCapacity);
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
                case ModelCode.PROTECTED_SWITCH_BREAKING_CAPACITY:
                    breakingCapacity = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation
    }
}

