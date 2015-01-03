using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderWin32
{
    public class ParameterReference 
    {

        public TypeReference ParameterType
        {
            get;
            internal set;
        }

        public override string ToString()
        {
            return ParameterType.Name;
        }

    }
}
