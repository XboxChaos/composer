using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// A music track in a sound bank.
    /// </summary>
    public class SoundBankMusicTrack : IWwiseObject
    {
        public SoundBankMusicTrack(IReader reader, uint id)
        {
            ID = id;

            reader.Skip(0xC);
            AudioID = reader.ReadUInt32();
            SourceID = reader.ReadUInt32();
        }

        /// <summary>
        /// The music track's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// The ID of the audio file associated with the track.
        /// </summary>
        public uint AudioID { get; private set; }

        /// <summary>
        /// The ID of the source object associated with the track.
        /// If the file is embedded, this will be the ID of the sound bank it's embedded in. Otherwise, this will be the same as AudioID.
        /// </summary>
        public uint SourceID { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankMusicTrack) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
