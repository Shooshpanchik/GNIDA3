using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderWin32
{
    public interface ISpecification : IMetaDataMember
    {
        MemberReference TransformWith(IGenericContext context);
    }
}
