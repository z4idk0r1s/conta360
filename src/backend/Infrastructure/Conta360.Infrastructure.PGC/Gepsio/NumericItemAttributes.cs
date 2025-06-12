using Conta360.Infrastructure.PGC.Gepsio.Xsd;

namespace Conta360.Infrastructure.PGC.Gepsio
{
    internal class NumericItemAttributes : EssentialNumericItemAttributes
    {
        internal NumericItemAttributes()
            : base()
        {
            AddAttribute(new Attribute("precision", typeof(PrecisionType), false));
            AddAttribute(new Attribute("decimals", typeof(DecimalsType), false));
        }
    }
}
