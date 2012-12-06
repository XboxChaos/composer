using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace Composer
{
    /// <summary>
    /// Holds a collection of IDs which can be translated to names.
    /// </summary>
    public class NameLookup
    {
        private Dictionary<uint, string> _lookup = new Dictionary<uint, string>();

        /// <summary>
        /// Associates an ID number with a name.
        /// </summary>
        /// <param name="id">The ID number to register.</param>
        /// <param name="name">The name to associate with the ID.</param>
        public void Add(uint id, string name)
        {
            _lookup[id] = name;
        }

        /// <summary>
        /// Translates an ID into a name string and returns it.
        /// </summary>
        /// <param name="id">The ID to translate.</param>
        /// <returns>The name associated with the ID, or null if none.</returns>
        public string FindName(uint id)
        {
            string name;
            if (_lookup.TryGetValue(id, out name))
                return name;
            return null;
        }
    }
}
