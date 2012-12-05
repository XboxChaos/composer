using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Composer.Wwise
{
    /// <summary>
    /// Interface for an object which can be referenced through a Wwise ID.
    /// </summary>
    public interface IWwiseObject
    {
        /// <summary>
        /// The object's ID.
        /// </summary>
        uint ID { get; }

        /// <summary>
        /// Calls a method on an IWwiseObjectVisitor depending upon the type of this object.
        /// </summary>
        /// <param name="visitor">The visitor to call the corresponding method on.</param>
        void Accept(IWwiseObjectVisitor visitor);
    }
}
