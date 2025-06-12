using Conta360.Infrastructure.PGC.Gepsio.Xml.Interfaces;

namespace Conta360.Infrastructure.PGC.Gepsio
{
    /// <summary>
    /// An encapsulation of the XML element "referenceArc" as defined in the http://www.xbrl.org/2003/linkbase namespace.
    /// </summary>
    public class ReferenceArc : Arc
    {
        internal ReferenceArc(INode referenceArcNode) : base(referenceArcNode)
        {
            foreach(var CurrentChild in referenceArcNode.ChildNodes)
            {
            }
        }
    }
}
