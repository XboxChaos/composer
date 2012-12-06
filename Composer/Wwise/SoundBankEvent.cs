using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// An event in a sound bank.
    /// </summary>
    public class SoundBankEvent : IWwiseObject
    {
        public SoundBankEvent(IReader reader, uint id)
        {
            ID = id;

            // Read the action list
            int numActions = reader.ReadInt32();
            ActionIDs = new uint[numActions];
            for (int i = 0; i < numActions; i++)
                ActionIDs[i] = reader.ReadUInt32();
        }

        /// <summary>
        /// The list of action IDs in the event.
        /// </summary>
        public uint[] ActionIDs { get; private set; }

        /// <summary>
        /// The event's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankEvent) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
