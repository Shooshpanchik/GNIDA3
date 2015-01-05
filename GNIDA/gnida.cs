using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using plugins;

namespace GNIDA
{
    public class GNIDA1 : IGNIDA
    {
        public plugins.ILoader assembly;
        public IDasmer MeDisasm;
        public BackgroundWorker bw = new BackgroundWorker();
        public MyDictionary FullProcList() { return _ToFullProcList; }
        public MyDictionary _ToFullProcList = new MyDictionary();
        public MyDictionary _ToDisasmFuncList = new MyDictionary();
        public MyDictionary ToDisasmFuncList() { return _ToDisasmFuncList; }
        public MyDictionary _DisasmedFuncList = new MyDictionary();
        public MyDictionary DisasmedFuncList() { return _DisasmedFuncList; }
        public VarDictionary VarDict() { return _VarDict; }
        public VarDictionary _VarDict = new VarDictionary();
        VarDictionary NewVars = new VarDictionary();
        public Flirt flirt;
        MyDictionary NewSubs = new MyDictionary();
        public GNIDA1(string FlirtCfg)
        {

            flirt = new Flirt(FlirtCfg);
            FullProcList().Parent = this;
            VarDict().Parent = this;
        }
        public int RenameFunction(TFunc f, string NName)
        {
            if (f != null) { f.FName = NName; RaiseFuncChanged(this, f); return 1; }
            return 0;
        }
        private List<ulong> Tasks = new List<ulong>();
        private List<ulong> DTasks = new List<ulong>();
        private List<ulong> LabelList = new List<ulong>();
        private void AddLabel(ulong addr)
        {
            addr = RVA2FO(addr);
            if (!LabelList.Contains(addr))
            {
                if ((!DTasks.Contains(addr) && (!Tasks.Contains(addr))))
                    Tasks.Add(addr);
                LabelList.Add(addr);
            }
        }
        private string AddVar(ulong addr, string nm="", uint tip=0, string vl="")
        {
            if (_VarDict.ContainsKey(addr)) return _VarDict[addr].FName;
            TVar tmp = new TVar(addr, nm, tip, vl);
            if (NewVars != null) if (!NewVars.ContainsKey(addr)) NewVars.Add(addr, tmp);
            return tmp.FName;
        }
        private static void AddProc(ulong x, MyDictionary ProcList, Dictionary<ulong, TFunc> NewSubs)
        {
            if (ProcList.ContainsKey(x)) return;
            TFunc tmpfunc = new TFunc(x, 3);
            if (NewSubs != null)if (!NewSubs.ContainsKey(x)) NewSubs.Add(x, tmpfunc);
        }

