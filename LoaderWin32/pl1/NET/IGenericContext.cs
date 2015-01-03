using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderWin32
{
    public interface IGenericContext : IMetaDataMember
    {
        IGenericParamProvider Method { get; }
        IGenericParamProvider Type { get; }
        bool IsDefinition { get; }
    }
}
