﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderWin32
{
    public class ExceptionHandler
    {
        internal ExceptionHandler(ExceptionHandlerType type, int trystart, int trylength, int handlerstart, int handlerlength)
        {
            Type = type;
            TryStart = trystart;
            TryEnd = trystart+trylength;
            HandlerStart = handlerstart;
            HandlerEnd = handlerstart + handlerlength;
        }

        public ExceptionHandlerType Type { get; internal set; }
        public int TryStart { get; internal set; }
        public int TryEnd { get; internal set; }
        public int HandlerStart { get; internal set; }
        public int HandlerEnd { get; internal set; }
        public TypeReference CatchType { get; internal set; }
        public int FilterStart { get; internal set; }
    }
}
