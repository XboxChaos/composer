using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    public enum RTPCYAxisType : int
    {
        VoiceVolume,
        VoiceLowPassFilter = 0x3,
        Priority = 0x8,
        SoundInstanceLimit,
        UserDefinedAuxiliarySendsVolume0 = 0xF,
        UserDefinedAuxiliarySendsVolume1,
        UserDefinedAuxiliarySendsVolume2,
        UserDefinedAuxiliarySendsVolume3,
        GameDefinedAuxiliarySendsVolume,
        OutputBusVolume = 0x16,
        OutputBussLowPassFilter,
        BypassEffect0,
        BypassEffect1,
        BypassEffect2,
        BypassEffect3,
        BypassAllEffects,
        MotionVolumeOffset,
        MotionLowPass
    }

    public enum RTPCCurveShape : int
    {
        Logarithmic3,
        SineConstantPowerFadeIn,
        Logarithmic1_41,
        InvertedSCurve,
        Linear,
        SCurve,
        Exponential1_41,
        SineConstantPowerFadeOut,
        Exponential3,
        Constant
    }

    public class RTPCPoint
    {
        public RTPCPoint(IReader reader)
        {
            X = reader.ReadFloat();
            Y = reader.ReadFloat();
            CurveShape = (RTPCCurveShape)reader.ReadInt32();
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public RTPCCurveShape CurveShape { get; private set; }
    }

    /// <summary>
    /// Represents a Wwise Real-Time Parameter Control (RTPC).
    /// </summary>
    public class RTPC
    {
        public RTPC(IReader reader)
        {
            XAxisParameterID = reader.ReadUInt32();
            YAxisType = (RTPCYAxisType)reader.ReadInt32();

            reader.Skip(5);
            short numPoints = reader.ReadInt16();
            Points = new RTPCPoint[numPoints];

            // Read points
            for (byte i = 0; i < numPoints; i++)
                Points[i] = new RTPCPoint(reader);
        }

        public uint XAxisParameterID { get; private set; }
        public RTPCYAxisType YAxisType { get; private set; }
        public RTPCPoint[] Points { get; private set; }
    }
}
