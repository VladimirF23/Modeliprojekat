using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Ja
{
    public class SeasonDayTypeSchedule : RegularIntervalSchedule
    {
        public SeasonDayTypeSchedule(long globalId) : base(globalId) { }

        //treba dodati polje za DayType ref i Season
        //ovo je kljuc
        private long dayType = 0;

        public long DayType { get => dayType; set => dayType = value; }

        private long season = 0;

        public long Season { get => season; set => season = value; }

        //REFRENCE

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            //pitaj zokija kako ovo ide kada imamo 2 reference
            if (dayType != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SEASON_DAY_TYPE_S_REF1] = new List<long>();
                references[ModelCode.SEASON_DAY_TYPE_S_REF2] = new List<long>();

                references[ModelCode.SEASON_DAY_TYPE_S_REF1].Add(season);

                references[ModelCode.SEASON_DAY_TYPE_S_REF2].Add(dayType);

            }

            base.GetReferences(references, refType);
        }


        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SeasonDayTypeSchedule x = obj as SeasonDayTypeSchedule;
                return (x.season == this.season &&
                        x.dayType == this.dayType);
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
                case ModelCode.SEASON_DAY_TYPE_S_REF1:
                case ModelCode.SEASON_DAY_TYPE_S_REF2:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                //ref1 je za season
                case ModelCode.SEASON_DAY_TYPE_S_REF1:
                    property.SetValue(season);
                    break;

                case ModelCode.SEASON_DAY_TYPE_S_REF2:
                    property.SetValue(dayType);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        //kada se dobija objekat od servera 
        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {

                case ModelCode.SEASON_DAY_TYPE_S_REF1:
                    season = property.AsReference();
                    break;

                case ModelCode.SEASON_DAY_TYPE_S_REF2:
                    dayType = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation


    }
}
