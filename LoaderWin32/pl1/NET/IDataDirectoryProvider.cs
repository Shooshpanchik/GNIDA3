using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderWin32
{
    public interface IDataDirectoryProvider
    {
        DataDirectory[] DataDirectories { get; }
    }
}
