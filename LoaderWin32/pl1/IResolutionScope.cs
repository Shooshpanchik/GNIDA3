using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderWin32
{
    public interface IResolutionScope : IMetaDataMember
    {
        string Name { get; }
        uint MetaDataToken { get; }
        MetaDataRow MetaDataRow { get; }
    }
}
