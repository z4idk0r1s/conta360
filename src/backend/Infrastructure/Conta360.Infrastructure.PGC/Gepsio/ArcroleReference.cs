using Conta360.Infrastructure.PGC.Gepsio.Xlink;
using Conta360.Infrastructure.PGC.Gepsio.Xml.Interfaces;
using System;

namespace Conta360.Infrastructure.PGC.Gepsio
{
    /// <summary>
    /// Represents an arcrole reference.
    /// </summary>
    /// <remarks>
    /// Role references are denoted in XBRL instances with the arcroleRef element.
    /// </remarks>
    public class ArcroleReference : XlinkNode
    {
        /// <summary>
        /// The URI for this arcrole reference.
        /// </summary>
        public Uri Uri { get; private set; }

        internal ArcroleReference(INode arcroleRefNode)
            : base(arcroleRefNode)
        {
            this.Uri = new Uri(arcroleRefNode.GetAttributeValue("arcroleURI"));
        }
    }
}
