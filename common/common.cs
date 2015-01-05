using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Capstone;
using Capstone.X86;


namespace plugins
{
    public enum ExecutableFlags
    {
        /// <summary>
        /// Indicates that the file does not contain base relocations and must therefore be loaded at its preferred base address. 
        /// </summary>
        RelocationsStripped = 0x1,
        /// <summary>
        /// Indicates that the image file is valid and can be run. 
        /// </summary>
        ExecutableFile = 0x02,
        /// <summary>
        /// COFF line numbers have been removed.
        /// </summary>
        LineNumbersStripped = 0x4,
        /// <summary>
        /// COFF symbol table entries for local symbols have been removed. 
        /// </summary>
        LocalSymsStripped = 0x8,
        /// <summary>
        /// Obsolete. Aggressively trim working set. 
        /// </summary>
        AggressiveWSTrim = 0x10,
        /// <summary>
        /// Application can handle bigger than 2 GB addresses.
        /// </summary>
        LargeAddressAware = 0x20,
        /// <summary>
        /// This flag is reserved for future use.
        /// </summary>
        Machine16Bit = 0x40,
        /// <summary>
        /// Little endian: the least significant bit (LSB) precedes the most significant bit (MSB) in memory. 
        /// </summary>
        BytesReversedLO = 0x80,
        /// <summary>
        /// Machine is based on a 32-bit-word architecture.
        /// </summary>
        Machine32Bit = 0x100,
        /// <summary>
        /// Debugging information is removed from the image file.
        /// </summary>
        DebuggingInfoStripped = 0x200,
        /// <summary>
        /// If the image is on removable media, fully load it and copy it to the swap file.
        /// </summary>
        RemovableRunFromSwap = 0x400,
        /// <summary>
        /// If the image is on network media, fully load it and copy it to the swap file.
        /// </summary>
        NetRunFromSwap = 0x800,
        /// <summary>
        /// The image file is a system file, not a user program.
        /// </summary>
        System = 0x1000,
        /// <summary>
        /// The File is a DLL Library.
        /// </summary>
        DynamicLoadedLibraryFile = 0x2000,
        /// <summary>
        /// The file should be run only on a uniprocessor machine.
        /// </summary>
        UpSystemOnly = 0x4000,
        /// <summary>
        /// Big endian: the MSB precedes the LSB in memory. This flag is deprecated and should be zero.
        /// </summary>
        BytesReversedHI = 0x8000,
    }

    public enum SubSystem
    {
        /// <summary>
        /// The subsystem is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Doesn't require a subsystem.
        /// </summary>
        Native = 1,
        /// <summary>
        /// Runs in the Windows GUI subsystem.
        /// </summary>
        WindowsGraphicalUI = 2,
        /// <summary>
        /// Runs in the Windows character (console) subsystem.
        /// </summary>
        WindowsConsoleUI = 3,
        /// <summary>
        /// Runs in the OS/2 character (console) subsystem (OS/2 1.x apps only).
        /// </summary>
        OS2ConsoleUI = 5,
        /// <summary>
        /// Runs in the Posix character (console) subsystem.
        /// </summary>
        POSIXConsoleUI = 7,
        /// <summary>
        /// Runs as a native Win9x driver.
        /// </summary>
        NativeWindows = 8,
        /// <summary>
        /// Runs in the Windows Embedded Compact GUI subsystem.
        /// </summary>
        WindowsCEGUI = 9,
        /// <summary>
        /// Runs as an Extensible Firmware Interface (EFI) application
        /// </summary>
        EFIApplication = 10,
        /// <summary>
        /// Runs as an Extensible Firmware Interface (EFI) driver with boot services
        /// </summary>
        EFIBootServiceDriver = 11,
        /// <summary>
        /// Runs as an Extensible Firmware Interface (EFI) driver with run-time services
        /// </summary>
        EFIRuntimeDriver = 12,
        /// <summary>
        /// Runs as an Extensible Firmware Interface (EFI) ROM image
        /// </summary>
        EFIRom = 13,
        /// <summary>
        /// Runs in the Xbox subsystem
        /// </summary>
        Xbox = 14,
        /// <summary>
        /// Runs in the Windows boot subsystem.
        /// </summary>
        WindowsBootApplication = 16,
    }

