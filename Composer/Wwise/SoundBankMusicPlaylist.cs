using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    public class SoundBankMusicPlaylist : IWwiseObject
    {
        public SoundBankMusicPlaylist(IReader reader, uint id)
        {
            ID = id;

            Info = new SoundInfo(reader);

            // Read segment IDs
            int numSegments = reader.ReadInt32();
            SegmentIDs = new uint[numSegments];
            for (int i = 0; i < numSegments; i++)
                SegmentIDs[i] = reader.ReadUInt32();

            // TODO: read the rest of the data
        }

        /// <summary>
        /// The playlist's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Sound information about the playlist.
        /// </summary>
        public SoundInfo Info { get; private set; }

        /// <summary>
        /// The IDs of the music segments in the playlist.
        /// </summary>
        public uint[] SegmentIDs { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankMusicPlaylist) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
