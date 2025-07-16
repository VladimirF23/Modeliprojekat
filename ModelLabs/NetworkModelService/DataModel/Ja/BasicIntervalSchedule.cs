using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class BasicIntervalSchedule : IdentifiedObject
    {
        //proveri da li trebaju enumi ovde

        public BasicIntervalSchedule(long globalId) : base(globalId)
        {}

        private UnitMultiplier value1Multiplier;
        private DateTime startTime;
        private UnitSymbol value1Unit;
        private UnitMultiplier value2Multiplier;
        private UnitSymbol value2Unit;

        public UnitMultiplier Value1Multiplier { get => value1Multiplier; set => value1Multiplier = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public UnitSymbol Value1Unit { get => value1Unit; set => value1Unit = value; }
        public UnitMultiplier Value2Multiplier { get => value2Multiplier; set => value2Multiplier = value; }

        public UnitSymbol Value2Unit { get => value2Unit; set => value2Unit = value; }




        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.BASIC_IS_STARTIME:
                case ModelCode.BASIC_IS_VALUE1_MULTIPLIER:
                case ModelCode.BASIC_IS_VALUE1_UNIT:
                case ModelCode.BASIC_IS_VALUE2_MULTIPLIER:
                case ModelCode.BASIC_IS_VALUE2_UNIT:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                //pazi na enume !
                case ModelCode.BASIC_IS_VALUE1_MULTIPLIER:
                    prop.SetValue((short)value1Multiplier);
                    break;

                case ModelCode.BASIC_IS_VALUE1_UNIT:
                    prop.SetValue((short)value1Unit);
                    break;
                case ModelCode.BASIC_IS_VALUE2_MULTIPLIER:
                    prop.SetValue((short)value2Multiplier);
                    break;
                case ModelCode.BASIC_IS_VALUE2_UNIT:
                    prop.SetValue((short)value2Unit);
                    break;
                case ModelCode.BASIC_IS_STARTIME:
                    prop.SetValue(startTime);
                    break;


                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.BASIC_IS_VALUE1_MULTIPLIER:
                    value1Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.BASIC_IS_VALUE1_UNIT:
                    value1Unit = (UnitSymbol)property.AsEnum();
                    break;

                case ModelCode.BASIC_IS_VALUE2_MULTIPLIER:
                    value2Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.BASIC_IS_VALUE2_UNIT:
                    value2Unit = (UnitSymbol)property.AsEnum();
                    break;

                case ModelCode.BASIC_IS_STARTIME:
                    startTime = property.AsDateTime();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

    }
}
