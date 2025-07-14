namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    using FTN.Common;

    /// <summary>
    /// PowerTransformerConverter has methods for populating
    /// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
    /// </summary>
    public static class PowerTransformerConverter
    {
        //U KONVERTERU ABSTRAKNE KLASE OPISUJEMO
        #region Populate ResourceDescription
        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
                }
                if (cimIdentifiedObject.AliasNameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
                }
            }
        }

        public static void PopulateSeasonProperties(FTN.Season cimSeason, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSeason != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimSeason, rd);

                if (cimSeason.StartDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEASON_START_DATE, cimSeason.StartDate));
                }
                if (cimSeason.EndDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEASON_END_DATE, cimSeason.EndDate));
                }
            }
        }


        public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cimBasicIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimBasicIntervalSchedule != null) && (rd != null))
            {

                //Roditelja poziva svaki PAAZI na ovo
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimBasicIntervalSchedule, rd);


                if (cimBasicIntervalSchedule.StartTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASIC_IS_STARTIME, cimBasicIntervalSchedule.StartTime));
                }
                if (cimBasicIntervalSchedule.Value1MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASIC_IS_VALUE1_MULTIPLIER, (short)GetDMSUnitMultiplier(cimBasicIntervalSchedule.Value1Multiplier)));
                }

                if (cimBasicIntervalSchedule.Value1UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASIC_IS_VALUE1_UNIT, (short)GetDMSUnitSymbol(cimBasicIntervalSchedule.Value1Unit)));
                }


                if (cimBasicIntervalSchedule.Value2MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASIC_IS_VALUE2_MULTIPLIER, (short)GetDMSUnitMultiplier(cimBasicIntervalSchedule.Value2Multiplier)));

                }
                if (cimBasicIntervalSchedule.Value2UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASIC_IS_VALUE2_UNIT, (short)GetDMSUnitSymbol(cimBasicIntervalSchedule.Value2Unit)));
                }
            }

        }

        public static void PopulateRegularIntervalScheduleProperties(FTN.RegularIntervalSchedule cimRegularIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegularIntervalSchedule != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateBasicIntervalScheduleProperties(cimRegularIntervalSchedule, rd, importHelper, report);

            }

        }


        public static void PopulateSeasonDayTypeScheduleProperties(FTN.SeasonDayTypeSchedule cimSeasonDayTypeSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSeasonDayTypeSchedule != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateRegularIntervalScheduleProperties(cimSeasonDayTypeSchedule, rd, importHelper, report);


                if (cimSeasonDayTypeSchedule.DayTypeHasValue)
                {

                    long gid = importHelper.GetMappedGID(cimSeasonDayTypeSchedule.DayType.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSeasonDayTypeSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSeasonDayTypeSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimSeasonDayTypeSchedule.DayType.ID).AppendLine(" \" is not mapped to GID!");
                    }


                    rd.AddProperty(new Property(ModelCode.SEASON_DAY_TYPE_S_REF1, gid));

                }

                if (cimSeasonDayTypeSchedule.SeasonHasValue)
                {

                    long gid = importHelper.GetMappedGID(cimSeasonDayTypeSchedule.Season.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSeasonDayTypeSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSeasonDayTypeSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimSeasonDayTypeSchedule.Season.ID).AppendLine(" \" is not mapped to GID!");
                    }


                    rd.AddProperty(new Property(ModelCode.SEASON_DAY_TYPE_S_REF2, gid));
                }
            }
        }

        public static void PopulateSwitchScheduleProperties(FTN.SwitchSchedule cimSwitchSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSwitchSchedule != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateSeasonDayTypeScheduleProperties(cimSwitchSchedule, rd, importHelper, report);

                if (cimSwitchSchedule.SwitchHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSwitchSchedule.Switch.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSwitchSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSwitchSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimSwitchSchedule.Switch.ID).AppendLine(" \" is not mapped to GID!");
                    }

                    rd.AddProperty(new Property(ModelCode.SWITCH_SCHEDULE_REF, gid));

                }

            }

        }

        public static void PopulateDayTypeProperties(FTN.DayType cimDayType, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimDayType != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimDayType, rd);


            }

        }

        public static void PopulateRegularTimePointProperties(FTN.RegularTimePoint cimRegularTimePoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegularTimePoint != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimRegularTimePoint, rd);


                if (cimRegularTimePoint.SequenceNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULAR_TIME_POINT_SQ_NUM, cimRegularTimePoint.SequenceNumber));
                }
                if (cimRegularTimePoint.Value1HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULAR_TIME_POINT_VALUE1, cimRegularTimePoint.Value1));
                }

                if (cimRegularTimePoint.Value2HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULAR_TIME_POINT_VALUE2, cimRegularTimePoint.Value2));
                }




                if (cimRegularTimePoint.IntervalScheduleHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRegularTimePoint.IntervalSchedule.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRegularTimePoint.GetType().ToString()).Append(" rdfID = \"").Append(cimRegularTimePoint.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimRegularTimePoint.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
                    }

                    rd.AddProperty(new Property(ModelCode.REGULAR_TIME_POINT_REF, gid));

                }
            }


        }

        public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {

            if ((cimPowerSystemResource != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);


            }


        }

        public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);

                if (cimEquipment.AggregateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cimEquipment.Aggregate));
                }
                if (cimEquipment.NormallyInServiceHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_NORMALY_IN_SERVICE, cimEquipment.NormallyInService));
                }

            }


        }

        public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {

            if ((cimConductingEquipment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);



            }



        }
        public static void PopulateSwitchProperties(FTN.Switch cimSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSwitch != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateConductingEquipmentProperties(cimSwitch, rd, importHelper, report);

                if (cimSwitch.NormalOpenHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_NORMAL_OPEN, cimSwitch.NormalOpen));
                }
                if (cimSwitch.RatedCurrentHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_CURRENT_FLOW, cimSwitch.RatedCurrent));
                }


                if (cimSwitch.RetainedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RETAINED, cimSwitch.Retained));
                }

                if (cimSwitch.SwitchOnCountHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_ON_COUNT, cimSwitch.SwitchOnCount));
                }
                if (cimSwitch.SwitchOnDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_ON_DATE, cimSwitch.SwitchOnDate));
                }
            }
        }


        public static void PopulateProtectedSwitchProperties(FTN.ProtectedSwitch cimProtectedSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {

            if ((cimProtectedSwitch != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateSwitchProperties(cimProtectedSwitch, rd, importHelper, report);


                if (cimProtectedSwitch.BreakingCapacityHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PROTECTED_SWITCH_CURRENT_FLOW, cimProtectedSwitch.BreakingCapacity));
                }
            }

        }

        public static void PopulateBreakerProperties(FTN.Breaker cimBreaker, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {

            if ((cimBreaker != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateProtectedSwitchProperties(cimBreaker, rd, importHelper, report);


                if (cimBreaker.InTransitTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BREAKER_IN_TRANSIT_TIME, cimBreaker.InTransitTime));
                }
            }

        }


        public static void PopulateRecloserProperties(FTN.Recloser cimRecloser, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {

            if ((cimRecloser != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateProtectedSwitchProperties(cimRecloser, rd, importHelper, report);



            }

        }

        public static void PopulateLoadBreackSwitchProperties(FTN.LoadBreakSwitch cimLoadBrakeSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {

            if ((cimLoadBrakeSwitch != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateProtectedSwitchProperties(cimLoadBrakeSwitch, rd, importHelper, report);



            }

        }


        #endregion Populate ResourceDescription



        //URADJENI ENUMI
        #region Enums convert
        public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier multiplier)
        {
            switch (multiplier)
            {
                case FTN.UnitMultiplier.c:
                    return Common.UnitMultiplier.c;
                case FTN.UnitMultiplier.d:
                    return Common.UnitMultiplier.d;
                case FTN.UnitMultiplier.G:
                    return Common.UnitMultiplier.G;
                case FTN.UnitMultiplier.k:
                    return Common.UnitMultiplier.k;
                case FTN.UnitMultiplier.m:
                    return Common.UnitMultiplier.m;
                case FTN.UnitMultiplier.M:
                    return Common.UnitMultiplier.M;
                case FTN.UnitMultiplier.micro:
                    return Common.UnitMultiplier.micro;
                case FTN.UnitMultiplier.n:
                    return Common.UnitMultiplier.n;
                case FTN.UnitMultiplier.none:
                    return Common.UnitMultiplier.none;
                case FTN.UnitMultiplier.p:
                    return Common.UnitMultiplier.p;
                default: return Common.UnitMultiplier.T;
            }
        }

        public static Common.UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol unitSymbol)
        {
            switch (unitSymbol)
            {
                case FTN.UnitSymbol.A:
                    return Common.UnitSymbol.A;
                case FTN.UnitSymbol.deg:
                    return Common.UnitSymbol.deg;
                case FTN.UnitSymbol.degC:
                    return Common.UnitSymbol.degC;
                case FTN.UnitSymbol.F:
                    return Common.UnitSymbol.F;
                case FTN.UnitSymbol.g:
                    return Common.UnitSymbol.g;
                case FTN.UnitSymbol.h:
                    return Common.UnitSymbol.h;
                case FTN.UnitSymbol.H:
                    return Common.UnitSymbol.H;
                case FTN.UnitSymbol.Hz:
                    return Common.UnitSymbol.Hz;
                case FTN.UnitSymbol.J:
                    return Common.UnitSymbol.J;
                case FTN.UnitSymbol.m:
                    return Common.UnitSymbol.m;
                case FTN.UnitSymbol.m2:
                    return Common.UnitSymbol.m2;
                case FTN.UnitSymbol.m3:
                    return Common.UnitSymbol.m3;
                case FTN.UnitSymbol.min:
                    return Common.UnitSymbol.min;
                case FTN.UnitSymbol.N:
                    return Common.UnitSymbol.N;
                case FTN.UnitSymbol.none:
                    return Common.UnitSymbol.none;
                case FTN.UnitSymbol.ohm:
                    return Common.UnitSymbol.ohm;
                case FTN.UnitSymbol.Pa:
                    return Common.UnitSymbol.Pa;
                case FTN.UnitSymbol.rad:
                    return Common.UnitSymbol.rad;
                case FTN.UnitSymbol.s:
                    return Common.UnitSymbol.s;
                case FTN.UnitSymbol.S:
                    return Common.UnitSymbol.S;
                case FTN.UnitSymbol.V:
                    return Common.UnitSymbol.V;
                case FTN.UnitSymbol.VA:
                    return Common.UnitSymbol.VA;
                case FTN.UnitSymbol.VAh:
                    return Common.UnitSymbol.VAh;
                case FTN.UnitSymbol.VAr:
                    return Common.UnitSymbol.VAr;
                case FTN.UnitSymbol.VArh:
                    return Common.UnitSymbol.VArh;
                case FTN.UnitSymbol.W:
                    return Common.UnitSymbol.W;
                default:
                    return Common.UnitSymbol.Wh;
            }
        }

        #endregion Enums convert
    }
}




