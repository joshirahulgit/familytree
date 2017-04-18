using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for MemberTreeWindow.xaml
    /// </summary>
    public partial class MemberMapWindow : Window
    {
        private static MemberMapWindow thisWindow;

        //private string text;

        public MemberMapWindow()
        {
            InitializeComponent();
            if (thisWindow != null)
            {
                thisWindow.Close();
            }
            thisWindow = this;
        }

        //public MemberTreeWindow(string text) : this()
        //{
        //    this.TMMember.MemberId = Convert.ToInt32(text);
        //}

        public static void CloseWindow()
        {
            if (thisWindow == null)
                return;

            thisWindow.Close();
        }

        public static void SetMemberId(long memberId)
        {
            if (thisWindow == null)
                openWindow();

            thisWindow.TMMember.loadMember(memberId, thisWindow);
        }

        private static void openWindow()
        {
            MemberMapWindow cw = new MemberMapWindow();
            Size mainWindowSize = Application.Current.MainWindow.RenderSize;
            cw.Height = mainWindowSize.Height - 20;
            cw.Width = mainWindowSize.Width - 20;
            cw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cw.WindowState = WindowState.Maximized;
            cw.ShowInTaskbar = false;
            cw.Owner = Application.Current.MainWindow;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            thisWindow = null;
        }
    }
}
