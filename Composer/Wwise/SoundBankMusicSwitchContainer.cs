using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// A music switch container in a sound bank.
    /// </summary>
    public class SoundBankMusicSwitchContainer : IWwiseObject
    {
        public SoundBankMusicSwitchContainer(IReader reader, uint id)
        {
            ID = id;

            Info = new SoundInfo(reader);

            // Read segment IDs
            // TODO: this is pretty similar to SoundBankMusicPlaylist,
            // maybe this can be factored out into a common class somehow?
            int numSegments = reader.ReadInt32();
            SegmentIDs = new uint[numSegments];
            for (int i = 0; i < numSegments; i++)
                SegmentIDs[i] = reader.ReadUInt32();

            // TODO: read the rest of the data
        }

        /// <summary>
        /// The switch container's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Sound information about the container.
        /// </summary>
        public SoundInfo Info { get; private set; }

        /// <summary>
        /// The IDs of the music segments in the container.
        /// </summary>
        public uint[] SegmentIDs { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankMusicSwitchContainer) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
