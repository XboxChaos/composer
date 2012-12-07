using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    public enum SoundPositionType : sbyte
    {
        Position2D,
        Position3D,
    }

    public enum SoundPositionSourceType : int
    {
        UserDefined = 2,
        GameDefined = 3
    }

    public enum SoundPlayType : int
    {
        SequenceStep,
        RandomStop,
        SequenceContinuous,
        RandomContinuous,
        SequenceStepPickPathAtStart,
        RandomStepPickPathAtStart
    }

    public enum SoundLimitMethod : sbyte
    {
        PerGameObject,
        Global
    }

    public enum SoundVirtualVoiceBehavior : sbyte
    {
        ContinuePlaying,
        Kill,
        SendToVirtualVoice
    }

    /// <summary>
    /// Represents information about a sound.
    /// </summary>
    public class SoundInfo
    {
        public SoundInfo(IReader reader)
        {
            OverrideParentEffectSettings = (reader.ReadByte() != 0);

            ReadEffects(reader);

            BusID = reader.ReadUInt32();
            ParentID = reader.ReadUInt32();

            OverrideParentPrioritySettings = (reader.ReadByte() != 0);
            OffsetPriorityAtMaxDistance = (reader.ReadByte() != 0);

            byte numParameters = reader.ReadByte();
            // TODO: actually store the parameter values instead of skipping over them
            reader.Skip(numParameters);
            reader.Skip(numParameters * 4);
            reader.Skip(1);

            HasPositioning = (reader.ReadByte() != 0);
            if (HasPositioning)
                ReadPositioningInfo(reader);

            // Read auxiliary send settings
            OverrideParentGameDefinedAuxiliarySendSettings = (reader.ReadByte() != 0);
            UseGameDefinedAuxiliarySends = (reader.ReadByte() != 0);
            OverrideParentUserDefinedAuxiliarySendSettings = (reader.ReadByte() != 0);
            HasUserDefinedAuxiliarySends = (reader.ReadByte() != 0);
            if (HasUserDefinedAuxiliarySends)
            {
                AuxiliaryBusIDs = new uint[4];
                for (int i = 0; i < 4; i++)
                    AuxiliaryBusIDs[i] = reader.ReadUInt32();
            }
            else
            {
                AuxiliaryBusIDs = new uint[0];
            }

            bool unknown = (reader.ReadByte() != 0);
            /*if (unknown)
                reader.Skip(4);*/

            LimitMethod = (SoundLimitMethod)reader.ReadSByte();
            VirtualVoiceBehavior = (SoundVirtualVoiceBehavior)reader.ReadSByte();
            OverrideParentPlaybackLimitSettings = (reader.ReadByte() != 0);
            OverrideParentVirtualVoiceSettings = (reader.ReadByte() != 0);

            ReadStateGroups(reader);
            ReadRTPCs(reader);

            reader.Skip(4); // I think this is part of the sound info data...
        }

        public bool OverrideParentEffectSettings { get; private set; }
        public byte EffectBypassMask { get; private set; }
        public uint[] EffectIDs { get; private set; }
        
        public uint BusID { get; private set; }
        public uint ParentID { get; private set; }

        public bool OverrideParentPrioritySettings { get; private set; }
        public bool OffsetPriorityAtMaxDistance { get; private set; }

        public bool HasPositioning { get; private set; }
        public SoundPositionType PositionType { get; private set; }
        public bool EnablePanner { get; private set; }
        public SoundPositionSourceType PositionSourceType { get; private set; }
        public uint AttenuationID { get; private set; }
        public bool EnableSpatialization { get; private set; }
        public SoundPlayType PlayType { get; private set; }
        public bool Loop { get; private set; }
        public uint TransitionTime { get; private set; }
        public bool FollowListenerOrientation { get; private set; }
        public bool UpdateEachFrame { get; private set; }

        public bool OverrideParentGameDefinedAuxiliarySendSettings { get; private set; }
        public bool UseGameDefinedAuxiliarySends { get; private set; }
        public bool OverrideParentUserDefinedAuxiliarySendSettings { get; private set; }
        public bool HasUserDefinedAuxiliarySends { get; private set; }
        public uint[] AuxiliaryBusIDs { get; private set; }

        public SoundLimitMethod LimitMethod { get; private set; }
        public SoundVirtualVoiceBehavior VirtualVoiceBehavior { get; private set; }

        public bool OverrideParentPlaybackLimitSettings { get; private set; }
        public bool OverrideParentVirtualVoiceSettings { get; private set; }

        public StateGroup[] StateGroups { get; private set; }
        public RTPC[] RTPCs { get; private set; }

        private void ReadPositioningInfo(IReader reader)
        {
            PositionType = (SoundPositionType)reader.ReadByte();
            if (PositionType == SoundPositionType.Position2D)
            {
                EnablePanner = (reader.ReadByte() != 0);
            }
            else
            {
                PositionSourceType = (SoundPositionSourceType)reader.ReadInt32();
                AttenuationID = reader.ReadUInt32();
                EnableSpatialization = (reader.ReadByte() != 0);

                if (PositionSourceType == SoundPositionSourceType.UserDefined)
                {
                    PlayType = (SoundPlayType)reader.ReadInt32();
                    Loop = (reader.ReadByte() != 0);
                    TransitionTime = reader.ReadUInt32();
                    FollowListenerOrientation = (reader.ReadByte() != 0);
                }
                else if (PositionSourceType == SoundPositionSourceType.GameDefined)
                {
                    UpdateEachFrame = (reader.ReadByte() != 0);
                }
            }
        }

        private void ReadEffects(IReader reader)
        {
            byte numEffects = reader.ReadByte();
            if (numEffects > 0)
            {
                EffectBypassMask = reader.ReadByte();
                EffectIDs = new uint[numEffects];
                for (byte i = 0; i < numEffects; i++)
                {
                    reader.Skip(1); // Effect index, useless
                    EffectIDs[i] = reader.ReadUInt32();
                    reader.Skip(2);
                }
            }
            else
            {
                EffectIDs = new uint[0];
            }
        }

        private void ReadStateGroups(IReader reader)
        {
            int numStateGroups = reader.ReadInt32();
            StateGroups = new StateGroup[numStateGroups];
            for (int i = 0; i < numStateGroups; i++)
                StateGroups[i] = new StateGroup(reader);
        }

        private void ReadRTPCs(IReader reader)
        {
            short numRtpcs = reader.ReadInt16();
            RTPCs = new RTPC[numRtpcs];
            for (short i = 0; i < numRtpcs; i++)
                RTPCs[i] = new RTPC(reader);
        }
    }
}
