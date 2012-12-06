using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// A file in a sound bank.
    /// </summary>
    public class SoundBankFile : IWwiseObject
    {
        public SoundBankFile(SoundBank parent, uint id, int offset, int size)
        {
            ParentBank = parent;
            ID = id;
            Offset = offset;
            Size = size;
        }

        public SoundBankFile(SoundBank parent, IReader reader)
        {
            ParentBank = parent;
            ID = reader.ReadUInt32();
            Offset = reader.ReadInt32();
            Size = reader.ReadInt32();
        }

        /// <summary>
        /// The SoundBank that the file belongs to.
        /// </summary>
        public SoundBank ParentBank { get; private set; }

        /// <summary>
        /// The file's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// The offset of the file within the sound bank's data area.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// The file's size in bytes.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundBankFile) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
