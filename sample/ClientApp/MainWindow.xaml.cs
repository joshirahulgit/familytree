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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemberTreeWindow cw = new MemberTreeWindow(txtSearchMember.Text);
            cw.Height = this.ActualHeight - 40;
            cw.Width = this.ActualWidth - 40;
            cw.Top = 20;
            cw.Left = 20;
            cw.ShowInTaskbar = false;
            cw.Owner = Application.Current.MainWindow;
            cw.ShowDialog();
        }

        private void AddNewMember_Click(object sender, RoutedEventArgs e)
        {
            SaveMemberWindow cw = new SaveMemberWindow();
            cw.Height = 480;
            cw.Width = 750;
            double left = (Application.Current.MainWindow.RenderSize.Width - cw.Width) / 2;
            double top = (Application.Current.MainWindow.RenderSize.Height - cw.Height) / 2;
            cw.Top = top;
            cw.Left = left;
            cw.ShowInTaskbar = false;
            cw.Owner = Application.Current.MainWindow;
            cw.ShowDialog();
        }
    }
}
