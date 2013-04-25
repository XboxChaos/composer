using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    public enum SoundBankActionScope : sbyte
    {
        SwitchOrTriggerObject = 1,
        Global,
        Object,
        State,
        All,
        AllExceptObject = 9
    }

    public enum SoundBankActionType : sbyte
    {
        Stop = 1,
        Pause,
        Resume,
        Play,
        Trigger,
        Mute,
        UnMute,
        SetVoicePitch,
        ResetVoicePitch,
        SetVoiceVolume,
        ResetVoiceVolume,
        SetBusVolume,
        ResetBusVolume,
        SetVoiceLowPassFilter,
        ResetVoiceLowPassFilter,
        EnableState,
        DisableState,
        SetState,
        SetGameParameter,
        ResetGameParameter,
        SetSwitch = 0x19,
        SetBypass,
        ResetBypass,
        Break,
        Seek = 0x1E
    }

    /// <summary>
    /// An event action in a sound bank.
    /// </summary>
    public class SoundBankAction : IWwiseObject
    {
        public SoundBankAction(IReader reader, uint id)
        {
            ID = id;

            Type = (SoundBankActionType)reader.ReadSByte();
            Scope = (SoundBankActionScope)reader.ReadSByte();
            ObjectID = reader.ReadUInt32();

            // TODO: read parameters and state/switch group IDs
        }

        /// <summary>
        /// The action's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// The action's scope.
        /// </summary>
        public SoundBankActionScope Scope { get; private set; }

        /// <summary>
        /// The action's type.
        /// </summary>
        public SoundBankActionType Type { get; private set; }

        /// <summary>
        /// The ID of the game object the event references. Can be zero.
        /// </summary>
        public uint ObjectID { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankAction) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
