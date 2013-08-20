using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Composer.Wwise
{
    /// <summary>
    /// A collection of Wwise objects that can be referenced through an ID number.
    /// </summary>
    public class WwiseObjectCollection
    {
        private Dictionary<uint, IWwiseObject> _objects = new Dictionary<uint, IWwiseObject>();

        /// <summary>
        /// Adds an object to the collection so that it can be referenced by its ID number.
        /// </summary>
        /// <param name="obj">The IWwiseObject to store.</param>
        public void Add(IWwiseObject obj)
        {
            _objects[obj.ID] = obj;
        }

        /// <summary>
        /// Finds a Wwise object by ID number.
        /// </summary>
        /// <param name="id">The ID number of the object to find.</param>
        /// <returns>The IWwiseObject if found, or null otherwise.</returns>
        public IWwiseObject Find(uint id)
        {
            IWwiseObject result;
            if (_objects.TryGetValue(id, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Removes all objects from the collection.
        /// </summary>
        public void Clear()
        {
            _objects.Clear();
        }

        /// <summary>
        /// Finds a Wwise object by ID number and then passes it to an IWwiseObjectVisitor.
        /// </summary>
        /// <param name="id">The ID number of the object to find.</param>
        /// <param name="visitor">The IWwiseObjectVisitor to pass the object to if it is found.</param>
        /// <returns>true if the object was found and a method was called on the visitor.</returns>
        public bool Dispatch(uint id, IWwiseObjectVisitor visitor)
        {
            IWwiseObject obj = Find(id);
            if (obj != null)
            {
                obj.Accept(visitor);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Imports objects from another collection into this collection.
        /// </summary>
        /// <param name="other">The WwiseObjectCollection to import objects from.</param>
        public void Import(WwiseObjectCollection other)
        {
            foreach (KeyValuePair<uint, IWwiseObject> objectPair in other._objects)
                _objects[objectPair.Key] = objectPair.Value;
        }
    }
}
