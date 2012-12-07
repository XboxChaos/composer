using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    public enum StateGroupChangeMoment : sbyte
    {
        Immediate,
        NextGrid,
        NextBar,
        NextBeat,
        NextCue,
        CustomCue,
        EntryCue,
        ExitCue
    }

    public class CustomState
    {
        public CustomState(IReader reader)
        {
            ID = reader.ReadUInt32();
            SettingsID = reader.ReadUInt32();
        }

        /// <summary>
        /// The custom state's ID?
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// The ID of the settings object for the state.
        /// </summary>
        public uint SettingsID { get; private set; }
    }

    /// <summary>
    /// Represents a state group in a SoundInfo object.
    /// </summary>
    public class StateGroup
    {
        public StateGroup(IReader reader)
        {
            ID = reader.ReadUInt32();
            ChangeMoment = (StateGroupChangeMoment)reader.ReadSByte();

            // Read custom states
            short numCustomStates = reader.ReadInt16();
            CustomStates = new CustomState[numCustomStates];
            for (short i = 0; i < numCustomStates; i++)
                CustomStates[i] = new CustomState(reader);
        }

        /// <summary>
        /// The state group's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// If the music is interactive, describes when the state change occurs.
        /// </summary>
        public StateGroupChangeMoment ChangeMoment { get; private set; }

        /// <summary>
        /// States whose settings are different from the default settings.
        /// </summary>
        public CustomState[] CustomStates { get; private set; }
    }
}
