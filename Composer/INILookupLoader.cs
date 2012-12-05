using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Composer
{
    /// <summary>
    /// Provides methods for loading IDLookups from INI-like files.
    /// Basically Ascension taglists.
    /// </summary>
    public static class INILookupLoader
    {
        /// <summary>
        /// Loads an IDLookup from a TextReader.
        /// </summary>
        /// <param name="reader">The TextReader to read from.</param>
        /// <returns>The IDLookup that was created.</returns>
        public static IDLookup Load(TextReader reader)
        {
            IDLookup result = new IDLookup();

            // Adapted from INITagList in Liberty, lul
            string line;
            int lineNumber = 0;
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                if (string.IsNullOrEmpty(line))
                    continue;

                // If there is a comment, only look at everything before it
                int commentPos = line.IndexOf(';');
                if (commentPos != -1)
                    line = line.Substring(0, commentPos);

                line = line.Trim();

                // Read a key = value pair
                int equalsPos = line.IndexOf('=');
                if (equalsPos == -1)
                    throw new ArgumentException("The ID list is invalid at line " + lineNumber + ":\r\nIDs must be stored as key = value pairs.");
                string idStr = line.Substring(0, equalsPos);
                string name = line.Substring(equalsPos + 1).TrimStart(null);

                // Parse it
                uint id;
                if (idStr.StartsWith("0x", true, null))
                {
                    // Parse a hex number
                    if (!uint.TryParse(idStr.Substring(2), NumberStyles.HexNumber, null, out id))
                        throw new ArgumentException("The ID list is invalid at line " + lineNumber + ":\r\nIDs starting with 0x must be hexadecimal integers.");
                }
                else
                {
                    // Parse a decimal number
                    if (!uint.TryParse(idStr, out id))
                        throw new ArgumentException("The ID list is invalid at line " + lineNumber + ":\r\nIDs without a 0x prefix must be decimal integers.");
                }

                // Store it
                result.Add(id, name);
            }

            return result;
        }

        /// <summary>
        /// Loads an IDLookup from a text file.
        /// </summary>
        /// <param name="directoryPath">The directoryPath to the text file to read.</param>
        /// <returns>The IDLookup that was created.</returns>
        public static IDLookup LoadFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
                return Load(reader);
        }
    }
}
