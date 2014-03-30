using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// A file in a sound pack.
    /// </summary>
    public class SoundPackFile : IWwiseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundPackFile"/> class.
        /// </summary>
        /// <param name="reader">The reader to read the file from.</param>
        public SoundPackFile(SoundPackFileType type, IReader reader)
        {
            Type = type;
            ID = reader.ReadUInt32();
            Flags = reader.ReadUInt32();
            Size = reader.ReadInt32();
            Offset = reader.ReadInt32();
            FolderIndex = reader.ReadInt32();
        }

        /// <summary>
        /// Gets the file's type.
        /// </summary>
        public SoundPackFileType Type { get; private set; }

        /// <summary>
        /// Gets the file's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Gets flags for the file.
        /// </summary>
        public uint Flags { get; private set; }

        /// <summary>
        /// Gets the file's size in bytes.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Gets the offset of the file within the pack.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// Gets the index of the folder that the sound belongs to.
        /// </summary>
        public int FolderIndex { get; private set; }

        /// <summary>
        /// Calls the Visit(SoundPackFile) method on an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="visitor">The visitor to call the method on.</param>
        public void Accept(IWwiseObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