        public ulong DisasmFunc(List<Stroka> lst, ulong addr, MyDictionary ProcList)
        {
            Tasks = new List<ulong>();
            DTasks = new List<ulong>();
            LabelList = new List<ulong>();
            ulong StartAdr = addr;
            ulong EndAddr = addr;
            DISASM_INOUT_PARAMS param = new DISASM_INOUT_PARAMS();
            uint Len = 0;
            byte[] sf_prefixes = new byte[Dasmer.MAX_INSTRUCTION_LEN];
            byte[] smp = new byte[4];
            param.arch = Dasmer.ARCH_ALL;
            param.sf_prefixes = sf_prefixes;
            param.mode = DISMODE.DISASSEMBLE_MODE_32;
            param.options = (byte)(Dasmer.DISASM_OPTION_APPLY_REL | Dasmer.DISASM_OPTION_OPTIMIZE_DISP);

            IInstruction instr1;
            Tasks.Add(addr);
            for (uint i = 0; Tasks.Count > 0; i++)
            {
                param.bas = FO2RVA(Tasks[0]);
                Len = MeDisasm.disassemble(Tasks[0], out instr1, ref param);
                instr1.insn.Mnemonic = "$" + instr1.insn.Mnemonic;
                if (EndAddr < (Tasks[0] + Len)) EndAddr = Tasks[0] + Len;
                DTasks.Add(Tasks[0]);
                Tasks.Remove(Tasks[0]);
                lst.Add(new Stroka(this, instr1));
                switch(instr1.ins)
                {
                    case Capstone.X86.INSN.MOVSLDUP:
                        instr1.insn.Operands = "";//movsd
                        break;
                    case Capstone.X86.INSN.POPCNT:
                    case Capstone.X86.INSN.PUSHFD:
                        instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.MUL:
                        instr1.ops[1].size = 2;
                        instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + ", " + instr1.ops[1].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.LODSB:
                    case Capstone.X86.INSN.FUCOMPI:
                        instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + ", " + instr1.ops[1].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.XRSTOR64:
                        instr1.insn.Operands = "DSDWORD[ECX], " + instr1.ops[1].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.CMPXCHG16B:
                        instr1.insn.Mnemonic = "$lock $cmpxchg";
                        instr1.insn.Operands = "DSDWORD[EDX], " + instr1.ops[1].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.LEAVE:
                        instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + ", " + instr1.ops[1].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.CMOVS:
                        instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + ", " + instr1.ops[1].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.AND:
                        instr1.insn.Mnemonic = "";
                        instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + " &= " + instr1.ops[1].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.ADD:
                        instr1.insn.Mnemonic = "";
                        instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + " += " + instr1.ops[1].ToString(AddVar) + ";";
                        break;
                    case Capstone.X86.INSN.INT:
                            instr1.insn.Mnemonic = "";
                            instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + "++;";
                        break;
                    case Capstone.X86.INSN.MOVBE:
                        instr1.insn.Mnemonic = "";
                        if (instr1.OpCount == 2) instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + " = " + instr1.ops[1].ToString(AddVar) + ";";//SIB
                            else instr1.insn.Operands = instr1.ops[0].ToString(AddVar) + " = EAX;";
                        break;
                    case Capstone.X86.INSN.JA:
                    case Capstone.X86.INSN.JAE:
                    case Capstone.X86.INSN.JB:
                    case Capstone.X86.INSN.JBE:
                    case Capstone.X86.INSN.JCXZ:
                    case Capstone.X86.INSN.JE:
                    case Capstone.X86.INSN.JECXZ:
                    case Capstone.X86.INSN.JG:
                    case Capstone.X86.INSN.JGE:
                    case Capstone.X86.INSN.JL:
                    case Capstone.X86.INSN.JLE:
                    case Capstone.X86.INSN.JNE:
                    case Capstone.X86.INSN.JNO:
                    //case Capstone.X86.INSN.JNP:
                    case Capstone.X86.INSN.JNS:
                    case Capstone.X86.INSN.JO:
                    case Capstone.X86.INSN.JP:
                    case Capstone.X86.INSN.JRCXZ:
                    case Capstone.X86.INSN.JS:
                        if (instr1.ops[0].value.imm.imm64 == 0) instr1.ops[0].value.imm.imm64 = instr1.disp.value.d64;
                        instr1.insn.Operands = "Loc_" + instr1.ops[0].value.imm.imm64.ToString("X8");
                        AddLabel(instr1.ops[0].value.imm.imm64); 
                        break;
                    case Capstone.X86.INSN.JNP:
                    //case Capstone.X86.INSN.JMPQ:
                    //case Capstone.X86.INSN.LJMP:
                        instr1.insn.Mnemonic = "goto";
                        if (instr1.ops[0].value.imm.imm64 == 0) instr1.ops[0].value.imm.imm64 = instr1.disp.value.d64;
                        if (ProcList.ContainsKey(instr1.ops[0].value.imm.imm64)) instr1.insn.Operands = ProcList[instr1.ops[0].value.imm.imm64].FName;
                        else
                        {
                            instr1.insn.Operands = "Loc_" + instr1.ops[0].value.imm.imm64.ToString("X8")+";";
                            AddLabel(instr1.ops[0].value.imm.imm64);
                        }
                            instr1.Addr = FO2RVA(instr1.Addr);
                        continue;// Don't disasm after it

                    case Capstone.X86.INSN.CALL: 
                    case Capstone.X86.INSN.CALLW:
                    case Capstone.X86.INSN.LCALL: ulong a = 0;
                        switch(instr1.dt.Operands[0].Type)
                        {
                            case Capstone.X86.OP.IMM: a = (ulong)instr1.dt.Operands[0].Value.Imm; break;
                            case Capstone.X86.OP.MEM: a = (ulong)instr1.dt.Operands[0].Value.Mem.Disp; break;
                        }
                        if (a == 0) break;
                        if (instr1.dt.Operands[0].Type == Capstone.X86.OP.MEM)
                            if (!ProcList.ContainsKey(a))
                            {
                                smp = assembly.ReadBytes(RVA2FO(a), 4);
                                ulong a1 = (ulong)((smp[3] << 24) + (smp[2] << 16) + (smp[1] << 8) + smp[0]);
                                AddVar(a, "dword_" + a.ToString("X8"), 4, "#proc_" + a1.ToString("X8"));
                                instr1.insn.Operands = "dword_" + a.ToString("X8");
                                AddProc(a1, ProcList, NewSubs);
                                break;
                            };
                        AddProc(a, ProcList, NewSubs);
                        if (ProcList.ContainsKey(a))
                        {
                            instr1.insn.Operands = ProcList[a].FName;
                            if (ProcList[a].FName.Contains("ExitProcess")) continue;
                        }
                        else instr1.insn.Operands = "proc_" + a.ToString("X8");
                        break;

                    case Capstone.X86.INSN.RET:
                    case Capstone.X86.INSN.RETF:
                    case Capstone.X86.INSN.RORX:
                        instr1.Addr = FO2RVA(instr1.Addr);
                        continue;// Don't disasm after it
                };
                if ((!DTasks.Contains(instr1.Addr + Len)) && (!Tasks.Contains(instr1.Addr + Len)))
                    Tasks.Add(instr1.Addr + Len);
                instr1.Addr = FO2RVA(instr1.Addr);
            }
            lst.Sort(delegate(Stroka x, Stroka y)
            {
                if (x.addr > y.addr) return 1;
                if (x.addr == y.addr) return 0;
                return -1;
            });
            foreach (ulong Addr in LabelList)
            {
                Stroka result = lst.Find(delegate(Stroka sstr){return sstr.addr == FO2RVA(Addr);});
                if (result != null)
                    result.Label = "Loc_" + result.Inst.Addr.ToString("X8");
            }
            return EndAddr-StartAdr;
        }

