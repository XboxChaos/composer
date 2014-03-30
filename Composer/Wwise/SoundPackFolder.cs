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
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundPackFolder"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
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
        /// Gets the folder's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a list of files in the folder.
        /// </summary>
        public IList<SoundPackFile> Files { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
