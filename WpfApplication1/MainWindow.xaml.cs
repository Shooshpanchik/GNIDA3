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
using GNIDA;
using plugins;
using System.Data;
using System.Collections.ObjectModel;
using Capstone;


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
            BaseDataSet.proceduresRow rw = baseDataSet.procedures.FindByAddr((long)func.Addr);
            if (rw == null) rw = baseDataSet.procedures.NewproceduresRow();
            rw.Addr = (long)func.Addr;
            rw.Name = func.FName;
            rw.type = func.type;
            rw.end = (long)(func.Addr + func.Length);
            baseDataSet.procedures.AddproceduresRow(rw);
            baseDataSetproceduresTableAdapter.Insert(rw.Addr, rw.Name, rw.type, (long)(func.Addr + func.Length));

            BaseDataSet.paramsRow[] pr = rw.GetparamsRows();
            if (pr.Count() == 0)
            {
                BaseDataSet.paramsRow rv = baseDataSet._params.NewparamsRow();
                rv.type = "void";
                rw.type = rv.Id;
                rv.func = rw.Addr;
                baseDataSet._params.AddparamsRow(rv);
                pr = rw.GetparamsRows();
            }
            Code.Inlines.Add(pr[0].type+" " + func.FName + "(void){");
            Code.Inlines.Add(new LineBreak());
            TextBlock blc = this.FindName("c" + func.FName) as TextBlock;
            if (blc == null)
            {
                blc = new TextBlock();
                blc.FontFamily = new System.Windows.Media.FontFamily("Lucida Console");
                blc.Name = "c" + func.FName;
                NameScope.GetNameScope(this).RegisterName(blc.Name, blc);
            }
            Code.Inlines.Add(blc);
            Code.Inlines.Add(new LineBreak());
            /*
            DataGrid grd = new DataGrid();
            DataGridColumn cl = new DataGridTextColumn();
            Binding Bnd = new Binding();
            cl.SetBinding

            grd.Columns.Add(cl);
            Code.Inlines.Add(grd);
            */
            Code.Inlines.Add("}");
            Code.Inlines.Add(new LineBreak());

        }
        private void AddFuncEvent1(object sender, TFunc func)
        {
            if (func.type == 3) return;
            BaseDataSet.proceduresRow rw = baseDataSet.procedures.FindByAddr((long)func.Addr);
            if (rw == null) rw = baseDataSet.procedures.NewproceduresRow();
            rw.Addr = (long)func.Addr;
            rw.Name = func.FName;
            rw.type = func.type;
            rw.end = (long)(func.Addr + func.Length);
            baseDataSet.procedures.AddproceduresRow(rw);
            baseDataSetproceduresTableAdapter.Insert(rw.Addr, rw.Name, rw.type, (long)(func.Addr + func.Length));
        }

        private void AddText1(uint addr, Stroka str, List<string> text)
        {
            BaseDataSet.strRow s = baseDataSet.str.NewstrRow();
            long? adr = baseDataSetproceduresTableAdapter.GetFuncAddr(addr);
            if (adr == null)
            {
                BaseDataSet.proceduresRow rw = baseDataSet.procedures.NewproceduresRow();
                rw.Addr = addr;
                s.fid = addr;
                baseDataSet.procedures.AddproceduresRow(rw);
            }else s.fid = adr.Value;

            s.addr = addr;
            s.command = str.Inst.insn.Mnemonic;
            s.bytes = str.Inst.bytes;
            s.comment = str.Comment;
            s.Op1 = str.Inst.OpToString(0);
            s.Op2 = str.Inst.OpToString(1);
            s.Op3 = str.Inst.OpToString(2);
            s.Op4 = "";
            baseDataSet.str.AddstrRow(s);
            baseDataSetstrTableAdapter.Insert(addr, s.bytes, text[0], s.comment,
                                              s.Op1,
                                              s.Op2,
                                              s.Op3,
                                              s.Op4,
                                              s.command,s.fid);
           

            string c = "c"+baseDataSetproceduresTableAdapter.Find(addr);
            TextBlock blc = this.FindName(c) as TextBlock;
            if (blc == null)
            {
                blc = new TextBlock();
                blc.Name = c;
                NameScope.GetNameScope(this).RegisterName(c, blc);
            }
            if (str.Label != "")
            {
                Run r2 = new Run("/*" + addr.ToString("X8") + "*/");
                r2.Foreground = Brushes.Green;
                blc.Inlines.Add(r2);
                blc.Inlines.Add(new Run("                          "));
                blc.Inlines.Add(rn(str.Label + ":", Brushes.Blue));
                blc.Inlines.Add(new LineBreak());
            }
            Run r3 = new Run("/*" + addr.ToString("X8") + "*/");
            r3.Foreground = Brushes.Green;
            blc.Inlines.Add(r3);
            string bt = " /* ";
            foreach (byte b in str.Inst.bytes)
                bt += b.ToString("X2") + ' ';
            while (bt.Length < 24) bt += ' ';
            bt += " */ ";
            blc.Inlines.Add(new Run(bt));
            if ((str.Inst.ins == Capstone.X86.INSN.CALL))
                    {
                        long a = 0;
                        switch (str.Inst.dt.Operands[0].Type)
                        {
                            case Capstone.X86.OP.IMM: a = str.Inst.dt.Operands[0].Value.Imm; break;
                            case Capstone.X86.OP.MEM: a = str.Inst.dt.Operands[0].Value.Mem.Disp; break;
                        }
                        string c1 = baseDataSetproceduresTableAdapter.Find(a) as string;
                        if (c1 == null) c1 = str.Inst.insn.Operands;
                        Run fnc = new Run(c1);
                        fnc.Foreground = Brushes.DarkCyan;
                        fnc.ContextMenu = mnu;
                        blc.Inlines.Add(fnc);
                        blc.Inlines.Add("();");
                    }
                else
                blc.Inlines.Add(str.Inst.ToString());
                blc.Inlines.Add(new LineBreak());
        }


        private void AddVarEvent1(object sender, TVar Var)
        {
            BaseDataSet.VariablesRow v = baseDataSet.Variables.NewVariablesRow();
            v.addr = (long)Var.Addr;
            v.name = Var.FName;
            v.type = (int)Var.type;
            baseDataSet.Variables.AddVariablesRow(v);
            baseDataSetVariablesTableAdapter.Insert(v.addr, v.type, v.name);
            blk_variables.Inlines.Add("dword ");
            Run fnc = new Run(Var.FName);
            fnc.Foreground = Brushes.DarkCyan;
            fnc.ContextMenu = mnu;
            blk_variables.Inlines.Add(fnc);
            blk_variables.Inlines.Add(";");
            blk_variables.Inlines.Add(new LineBreak());
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

                Code.Inlines.Clear();
                Window1 wd = new Window1(dlg.FileName);
                if (wd.ShowDialog() == true)
                {
                    MyGNIDA.assembly = ((ListViewItem)(wd.lv.SelectedItem)).Tag as ILoader;
                    MyGNIDA.MeDisasm = ((ListViewItem)(wd.da.SelectedItem)).Tag as IDasmer;
                    MyGNIDA.MeDisasm.Init(MyGNIDA.assembly);
                    MyGNIDA.LoadFile(dlg.FileName);
                    if ((MyGNIDA.assembly.ExecutableFlags() & (ulong)ExecutableFlags.DynamicLoadedLibraryFile) != 0)
                    { pragma.Text = "#pragma option DLL;"; }
                    else
                    {
                        switch (MyGNIDA.assembly.SubSystem())
                        {
                            case (ulong)SubSystem.WindowsGraphicalUI: pragma.Text = "#pragma option W32"; break;
                            case (ulong)SubSystem.WindowsConsoleUI: pragma.Text = "#pragma option W32C"; break;
                            default: pragma.Text = "#pragma option W32;//TO-DO!!!"; break;
                        }
                    }
                }
            }
        }

        private WpfApplication1.BaseDataSet baseDataSet;
        private WpfApplication1.BaseDataSetTableAdapters.proceduresTableAdapter baseDataSetproceduresTableAdapter;
        private WpfApplication1.BaseDataSetTableAdapters.VariablesTableAdapter baseDataSetVariablesTableAdapter;
        WpfApplication1.BaseDataSetTableAdapters.strTableAdapter baseDataSetstrTableAdapter;
        System.Windows.Data.CollectionViewSource strViewSource;
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
            // Загрузить данные в таблицу str. Можно изменить этот код как требуется.
            baseDataSetstrTableAdapter = new WpfApplication1.BaseDataSetTableAdapters.strTableAdapter();
            baseDataSetstrTableAdapter.Fill(baseDataSet.str);
            strViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("strViewSource")));
            strViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Variables. Можно изменить этот код как требуется.
            baseDataSetVariablesTableAdapter = new WpfApplication1.BaseDataSetTableAdapters.VariablesTableAdapter();
            baseDataSetVariablesTableAdapter.Fill(baseDataSet.Variables);
            System.Windows.Data.CollectionViewSource variablesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("variablesViewSource")));
            variablesViewSource.View.MoveCurrentToFirst();

            // Загрузить данные в таблицу _params. Можно изменить этот код как требуется.
            WpfApplication1.BaseDataSetTableAdapters.paramsTableAdapter baseDataSetparamsTableAdapter = new WpfApplication1.BaseDataSetTableAdapters.paramsTableAdapter();
            baseDataSetparamsTableAdapter.Fill(baseDataSet._params);
            System.Windows.Data.CollectionViewSource paramsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("paramsViewSource")));
            paramsViewSource.View.MoveCurrentToFirst();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox1 wd = new AboutBox1();
            wd.ShowDialog();
        }

    }
}