        #region Загружаем и работаем
        public void LoadFile(string FName)
        {
            byte[] sf_prefixes = new byte[Dasmer.MAX_INSTRUCTION_LEN];
            //IInstruction instr1;// = new mediana.INSTRUCTION();
            DISASM_INOUT_PARAMS param = new DISASM_INOUT_PARAMS();

            RaiseLogEvent(this, "Loading " + FName);
            IntPtr tmp = assembly.LoadFile(FName);
            //MeDisasm = new medi.mediana(assembly);
            int i = 0;
            foreach (Section1 sect in assembly.Sections())
            {
                RaiseLogEvent(this, i.ToString() + ". Creating a new segment " + sect.RVA.ToString("X8") + " - " + (sect.RVA + sect.VirtualSize).ToString("X8") + "... ... OK");
                i++;
            }

            TFunc fnc = new TFunc(assembly.ImageBase() + assembly.Entrypoint(), 3, 0, "main");

            param.arch = Dasmer.ARCH_ALL;
            param.sf_prefixes = sf_prefixes;
            param.mode = DISMODE.DISASSEMBLE_MODE_32;
            param.options = (byte)(Dasmer.DISASM_OPTION_APPLY_REL | Dasmer.DISASM_OPTION_OPTIMIZE_DISP);
            param.bas = assembly.ImageBase();
            //MeDisasm.disassemble(0x259, out instr1, ref param);
            //Console.WriteLine(instr1.ToString(FullProcList, null, null));
            //MeDisasm.medi_dump(instr, buff, OUT_BUFF_SIZE, DUMP_OPTION_IMM_UHEX | DUMP_OPTION_DISP_HEX);
            FullProcList().AddFunc(fnc);
            foreach (ExportMethod1 func in assembly.LibraryExports())
            {

                TFunc tmpfunc = new TFunc((uint)assembly.ImageBase() + (uint)func.RVA, 1, (uint)func.Ordinal, func.Name);
                FullProcList().AddFunc(tmpfunc);
            }
            foreach (LibraryReference1 lib in assembly.LibraryImports())
            {
                foreach (ImportMethods1 func in lib.ImportMethods)
                {
                    TFunc tmpfunc = new TFunc((uint)assembly.ImageBase() + func.RVA, 2, func.Ordinal, func.Name, lib.LibraryName);
                    FullProcList().AddFunc(tmpfunc);
                }
            }
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = false;
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }
        public void StopWork()
        {
            bw.CancelAsync();
        }



