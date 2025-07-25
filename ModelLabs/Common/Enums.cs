﻿using System;

namespace FTN.Common
{	
	public enum PhaseCode : short
	{
		Unknown = 0x0,
		N = 0x1,
		C = 0x2,
		CN = 0x3,
		B = 0x4,
		BN = 0x5,
		BC = 0x6,
		BCN = 0x7,
		A = 0x8,
		AN = 0x9,
		AC = 0xA,
		ACN = 0xB,
		AB = 0xC,
		ABN = 0xD,
		ABC = 0xE,
		ABCN = 0xF
	}
	
	public enum TransformerFunction : short
	{
		Supply = 1,				// Supply transformer
		Consumer = 2,			// Transformer supplying a consumer
		Grounding = 3,			// Transformer used only for grounding of network neutral
		Voltreg = 4,			// Feeder voltage regulator
		Step = 5,				// Step
		Generator = 6,			// Step-up transformer next to a generator.
		Transmission = 7,		// HV/HV transformer within transmission network.
		Interconnection = 8		// HV/HV transformer linking transmission network with other transmission networks.
	}
	
	public enum WindingConnection : short
	{
		Y = 1,		// Wye
		D = 2,		// Delta
		Z = 3,		// ZigZag
		I = 4,		// Single-phase connection. Phase-to-phase or phase-to-ground is determined by elements' phase attribute.
		Scott = 5,   // Scott T-connection. The primary winding is 2-phase, split in 8.66:1 ratio
		OY = 6,		// 2-phase open wye. Not used in Network Model, only as result of Topology Analysis.
		OD = 7		// 2-phase open delta. Not used in Network Model, only as result of Topology Analysis.
	}



    public enum UnitMultiplier : short
    {
        c = 0x0,
        d = 0x1,
        G = 0x2,
        k = 0x3,
        m = 0x4,
        M = 0x5,
        micro = 0x6,
        n = 0x7,
        none = 0x8,
        p = 0x9,
        T = 0xA,
    }

    public enum UnitSymbol : short
    {
        A = 0x0,
        deg = 0x1,
        degC = 0x2,
        F = 0x3,
        g = 0x4,
        h = 0x5,
        H = 0x6,
        Hz = 0x7,
        J = 0x8,
        m = 0x9,
        m2 = 0xA,
        m3 = 0xB,
        min = 0xC,
        N = 0xD,
        none = 0xE,
        ohm = 0xF,
        Pa = 0x10,
        rad = 0x11,
        s = 0x12,
        S = 0x13,
        V = 0x14,
        VA = 0x15,
        VAh = 0x16,
        VAr = 0x17,
        VArh = 0x18,
        W = 0x19,
        Wh = 0x1A,
    }




    public enum WindingType : short
	{
		None = 0,
		Primary = 1,
		Secondary = 2,
		Tertiary = 3
	}			
}
