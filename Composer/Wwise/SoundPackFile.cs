using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// A file in a sound _currentPack.
    /// </summary>
    public class SoundPackFile
    {
        public SoundPackFile(IReader reader)
        {
            ID = reader.ReadUInt32();
            reader.Skip(4); // Flags?
            Size = reader.ReadInt32();
            Offset = reader.ReadInt32();
            FolderID = reader.ReadInt32();
        }

        /// <summary>
        /// The file's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// The file's size in bytes.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// The offset of the file within the pack.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// The ID of the folder that the sound belongs to.
        /// </summary>
        public int FolderID { get; private set; }
    }
}
