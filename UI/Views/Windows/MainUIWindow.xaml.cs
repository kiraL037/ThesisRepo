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
using ThesisProjectARM.UI.ViewModels;
using UI;

namespace ThesisProjectARM.UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainUIWindow.xaml
    /// </summary>
    public partial class MainUIWindow : Window
    {
        public MainUIWindow()
        {
            InitializeComponent();
            DataContext = new MainUIVM();
        }
    }
}
