using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class Breaker : ProtectedSwitch
    {
        private double inTransitTime;

        public double InTransitTime { get => inTransitTime; set => inTransitTime = value; }

        public Breaker(long globalId) : base(globalId)
        {
        }
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Breaker x = obj as Breaker;
                return (x.inTransitTime == this.inTransitTime);
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
                case ModelCode.BREAKER_IN_TRANSIT_TIME:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.BREAKER_IN_TRANSIT_TIME:
                    property.SetValue((double)inTransitTime);
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
                case ModelCode.BREAKER_IN_TRANSIT_TIME:
                    inTransitTime = property.AsFloat();         //treba da je asDouble ali ne postoji 
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation
    }


}