    public class Stroka
    {
        IGNIDA Parent;
        public uint addr;
        public string UpComment;
        public string Comment;
        public string SubComment;
        public IInstruction Inst;
        public string Label = "";
        public Stroka(IGNIDA Prnt, IInstruction Ins, string UpC = "", string Com = "", string SubC = "")
        {
            Parent = Prnt;
            Inst = Ins;
            UpComment = UpC;
            Comment = Com;
            SubComment = SubC;
            addr = (uint)Prnt.FO2RVA(Ins.Addr);
        }
        public List<string> ToCmmString(Dictionary<ulong, TFunc> NewSubs)
        {
            string bt = "";
            string bt2 = "                      ";
            if (Inst.bytes == null) return null;
            if (Inst.bytes.Count() == 0) return null;
            foreach (byte b in Inst.bytes)
                bt += b.ToString("X2") + ' ';
            while (bt.Length < 20) bt += ' ';
            List<string> tmp = new List<string>();
            string tmp1 = "";
            if (Label != "") tmp.Add(bt2 + Label + ":");
            tmp1 += "/*" + bt + "*/ " + Inst.ToString();
            if (Comment != "") tmp1 += "// " + Comment;
            tmp.Add(tmp1);
            if (SubComment != "") tmp.Add("// " + SubComment);
            return tmp;
        }
    }

    public class ImportMethods1
    {
        public ulong RVA;
        public ulong Ordinal;
        public string Name;
        public ImportMethods1(ulong _RVA, ulong _Ordinal, string _Name)
        {
            RVA = _RVA;
            Ordinal = _Ordinal;
            Name = _Name;
        }
    }
    public class LibraryReference1
    {
        public string LibraryName;
        public ImportMethods1[] ImportMethods;
        public LibraryReference1(string _LibraryName)
        {
            LibraryName = _LibraryName;
        }
    }
    public class ExportMethod1
    {
        public ulong RVA;
        public ulong Ordinal;
        public string Name;
        public ExportMethod1(ulong _RVA, ulong _Ordinal, string _Name)
        {
            RVA = _RVA;
            Ordinal = _Ordinal;
            Name = _Name;
        }
    }
    public class Section1
    {
        public ulong RVA;
        public ulong VirtualSize;
        public string Name;
        public ulong RawOffset;
        ulong SizeOfRawData;
        public ulong RawSize() { return SizeOfRawData; }
        public bool ContainsRawOffset(ulong rawoffset)
        {
            return ((rawoffset >= this.RawOffset) & (rawoffset < (this.RawOffset + this.RawSize())));
        }
        public bool ContainsRva(ulong Rva)
        {
            ulong endoffset = this.RVA + this.VirtualSize;
            return ((Rva >= this.RVA) & (Rva <= endoffset));
        }
        public ulong RVAToFileOffset(ulong rva)
        {
            return rva - this.RVA + RawOffset;
        }
        public Section1(ulong _RVA, ulong _VirtualSize, string _Name, ulong _RawOffset, ulong _SizeOfRawData)
        {
            RVA = _RVA;
            VirtualSize = _VirtualSize;
            Name = _Name;
            RawOffset = _RawOffset;
            SizeOfRawData = _SizeOfRawData;
        }
    }
    //[StructLayout(LayoutKind.Explicit)]
    public struct DISPLACEMENT
    {
        //[FieldOffset(0)]
        public byte size;
        //[FieldOffset(1)]
        public byte offset;
        //[FieldOffset(2)]
        public value2 value;
        [StructLayout(LayoutKind.Explicit)]
        public struct value2
        {
            [FieldOffset(0)]
            public UInt16 d16;
            [FieldOffset(0)]
            public UInt32 d32;
            [FieldOffset(0)]
            public UInt64 d64;
            //[FieldOffset(0)]
            //public byte[] ab;
        }
    };
    public class TFunc
    {
        public ulong Addr;
        public ulong Length;
        public byte[] bytes;
        public string FName;
        public string LibraryName;
        public int type;
        public ulong Ordinal;
        public TFunc(ulong addr, int Type, ulong Ord = 0, string Name = "", string LibName = "")
        {
            Addr = addr;
            type = Type;
            Ordinal = Ord;
            LibraryName = LibName;
            if (Name != "") FName = Name;
            else FName = "proc_" + Addr.ToString("X8");
        }
    }
    public class MyDictionary : Dictionary<ulong, TFunc>
    {
        public IGNIDA Parent;
        public void AddFunc(TFunc value)
        {
            if (!this.ContainsKey(value.Addr))
            {
                this.Add(value.Addr, value);
                Parent.RaiseAddFuncEvent(this, value);
                if (value.type != 2)
                    if ((!Parent.ToDisasmFuncList().ContainsKey(value.Addr))
                      || (!Parent.DisasmedFuncList().ContainsKey(value.Addr))) Parent.ToDisasmFuncList().Add(value.Addr, value);
            }
        }
    }

