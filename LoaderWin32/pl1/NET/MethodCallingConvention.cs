﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderWin32
{
    public enum MethodCallingConvention : byte
    {
        C = 1,
        Default = 0,
        FastCall = 4,
        Generic = 0x10,
        StdCall = 2,
        ThisCall = 3,
        VarArg = 5
    }

}
