using Conta360.Infrastructure.PGC.Gepsio.Xsd;

namespace Conta360.Infrastructure.PGC.Gepsio
{
    internal class ItemAttributes : FactAttributes
    {
        internal ItemAttributes() : base()
        {
            AddAttribute(new Attribute("contextRef", typeof(IDREF), true));
        }
    }
}