    public class TVar
    {
        public ulong Addr;
        public string FName;
        public uint type;
        public string val;
        public string ToStr()
        {
            switch (type)
            {
                case 0: return "void " + FName;
                case 1: return "byte " + FName;
                case 2: return "word " + FName;
                case 4: return "dword " + FName;
                default: return "void " + FName;
            }
        }
        public TVar(ulong addr, string Name = "", uint Type = 0, string vl="")
        {
            Addr = addr;
            type = Type;
            val = vl;
            if (Name != "") FName = Name;
            else
                switch (Type)
                {
                    case 1: FName = "byte_" + Addr.ToString("X8"); break;
                    case 2: FName = "word_" + Addr.ToString("X8"); break;
                    case 4: FName = "dword_" + Addr.ToString("X8"); break;
                    default: FName = "unk_" + Addr.ToString("X8"); break;
                }
        }
    }
    public class VarDictionary : Dictionary<ulong, TVar>
    {
        public IGNIDA Parent;
        public void AddVar(TVar value)
        {
            if (!this.ContainsKey(value.Addr))
            {
                this.Add(value.Addr, value);
                Parent.RaiseVarFuncEvent(this, value);
            }
        }
    }

    public enum REG_TYPE
    {
        REG_TYPE_GEN = 0x0,
        REG_TYPE_SEG = 0x1,
        REG_TYPE_CR = 0x2,
        REG_TYPE_DBG = 0x3,
        REG_TYPE_TR = 0x4,
        REG_TYPE_FPU = 0x5,
        REG_TYPE_MMX = 0x7,
        REG_TYPE_XMM = 0x8
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct reg1
    {
        [FieldOffset(0)]
        public byte code;
        [FieldOffset(1)]
        public REG_TYPE type;
    };
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct far_addr321
    {
        [FieldOffset(0)]
        public UInt16 offset;
        [FieldOffset(2)]
        public UInt16 seg;
        //[FieldOffset(0)]
        //public UInt32 Val;
    }
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct imm1
    {
        [FieldOffset(0)]
        public byte imm8;
        [FieldOffset(0)]
        public UInt16 imm16;
        [FieldOffset(0)]
        public UInt32 imm32;
        [FieldOffset(0)]
        public UInt64 imm64;
        //[FieldOffset(0)]
        //public byte[] immab;
        [FieldOffset(8)]
        public byte size;
        [FieldOffset(9)]
        public byte offset;
    }
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct far_addr481
    {
        [FieldOffset(0)]
        public UInt32 offset;
        [FieldOffset(4)]
        public UInt16 seg;
        [FieldOffset(0)]
        public UInt64 Val;
    }
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct far_addr1
    {
        [FieldOffset(0)]
        public far_addr321 far_addr32;
        [FieldOffset(0)]
        public far_addr481 far_addr48;
        //[FieldOffset(0)]
        //public byte[] far_addr_ab;
        [FieldOffset(6)]
        public byte offset;
    }
    public struct addr1
    {
        public byte seg;
        public byte mod;
        public byte bas;
        public byte index;
        public byte scale;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct value1
    {
        [FieldOffset(0)]
        public REG reg;
        [FieldOffset(0)]
        public imm1 imm;
        [FieldOffset(0)]
        public far_addr1 far_addr;
        [FieldOffset(0)]
        public addr1 addr;
    }

    public delegate string AddVr(ulong addr, string nm="", uint tip=0, string vl="");

