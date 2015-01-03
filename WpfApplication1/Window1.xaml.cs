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
using System.Windows.Shapes;
using System.Reflection;
using plugins;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string FileName;
        public Window1(string fname)
        {
            FileName = fname;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = (lv.SelectedItems.Count==1)&(da.SelectedItems.Count==1);
            this.Close();           
        }

        private void Loaders(string Path, ListView lv)
        {
            string iMyInterfaceName = typeof(ILoader).ToString();
            Type[] defaultConstructorParametersTypes = new Type[0];
            object[] defaultConstructorParameters = new object[0];
            Assembly assembly1;
            try
            {
                assembly1 = Assembly.LoadFrom(Path);
            }
            catch (System.BadImageFormatException) { return; }
            foreach (Type type in assembly1.GetTypes())
            {
                if (type.GetInterface(iMyInterfaceName) != null)
                {
                    ConstructorInfo defaultConstructor = type.GetConstructor(defaultConstructorParametersTypes);
                    object instance = defaultConstructor.Invoke(defaultConstructorParameters);
                    string descr;
                    if ((instance as ILoader).CanLoad(FileName, out descr))
                    {
                        ListViewItem itm = new ListViewItem();// (descr);
                        itm.Content = descr;
                        itm.Tag = instance as ILoader;
                        lv.Items.Add(itm);
                    }
                }
            }
        }

        private void Dasmers(string Path, ListView lv)
        {
            string iMyInterfaceName = typeof(IDasmer).ToString();
            Type[] defaultConstructorParametersTypes = new Type[0];
            object[] defaultConstructorParameters = new object[0];
            Assembly assembly1;
            try
            {
                assembly1 = Assembly.LoadFrom(Path);
            }
            catch (System.BadImageFormatException) { return; }
            foreach (Type type in assembly1.GetTypes())
            {
                if (type.GetInterface(iMyInterfaceName) != null)
                {
                    ConstructorInfo defaultConstructor = type.GetConstructor(defaultConstructorParametersTypes);
                    object instance = defaultConstructor.Invoke(defaultConstructorParameters);
                    ListViewItem itm = new ListViewItem();
                    itm.Content = (instance as IDasmer).Name();
                    itm.Tag = instance as IDasmer;
                    lv.Items.Add(itm);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lv.Items.Clear();
            da.Items.Clear();
            foreach (string findPlg in System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Loaders\\", "*.dll", System.IO.SearchOption.TopDirectoryOnly))
                try
            {
                    Loaders(findPlg, lv);
            }catch (Exception) {}

            foreach (string findPlg in System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Dasmers\\", "*.dll", System.IO.SearchOption.TopDirectoryOnly))
                try
            {
                    Dasmers(findPlg, da);
            }
            catch (Exception) { }
        }
    }
}
