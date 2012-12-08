using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// A music segment in a sound bank.
    /// </summary>
    public class SoundBankMusicSegment : IWwiseObject
    {
        public SoundBankMusicSegment(IReader reader, uint id)
        {
            ID = id;

            Info = new SoundInfo(reader);

            // Read child IDs
            int numChildren = reader.ReadInt32();
            ChildIDs = new uint[numChildren];
            for (int i = 0; i < numChildren; i++)
                ChildIDs[i] = reader.ReadUInt32();
        }

        /// <summary>
        /// The segment's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Sound information about the segment.
        /// </summary>
        public SoundInfo Info { get; private set; }

        /// <summary>
        /// The IDs of the child sound objects.
        /// </summary>
        public uint[] ChildIDs { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankMusicSegment) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
