using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    public enum VoiceStorageType : int
    {
        Embedded,
        Streamed,
        StreamedAndPrefetched
    }

    /// <summary>
    /// A voice in a sound bank.
    /// </summary>
    public class SoundBankVoice : IWwiseObject
    {
        public SoundBankVoice(IReader reader, uint id)
        {
            ID = id;

            reader.Skip(4);
            StorageType = (VoiceStorageType)reader.ReadInt32();
            AudioID = reader.ReadUInt32();
            SourceID = reader.ReadUInt32();
        }

        /// <summary>
        /// The voice's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// How the voice's sound data is stored.
        /// </summary>
        public VoiceStorageType StorageType { get; private set; }

        /// <summary>
        /// The ID of the audio file associated with the voice.
        /// </summary>
        public uint AudioID { get; private set; }

        /// <summary>
        /// The ID of the source object associated with the audio file.
        /// If the file is embedded, this will be the ID of the sound bank it's embedded in. Otherwise, this will be the same as AudioID.
        /// </summary>
        public uint SourceID { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankVoice) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
