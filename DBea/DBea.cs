using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone;
using plugins;
using System.Runtime.InteropServices;
using System.IO;
using Capstone.X86;

namespace DBea
{
    public class DBea : IDasmer
    {
        public static ILoader assembly;
        Capstone.Capstone cs;
        public string Name()
        { return "Capstone Engine"; }
        public void Init(ILoader _assembly)
        {
            assembly = _assembly;
            cs = new Capstone.Capstone(Architecture.X86, Mode.Mode32);
            cs.SetSyntax(OptionValue.SyntaxIntel);
            cs.SetDetail(true);
        }
        public class INSTRUCTION : IInstruction
        {
            public override string OpToString(byte N)
            {
                return insn.Operands;
            }
            public override string ToString()
            {
                return insn.Mnemonic + " " + insn.Operands;// disasm.CompleteInstr;
            }
            public INSTRUCTION()
            {
                ops = new OPERAND[3];//OPERAND[3];
                //insn = new Instruction();
            }
        }
        public UInt32 disassemble(ulong offset, out IInstruction instr1, ref DISASM_INOUT_PARAMS param)
        {
            INSTRUCTION instr = new INSTRUCTION();
            instr.bytes = new byte[1];
            instr1 = instr;
            instr.Addr = offset;
            Instruction[] insns = cs.Disassemble(assembly.ReadBytes(offset, 10), (uint)param.bas, (UIntPtr)(1));
            if (insns!=null)
            {
                instr.insn = insns[0];
                
                //instr.bytes = new byte[insns[0].Size];
                instr.bytes = assembly.ReadBytes(offset, insns[0].Size);
                instr.dt = (Capstone.X86.CsX86)insns[0].Arch;
                instr.ins = (Capstone.X86.INSN)instr.insn.Id;

                if (instr.dt.Operands.Count() > 0)
                    instr.ops[0].value.imm.imm64 = (ulong)instr.dt.Operands[0].Value.Imm;
                if (instr.dt.Operands.Count() > 1)
                    instr.ops[1].value.imm.imm64 = (ulong)instr.dt.Operands[1].Value.Imm;
                instr.disp.value.d64 = (ulong)instr.dt.Disp;
                instr1 = instr;
                return insns[0].Size;
            }
            else return 0;
        }        
    }
}
