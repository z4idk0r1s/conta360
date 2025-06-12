using System.Collections.Generic;

namespace Conta360.Infrastructure.PGC.Gepsio
{
    internal class AttributeGroup
    {
        private List<Attribute> thisAttributeList;

        internal AttributeGroup()
        {
            thisAttributeList = new List<Attribute>();
        }

        internal void AddAttribute(Attribute AttributeToAdd)
        {
            thisAttributeList.Add(AttributeToAdd);
        }
    }
}
