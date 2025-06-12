using Conta360.Infrastructure.PGC.Gepsio.Xml.Interfaces;

namespace Conta360.Infrastructure.PGC.Gepsio
{
    /// <summary>
    /// An encapsulation of a child of the XML element "reference" as defined in
    /// the http://www.xbrl.org/2003/linkbase namespace.
    /// </summary>
    public class ReferencePart
    {
        /// <summary>
        /// The name of the reference part.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The value of the reference part.
        /// </summary>
        public string Value { get; private set; }

        internal ReferencePart(INode referencePartNode)
        {
            Name = referencePartNode.LocalName;
            Value = referencePartNode.InnerText;
        }
    }
}
