using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderWin32
{
    public interface IMemberSignature
    {
        TypeReference ReturnType { get; }
    }
}
