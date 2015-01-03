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
using System.Windows.Markup;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace WpfApplication1
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplication1"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplication1;assembly=WpfApplication1"
    ///
    /// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    /// на данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///
    ///
    /// Шаг 2)
    /// Теперь можно использовать элемент управления в файле XAML.
    ///
    ///     <MyNamespace:BindableTextBlock/>
    ///
    /// </summary>
    class BindableTextBlock : TextBlock
    {
        public ObservableCollection<Inline> BoundInline
        {
            
            get { return (ObservableCollection<Inline>)GetValue(BoundInlineProperty); }
            set { SetValue(BoundInlineProperty, value); }
        }

        public static readonly DependencyProperty BoundInlineProperty =
            DependencyProperty.Register("BoundInline", typeof(ObservableCollection<Inline>), typeof(BindableTextBlock),
                new UIPropertyMetadata((PropertyChangedCallback)(
                    (d, e) => 
                    { 
                        ((BindableTextBlock)d).Inlines.Clear();
                        ((BindableTextBlock)d).Inlines.Add(e.NewValue as Inline);
                    })));
    }
}