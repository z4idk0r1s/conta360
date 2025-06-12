using Conta360.Infrastructure.PGC.Gepsio.Xml.Interfaces;

namespace Conta360.Infrastructure.PGC.Gepsio.Xsd
{
    internal class PrecisionType : NonNegativeInteger
    {
        internal PrecisionType(INode StringRootNode) : base(StringRootNode)
        {
        }
    }
}
