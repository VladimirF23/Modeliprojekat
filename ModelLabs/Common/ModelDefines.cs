using System;
using System.Collections.Generic;
using System.Text;

namespace FTN.Common
{
	
	public enum DMSType : short
	{		
		MASK_TYPE		 = unchecked((short)0xFFFF),

        SEASON			 =	0x0001,
		DAYTYPE			 =	0x0002,
		REGULARTIMEPOINT =	0x0003,
		SWITCHSCHEDULE	 =	0x0004,
		BREAKER			 =	0x0005,
		RECLOSER		 =	0x0006,
		LOADBREAKSWITCH  =	0x0007,

	}

    [Flags]
	public enum ModelCode : long
	{
        IDOBJ       =                   0x1000000000000000,
        IDOBJ_GID   =                   0x1000000000000104,
        IDOBJ_MRID  =                   0x1000000000000207,
        IDOBJ_NAME  =                   0x1000000000000307,
        IDOBJ_ALIASNAME =               0x1000000000000407,
        SEASON      =                   0x1100000000010000,
        SEASON_END_DATE =               0x1100000000010108,
        SEASON_START_DATE =             0x1100000000010208,
        SEASON_REF        =             0x1100000000010319,

        DAYTYPE =                       0x1200000000020000,
        DAYTYPE_REF =                   0x1200000000020119,

        REGULAR_TIME_POINT =            0x1300000000030000,
        REGULAR_TIME_POINT_SQ_NUM =     0x1300000000030103,
        REGULAR_TIME_POINT_VALUE1 =     0x1300000000030205,
        REGULAR_TIME_POINT_VALUE2 =     0x1300000000030305,
        REGULAR_TIME_POINT_REF =        0x1300000000030409,

        BASIC_IS =                      0x1400000000000000,
        BASIC_IS_STARTIME =             0x1400000000000108,
        BASIC_IS_VALUE1_MULTIPLIER =    0x140000000000020a,
        BASIC_IS_VALUE1_UNIT =          0x140000000000030a,
        BASIC_IS_VALUE2_MULTIPLIER =    0x140000000000040a,
        BASIC_IS_VALUE2_UNIT =          0x140000000000050a,

        POWER_SYSTEM_RESOURCE =         0x1500000000000000,

        REGULAR_IS =                    0x1410000000000000,
        REGULAR_IS_REF =                0x1410000000000119,

        EQUIPMENT =                     0x1510000000000000,
        EQUIPMENT_AGGREGATE =           0x1510000000000101,
        EQUIPMENT_NORMALY_IN_SERVICE =  0x1510000000000201,

        SEASON_DAY_TYPE_S =             0x1411000000000000,
        SEASON_DAY_TYPE_S_REF1 =        0x1411000000000109,
        SEASON_DAY_TYPE_S_REF2 =        0x1411000000000209,

        CONDUCTING_EQUIPMENT =          0x1511000000000000,

        SWITCH_SCHEDULE =               0x1411100000040000,
        SWITCH_SCHEDULE_REF =           0x1411100000040109,

        SWITCH =                        0x1511100000000000,
        SWITCH_NORMAL_OPEN =            0x1511100000000101,
        SWITCH_RATED_CURRENT =          0x1511100000000205,
        SWITCH_RETAINED =               0x1511100000000301,
        SWITCH_ON_COUNT =               0x1511100000000403, 
        SWITCH_ON_DATE =                0x1511100000000508,
        SWITCH_REF =                    0x1511100000000619,

        PROTECTED_SWITCH =              0x1511110000000000,
        PROTECTED_SWITCH_BREAKING_CAPACITY = 0x1511110000000105,

        BREAKER =                       0x1511111000050000,
        BREAKER_IN_TRANSIT_TIME =       0x1511111000050105,

        RECLOSER =                      0x1511112000060000,
        LOAD_BREAK_SWITCH =             0x1511113000070000

    }

    [Flags]
	public enum ModelCodeMask : long
	{
		MASK_TYPE			 = 0x00000000ffff0000,
		MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
		MASK_ATTRIBUTE_TYPE	 = 0x00000000000000ff,

		MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
		MASK_FIRSTNBL		  = unchecked((long)0xf000000000000000),
		MASK_DELFROMNBL8	  = unchecked((long)0xfffffff000000000),		
	}																		
}


