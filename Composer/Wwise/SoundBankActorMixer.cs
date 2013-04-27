using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// An actor-mixer in a sound bank.
    /// </summary>
    public class SoundBankActorMixer : IWwiseObject
    {
        public SoundBankActorMixer(IReader reader, uint id)
        {
            ID = id;

            Info = new SoundInfo(reader);

            // Actor-mixers are just a list of children
            int numChildren = reader.ReadInt32();
            ChildIDs = new uint[numChildren];
            for (int i = 0; i < numChildren; i++)
                ChildIDs[i] = reader.ReadUInt32();
        }

        /// <summary>
        /// The actor-mixer's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Sound information about the actor-mixer.
        /// </summary>
        public SoundInfo Info { get; private set; }

        /// <summary>
        /// The IDs of the actor-mixer's children.
        /// </summary>
        public uint[] ChildIDs { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankActorMixer) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
