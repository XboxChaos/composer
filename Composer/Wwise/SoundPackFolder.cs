using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Composer.Wwise
{
    /// <summary>
    /// A folder in a sound pack.
    /// </summary>
    public class SoundPackFolder
    {
        public SoundPackFolder(string name)
        {
            Name = name;
            Files = new List<SoundPackFile>();
        }

        /// <summary>
        /// Adds a file to the folder.
        /// </summary>
        /// <param name="file">The file to add.</param>
        public void AddFile(SoundPackFile file)
        {
            Files.Add(file);
        }

        /// <summary>
        /// The folder's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The files in the folder.
        /// </summary>
        public IList<SoundPackFile> Files { get; private set; }

        /// <summary>
        /// Returns the folder's name.
        /// </summary>
        /// <returns>The folder's name.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
