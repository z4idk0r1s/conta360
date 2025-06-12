using Conta360.Infrastructure.PGC.Gepsio.Xlink;
using Conta360.Infrastructure.PGC.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace Conta360.Infrastructure.PGC.Gepsio
{
    /// <summary>
    /// An encapsulation of the XML element "reference" as defined in the http://www.xbrl.org/2003/linkbase namespace.
    /// </summary>
    public class Reference : XlinkNode
    {
        /// <summary>
        /// A collection of parts for this reference.
        /// </summary>
        public List<ReferencePart> ReferenceParts { get; private set; }

        internal Reference(INode referenceNode) : base(referenceNode)
        {
            ReferenceParts = new List<ReferencePart>();
            foreach(INode CurrentChild in referenceNode.ChildNodes)
            {
                ReferenceParts.Add(new ReferencePart(CurrentChild));
            }
        }
    }
}
