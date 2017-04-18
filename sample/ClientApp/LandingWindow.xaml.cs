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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for LandingWindow.xaml
    /// </summary>
    public partial class LandingWindow : Window
    {
        public LandingWindow()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            long memId = 0L;
            if (txtSearchMember == null || string.IsNullOrEmpty(txtSearchMember.Text) || !long.TryParse(txtSearchMember.Text, out memId))
                return;

            MemberMapWindow.SetMemberId(memId);
        }

        private void btnAddMember_Click(object sender, RoutedEventArgs e)
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

        private void txtSearchMember_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
