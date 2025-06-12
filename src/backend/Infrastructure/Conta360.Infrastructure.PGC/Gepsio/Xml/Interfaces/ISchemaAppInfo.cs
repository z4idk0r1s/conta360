using System.Collections.Generic;
using System.Xml;

namespace Conta360.Infrastructure.PGC.Gepsio.Xml.Interfaces;

public interface ISchemaAppInfo 
{
    IEnumerable< INode > Markup { get; }
}
