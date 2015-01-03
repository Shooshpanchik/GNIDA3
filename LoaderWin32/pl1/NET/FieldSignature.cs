using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderWin32
{
    public class FieldSignature : IMemberSignature
    {
        public TypeReference ReturnType
        {
            get;
            internal set;
        }

    }
}
