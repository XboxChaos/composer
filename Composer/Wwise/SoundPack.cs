using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composer.IO;

namespace Composer.Wwise
{
    public class SoundPack
    {
        private int _folderListSize;
        private Dictionary<int, SoundPackFolder> _foldersById = new Dictionary<int, SoundPackFolder>(); // Maps folder IDs to their objects
        private Dictionary<uint, SoundPackFile> _filesById = new Dictionary<uint, SoundPackFile>();
        private WwiseObjectCollection _objects = new WwiseObjectCollection();

        const uint HeaderMagic = 0x414B504B; // 'AKPK'
        const int FolderListStart = 0x1C; // File offset of the folder list

        /// <summary>
        /// Loads a SoundPack from a stream.
        /// </summary>
        /// <param name="reader">The EndianReader to read from.</param>
        public SoundPack(EndianReader reader)
        {
            ReadHeader(reader);
            ReadFolderTable(reader);
            ReadFiles(reader);
        }

        /// <summary>
        /// Finds a folder in the pack by ID.
        /// </summary>
        /// <param name="id">The folder's ID number.</param>
        /// <returns>The SoundPackFolder if found, or null otherwise.</returns>
        public SoundPackFolder FindFolderByID(int id)
        {
            SoundPackFolder result;
            if (_foldersById.TryGetValue(id, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Finds a file in the pack by ID.
        /// </summary>
        /// <param name="id">The ID of the file to find.</param>
        /// <returns>The SoundPackFile if found, or null otherwise.</returns>
        public SoundPackFile FindFileByID(uint id)
        {
            SoundPackFile result;
            if (_filesById.TryGetValue(id, out result))
                return result;
            return null;
        }

        /// <summary>
        /// The folders in the sound pack, not stored in any particular order.
        /// </summary>
        public IEnumerable<SoundPackFolder> Folders
        {
            get { return _foldersById.Values; }
        }

        /// <summary>
        /// The Wwise objects stored in the sound pack.
        /// </summary>
        public WwiseObjectCollection Objects
        {
            get { return _objects; }
        }

        private void ReadHeader(EndianReader reader)
        {
            reader.Endianness = Endian.BigEndian;
            reader.SeekTo(0);

            // Validate the magic number at the beginning
            if (reader.ReadUInt32() != HeaderMagic)
                throw new InvalidOperationException("Invalid sound pack magic");

            reader.Skip(4 * 2); // Skip two unknown uint32s

            // Read the size of the folder list (needed to find the start of the file table)
            _folderListSize = reader.ReadInt32();
        }

        private void ReadFolderTable(EndianReader reader)
        {
            // Seek to the beginning of the folder list
            reader.Endianness = Endian.BigEndian;
            reader.SeekTo(FolderListStart);

            // Read the number of folders
            int folderCount = reader.ReadInt32();

            // Sort the folders into a list sorted by offset
            SortedList<int, int> folderOffsets = new SortedList<int, int>(); // Maps offset -> ID, sorted by offset
            for (int i = 0; i < folderCount; i++)
            {
                int offset = reader.ReadInt32();
                int id = reader.ReadInt32();
                folderOffsets.Add(offset, id);
            }

            // Read the folder names and create wrappers for them
            foreach (KeyValuePair<int, int> offset in folderOffsets)
            {
                reader.SeekTo(FolderListStart + offset.Key); // The name's offset is relative to the start of the folder list
                string name = reader.ReadAscii();

                _foldersById[offset.Value] = new SoundPackFolder(name);
            }
        }

        private void ReadFiles(EndianReader reader)
        {
            // The file table comes after the folder list and padding
            reader.SeekTo(FolderListStart + _folderListSize);

            // Align 4
            reader.SeekTo((reader.Position + 3) & ~3);

            // TODO: Load these into separate lists or something to make stuff easier
            ReadFileTable(reader); // Sound banks
            ReadFileTable(reader); // Global files
        }

        private void ReadFileTable(EndianReader reader)
        {
            int fileCount = reader.ReadInt32();

            // Read each file and sort it into its folder
            for (int i = 0; i < fileCount; i++)
            {
                SoundPackFile file = new SoundPackFile(this, reader);

                // Put the file into its parent folder
                SoundPackFolder folder = FindFolderByID(file.FolderID);
                if (folder != null)
                    folder.AddFile(file);

                // Associate its ID
                _filesById[file.ID] = file;
                _objects.Add(file);
            }
        }
    }
}