        private List<Stroka> tmp;
        private ulong adr,len;
        private string Name;
        private TFunc ffnc;
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (KeyValuePair<ulong, TFunc> dct1 in NewSubs) FullProcList().AddFunc(dct1.Value);
                //ApplyFlirt(ffnc);
                RaiseFuncDmed(this, ffnc);
                foreach (KeyValuePair<ulong, TVar> tv in NewVars)
                {
                    _VarDict.Add(tv.Key, tv.Value);
                    RaiseVarFuncEvent(this, tv.Value);
                }
                NewVars.Clear();
                foreach (Stroka t in tmp) RaiseAddStrEvent(t.addr, t, t.ToCmmString(NewSubs));
                if (NewSubs.Count > 0)
                {
                    NewSubs.Clear();
                    (sender as BackgroundWorker).RunWorkerAsync();
                }
                else if (_ToDisasmFuncList.Count > 0) (sender as BackgroundWorker).RunWorkerAsync();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            foreach (KeyValuePair<ulong, TFunc> dct in _ToDisasmFuncList)
            { 
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                tmp = new List<Stroka>();
                ulong Len = DisasmFunc(tmp, RVA2FO(dct.Key), FullProcList());
                dct.Value.Length = Len;
                dct.Value.bytes = assembly.ReadBytes(RVA2FO(dct.Key), (int)Len);
                adr = dct.Key;
                Name = dct.Value.FName;
                len = dct.Value.Length;
                ffnc = dct.Value;
                _DisasmedFuncList.Add(dct.Key, dct.Value);
                _ToDisasmFuncList.Remove(dct.Key);
                e.Cancel = true;
                break;
            }
            //_ToDisasmFuncList.Clear();
        }
        #endregion

        #region Some stuff
        public ulong FO2RVA(ulong FO)
        {
            ulong addr = 0;
            foreach (Section1 sct in assembly.Sections())
            {
                if (sct.ContainsRawOffset(FO))
                {
                    addr = sct.RVA + FO - sct.RawOffset + assembly.ImageBase();
                }
            }
            return addr;
        }
        public ulong RVA2FO(ulong RVA)
        {
            ulong addr = 0;
            RVA -= assembly.ImageBase();
            foreach (Section1 sct in assembly.Sections())
            {
                if (sct.ContainsRva(RVA)) addr = sct.RVAToFileOffset(RVA);
            }
            return addr;
        }
        #endregion

        #region Обработчики событий

        public delegate void FuncChanged(object sender, TFunc Func);
        public delegate void LogEvent(object sender, string LogStr);
        public delegate void VarEvent(object sender, TVar Var);
        public delegate void AddFuncEvent(object sender, TFunc Func);
        public delegate void FuncDmed(object sender, TFunc Func);
        public delegate void AddStrEvent(uint addr, Stroka t, List<string> Str);
        public event LogEvent OnLogEvent;
        public event FuncChanged OnFuncChanged;
        public event VarEvent OnVarEvent;
        public event AddFuncEvent OnAddFunc;
        public event FuncDmed OnFuncDmed;
        public event AddStrEvent OnAddStr;
        public void RaiseFuncDmed(object sender, TFunc Func)
        {
            if (OnFuncDmed != null) OnFuncDmed(sender, Func);
        }
        public void RaiseFuncChanged(object sender, TFunc Func)
        {
            if (OnFuncChanged != null) OnFuncChanged(sender, Func);
        }
        public void RaiseVarFuncEvent(object sender, TVar Var)
        {
            if (OnVarEvent != null) OnVarEvent(sender, Var);
        }
        private void RaiseLogEvent(object sender, string LogStr)
        {
            if (OnLogEvent != null) OnLogEvent(sender, LogStr);
        }
        public void RaiseAddFuncEvent(object sender, TFunc Func)
        {
            if (OnAddFunc != null) OnAddFunc(sender, Func);
        }
        private void RaiseAddStrEvent(uint addr, Stroka t, List<string> Str)
        {
            if (OnAddStr != null) OnAddStr(addr,t,  Str);
        }
        #endregion

    }

}