    public struct OPERAND
    {
        public value1 value;
        public ushort size; //Fuck... I need 16_t only for 'stx' size qualifier.
        public OP flags;
        public string ToString(AddVr AddVarProc)
        {
            switch(flags)
            {
                case OP.INVALID: return "EAX";// "INVALID";
                case OP.REG:
                            switch(value.reg)
                            { 
                                case REG.FS:return "FSDWORD[0]";
                                default : return value.reg.ToString();
                            }
                             
                case OP.IMM: return "0x" + value.imm.imm64.ToString("X8");
                case OP.MEM:
                    {
                        Int32 vl = (Int32)value.imm.imm32;
                        if (size != 2) size = 4;
                        if (vl > 0) return AddVarProc(value.imm.imm64, "", size);
                        vl = -vl;
                        if (size == 2) return "DSWORD[EBP-0x" + vl.ToString("X8") + "]";
                            return "DSDWORD[EBP-0x" + vl.ToString("X8") + "]";
                    }
            }
            return base.ToString();
        }
    };

    public abstract class IInstruction
    {
        public Instruction insn;
        public CsX86 dt;
        public INSN ins;
        public byte[] bytes { get; set; }
        public ulong Addr { get; set; }
        public abstract string ToString();
        public abstract string OpToString(byte N);
        public string mnemonic { get; set; }
        public OPERAND[] ops;//OPERAND[3];
        public DISPLACEMENT disp;
        public int OpCount;
    }
    public interface IPlugin
    {
        string Description();

    }
    public interface IGNIDA
    {
        MyDictionary ToDisasmFuncList();
        MyDictionary DisasmedFuncList();
        MyDictionary FullProcList();
        VarDictionary VarDict();
        ulong FO2RVA(ulong FO);
        ulong RVA2FO(ulong RVA);
        void RaiseAddFuncEvent(object sender, TFunc Func);
        void RaiseVarFuncEvent(object sender, TVar Var);
    }
    public interface ILoader
    {
        string FName { get; set; }
        ulong SubSystem();
        ulong ExecutableFlags();
        ulong ImageBase();
        ulong Entrypoint();
        List<Section1> Sections();
        List<ExportMethod1> LibraryExports();
        List<LibraryReference1> LibraryImports();
        byte[] ReadBytes(ulong offset, int length);
        bool CanLoad(string FName, out string descr);
        IntPtr LoadFile(string FName);
    }
    public enum ERRS
    {
        ERR_OK,
        ERR_BADCODE,
        ERR_TOO_LONG,
        ERR_NON_LOCKABLE,
        ERR_RM_REG,
        ERR_RM_MEM,
        ERR_16_32_ONLY,
        ERR_64_ONLY,
        ERR_REX_NOOPCD,
        ERR_ANOT_ARCH,
        ERR_INTERNAL
    };
    public enum DISMODE
    {
        DISASSEMBLE_MODE_16 = 0x1,
        DISASSEMBLE_MODE_32 = 0x2,
        DISASSEMBLE_MODE_64 = 0x4,
    }
    public struct DISASM_INOUT_PARAMS
    {
        public int sf_prefixes_len;
        public byte[] sf_prefixes;
        public ERRS errcode;
        public byte arch;
        public DISMODE mode;
        public byte options;
        public UInt64 bas;
    };
    public static class Dasmer
    {
        public static uint MAX_MNEMONIC_LEN = 0x0C;
        public static uint MAX_INSTRUCTION_LEN = 0x0F;
        //DISASM_INOUT_PARAMS.options' bits:
        public static byte DISASM_OPTION_APPLY_REL = 0x1;
        public static byte DISASM_OPTION_OPTIMIZE_DISP = 0x2;
        public static byte ARCH_COMMON = 0x1;
        public static byte ARCH_INTEL = 0x2;
        public static byte ARCH_AMD = 0x4;
        public static byte ARCH_ALL = (byte)((int)ARCH_COMMON | (int)ARCH_INTEL | (int)ARCH_AMD);
    }
    public interface IDasmer
   {
       void Init(ILoader ldr);
       string Name();
        UInt32 disassemble(ulong offset, out IInstruction instr, ref DISASM_INOUT_PARAMS param);
    }
}
