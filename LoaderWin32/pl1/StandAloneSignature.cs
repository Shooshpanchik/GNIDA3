﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderWin32
{
    public class StandAloneSignature : MetaDataMember 
    {
        public StandAloneSignature(uint signature) : this(new MetaDataRow(signature))
        {
        }

        public StandAloneSignature(MetaDataRow row)
            : base(row)
        {
        }

        public uint Signature
        {
            get { return Convert.ToUInt32(_metadatarow._parts[0]); }
        }

        public override void ClearCache()
        {

        }

        public override void LoadCache()
        {
            
        }
    }
}
