﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderWin32
{
    public class PropertySignature : IMemberSignature
    {
        public bool HasThis { get; internal set; }
        public TypeReference ReturnType { get; internal set; }
    }
}
