using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using GNIDA;
using plugins;
using System.Data;
using System.Collections.ObjectModel;
using Capstone;
using System.Diagnostics;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GNIDA1 MyGNIDA;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLogEvent1(object sender, string LogStr)
        {
            Log.Items.Add(LogStr);
            Log.ScrollIntoView(Log.Items[Log.Items.Count - 1]);
        }
        internal Run rn(string text, Brush col)
        {
            Run res = new Run(text);
            res.Foreground = col;
            return res;
        }
        private void AddFuncEvent2(object sender, TFunc func)
        {
            BaseDataSet.proceduresRow rw = baseDataSet.procedures.FindByAddr((uint)func.Addr);
            if (rw == null)
            {
                rw = baseDataSet.procedures.NewproceduresRow();
                rw.Addr = (uint)func.Addr;
                rw.Name = func.FName;
                rw.type = func.type;
                rw.end = (uint)(func.Addr + func.Length);
                baseDataSet.procedures.AddproceduresRow(rw);
                baseDataSetproceduresTableAdapter.Insert(rw.Addr, rw.Name, rw.type, (long)(func.Addr + func.Length));
            }
            Paragraph blc = this.FindName("c" + func.FName) as Paragraph;
            if (blc == null)
            {
                blc = new Paragraph();
                blc.FontFamily = new System.Windows.Media.FontFamily("Lucida Console");
                blc.Name = "c" + func.FName;
                NameScope.GetNameScope(this).RegisterName(blc.Name, blc);
            }
            blc.Inlines.Add(new Run("void " + func.FName + "(void){"));
            blc.Inlines.Add(new LineBreak());
//            blc.Inlines.Add(new LineBreak());
            blc.Inlines.Add(new Run("}"));
            Code.Document.Blocks.Add(blc);
        }
        private void AddFuncEvent1(object sender, TFunc func)
        {
            if (func.type == 3) return;
            BaseDataSet.proceduresRow rw = baseDataSet.procedures.FindByAddr((uint)func.Addr);
            if (rw == null) rw = baseDataSet.procedures.NewproceduresRow();
            rw.Addr = (uint)func.Addr;
            rw.Name = func.FName;
            rw.type = func.type;
            rw.end = (uint)(func.Addr + func.Length);
            baseDataSet.procedures.AddproceduresRow(rw);
            baseDataSetproceduresTableAdapter.Insert(rw.Addr, rw.Name, rw.type, (long)(func.Addr + func.Length));
        }

        private void AddText1(ulong addr, Stroka str, List<string> text)
        {
            string c = "c" + baseDataSetproceduresTableAdapter.GetFuncAddr((long)addr) as string;
            if ((c == "c"))
            {                
                BaseDataSet.proceduresRow rw = baseDataSet.procedures.NewproceduresRow();
                rw.Addr = (uint)addr;
                rw.Name = "Proc_" + addr.ToString("X8");
                if (baseDataSet.procedures.FindByAddr((uint)addr) == null) baseDataSet.procedures.AddproceduresRow(rw);
                c = "c" + rw.Name;
            };
            Paragraph blc = this.FindName(c) as Paragraph;
            if (blc == null)
            {
                blc = new Paragraph();
                blc.Name = c;
                NameScope.GetNameScope(this).RegisterName(c, blc);

                blc.Inlines.Add(new Run("void " + c + "(void){"));
                blc.Inlines.Add(new LineBreak());
                //            blc.Inlines.Add(new LineBreak());
                blc.Inlines.Add(new Run("}"));
                Code.Document.Blocks.Add(blc);
            }
            if (str.Label != "")
            {
                Run r2 = new Run("/*" + addr.ToString("X8") + "*/");
                r2.Foreground = Brushes.Green;
                blc.Inlines.InsertBefore(blc.Inlines.LastInline, r2);
                blc.Inlines.InsertBefore(blc.Inlines.LastInline, new Run("                                 "));
                blc.Inlines.InsertBefore(blc.Inlines.LastInline, rn(str.Label + ":", Brushes.Blue));
                blc.Inlines.InsertBefore(blc.Inlines.LastInline, new LineBreak());
            }
            Run r3 = new Run("/*" + addr.ToString("X8") + "*/");
            r3.Foreground = Brushes.Green;
            blc.Inlines.InsertBefore(blc.Inlines.LastInline, r3);
            string bt = " /* ";
            foreach (byte b in str.Inst.bytes)
                bt += b.ToString("X2") + ' ';
            while (bt.Length < 24) bt += ' ';
            bt += " */ ";
            while (bt.Length < 36) bt += ' ';
            blc.Inlines.InsertBefore(blc.Inlines.LastInline, new Run(bt));
            if ((str.Inst.ins == Capstone.X86.INSN.CALL))
                    {
                        long a = 0;
                        switch (str.Inst.dt.Operands[0].Type)
                        {
                            case Capstone.X86.OP.IMM: a = str.Inst.dt.Operands[0].Value.Imm; break;
                            case Capstone.X86.OP.MEM: a = str.Inst.dt.Operands[0].Value.Mem.Disp; break;
                        }
                        string c1 = baseDataSetproceduresTableAdapter.ScalarQuery(a) as string;
                        if (c1 == null) c1 = str.Inst.insn.Operands;
                        if (str.Inst.dt.Operands[0].Type == Capstone.X86.OP.REG)
                                    c1 = "$call " + str.Inst.dt.Operands[0].Value.Reg.ToString();
                                else c1 = c1 + "();";
                        Run fnc = new Run(c1);
                        fnc.Foreground = Brushes.DarkCyan;
                        fnc.ContextMenu = mnu;
                        blc.Inlines.InsertBefore(blc.Inlines.LastInline, fnc);
                    }
                else
                blc.Inlines.InsertBefore(blc.Inlines.LastInline, new Run(str.Inst.ToString()));
            blc.Inlines.InsertBefore(blc.Inlines.LastInline, new LineBreak());
        }


        private void AddVarEvent1(object sender, TVar Var)
        {
            ListBoxItem tt = new ListBoxItem();
            tt.Content = Var.FName;
            vbls.Items.Add(tt);
            Run fnc = new Run(Var.ToStr());
            fnc.Foreground = Brushes.Red;
            fnc.ContextMenu = mnu;
            blk_variables1.Inlines.Add(fnc);
            if (Var.val != "") blk_variables1.Inlines.Add("="+Var.val);
            blk_variables1.Inlines.Add(";");
            blk_variables1.Inlines.Add(new LineBreak());
            //blk_variables.AppendText(new LineBreak());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Executables (.exe;*.dll)|*.exe;*.dll|All files|*.*";
            if (dlg.ShowDialog() == true)
            {
                MyGNIDA = new GNIDA1(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\flirt.xml");

                MyGNIDA.OnLogEvent += OnLogEvent1;
                MyGNIDA.OnAddFunc += AddFuncEvent1;
                MyGNIDA.OnAddStr += AddText1;
                MyGNIDA.OnFuncDmed += AddFuncEvent2;
                MyGNIDA.OnVarEvent += AddVarEvent1;
                //MyGNIDA.OnFuncChanged += OnFuncChanged1;

                while (Code.Document.Blocks.Count > 4) Code.Document.Blocks.Remove(Code.Document.Blocks.LastBlock);
                blk_variables1.Inlines.Clear();
                Code1.Inlines.Clear();
                baseDataSet.procedures.Clear();
                baseDataSetproceduresTableAdapter.Fill(baseDataSet.procedures);
                HEX.Document.Blocks.Clear();
                this.Title = "GNIDA - " + dlg.FileName;

                Window1 wd = new Window1(dlg.FileName);
                if (wd.ShowDialog() == true)
                {
                    MyGNIDA.assembly = ((ListViewItem)(wd.lv.SelectedItem)).Tag as ILoader;
                    MyGNIDA.MeDisasm = ((ListViewItem)(wd.da.SelectedItem)).Tag as IDasmer;
                    MyGNIDA.MeDisasm.Init(MyGNIDA.assembly);
                    MyGNIDA.LoadFile(dlg.FileName);
                    if ((MyGNIDA.assembly.ExecutableFlags() & (ulong)ExecutableFlags.DynamicLoadedLibraryFile) != 0)
                    { PragmaOption.Text = "#pragma option DLL;"; }
                    else
                    {
                        switch (MyGNIDA.assembly.SubSystem())
                        {
                            case (ulong)SubSystem.WindowsGraphicalUI: PragmaOption.Text = "#pragma option W32"; break;
                            case (ulong)SubSystem.WindowsConsoleUI: PragmaOption.Text = "#pragma option W32C"; break;
                            default: PragmaOption.Text = "#pragma option W32;//TO-DO!!!"; break;
                        }
                    }
                }
                foreach (Section1 sct in MyGNIDA.assembly.Sections())
                {
                    Paragraph p = new Paragraph();
                    p.Name = sct.Name.Replace(".","_");
                    ulong st = sct.RVA + MyGNIDA.assembly.ImageBase();
                    byte[] hs = new byte[16];
                    while (st < Math.Min(sct.RawSize(), sct.VirtualSize) + sct.RVA + MyGNIDA.assembly.ImageBase())
                      {
                          string s = sct.Name + ":" + (st).ToString("X8") + "  ";
                          hs = MyGNIDA.assembly.ReadBytes(MyGNIDA.RVA2FO(st), 16);
                          foreach (byte bt in hs)s += bt.ToString("X2") + " ";
                          s += "   ";
                          foreach (byte bt in hs)
                            if ((bt > 0x20) & (bt<0x80)) s += (char)bt;
                              else s += ".";
                          p.Inlines.Add(s);
                          p.Inlines.Add(new LineBreak());
                          st += 16;
                      };
                    if (sct.RawSize() < sct.VirtualSize)
                    {
                        for (ulong x = 0; x < sct.VirtualSize; x += 16)
                        {
                            string s = sct.Name + ":" + (x + sct.RawOffset + sct.RVA + MyGNIDA.assembly.ImageBase()).ToString("X8") + "  ";
                            s += "?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ??    ????????????????";
                            Run rn = new Run(s);
                            rn.Name = p.Name + (x + sct.RawOffset + sct.RVA + MyGNIDA.assembly.ImageBase()).ToString("X8");
                            p.Inlines.Add(rn);
                            p.Inlines.Add(new LineBreak());
                        }
                    }
                 HEX.Document.Blocks.Add(p);
                }

            }
        }

        private WpfApplication1.BaseDataSet baseDataSet;
        private WpfApplication1.BaseDataSetTableAdapters.proceduresTableAdapter baseDataSetproceduresTableAdapter;
        private ContextMenu mnu;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mnu = new ContextMenu();
            MenuItem itm = new MenuItem();
            itm.Header = "Rename";
            mnu.Items.Add(itm);
            MenuItem itm2 = new MenuItem();
            itm2.Header = "Send to";
            MenuItem itm3 = new MenuItem();
            itm3.Header = "Flirt";
            itm2.Items.Add(itm3);
            mnu.Items.Add(itm2);

            baseDataSet = ((WpfApplication1.BaseDataSet)(this.FindResource("baseDataSet")));
            // Загрузить данные в таблицу procedures. Можно изменить этот код как требуется.
            baseDataSetproceduresTableAdapter = new WpfApplication1.BaseDataSetTableAdapters.proceduresTableAdapter();
            baseDataSetproceduresTableAdapter.Fill(baseDataSet.procedures);
            System.Windows.Data.CollectionViewSource proceduresViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("proceduresViewSource")));
            proceduresViewSource.View.MoveCurrentToFirst();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox1 wd = new AboutBox1();
            wd.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            FileStream docStream = new FileStream("cmm\\tmp.cmm", FileMode.Create);
            TextRange tr = new TextRange(Code.Document.ContentStart, Code.Document.ContentEnd);
            tr.Save(docStream, DataFormats.Text);
            docStream.Close();
            System.Diagnostics.Process MyProc = new System.Diagnostics.Process();
            MyProc.StartInfo.FileName = "cmm\\c--.exe";
            MyProc.StartInfo.Arguments = "cmm\\tmp.cmm";
            MyProc.StartInfo.UseShellExecute = false;
            MyProc.StartInfo.RedirectStandardOutput = true;
            MyProc.Start();
            do
            {
                Log.Items.Add(MyProc.StandardOutput.ReadLine());
            } while (!MyProc.StandardOutput.EndOfStream);
            MyProc.WaitForExit();
            Log.ScrollIntoView(Log.Items[Log.Items.Count - 1]);
        }

        private void Code_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            
        }

        private void Code_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextPointer caretLineStart = Code.CaretPosition.GetLineStartPosition(0);
            TextPointer p = Code.Document.ContentStart.GetLineStartPosition(0);
            int currentLineNumber = 1;

            while (true)
            {
                if (caretLineStart.CompareTo(p) < 0)break;
                int result;
                p = p.GetLineStartPosition(1, out result);
                if (result == 0)break;
                currentLineNumber++;
            }
            ;
            CurPos.Content = "X:" + Code.CaretPosition.GetLineStartPosition(0).GetOffsetToPosition(Code.Selection.Start).ToString() +
                             " Y:"+ currentLineNumber.ToString();
                            
        }

        private void lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string sd = "c"+((lb.SelectedItem as DataRowView).Row.ItemArray[1].ToString());
            object ttt = this.FindName(sd);
            Paragraph blc = this.FindName(sd) as Paragraph;
            if (blc != null) blc.BringIntoView();           
        }

    }
}
