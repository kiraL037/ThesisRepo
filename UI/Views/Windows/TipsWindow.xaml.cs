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

namespace ThesisProjectARM.UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для TipsWindow.xaml
    /// </summary>
    public partial class TipsWindow : Window
    {
        public TipsWindow()
        {
            InitializeComponent();
            DataContext = new TipsWindowVM();
        }
    }
}
