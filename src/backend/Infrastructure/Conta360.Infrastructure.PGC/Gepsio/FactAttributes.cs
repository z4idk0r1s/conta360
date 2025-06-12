using Conta360.Infrastructure.PGC.Gepsio.Xsd;
namespace Conta360.Infrastructure.PGC.Gepsio
{
    internal class FactAttributes : AttributeGroup
    {
        internal FactAttributes() : base()
        {
            AddAttribute(new Attribute("id", typeof(ID), false));
        }
    }
}
