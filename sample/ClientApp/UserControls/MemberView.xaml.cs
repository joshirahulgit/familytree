using DataModel;
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

namespace ClientApp.UserControls
{
    /// <summary>
    /// Interaction logic for MemberView.xaml
    /// </summary>
    public partial class MemberView : UserControl
    {


        public Visibility CommandVisibility
        {
            get { return (Visibility)GetValue(CommandVisibilityProperty); }
            set { SetValue(CommandVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandVisibilityProperty =
            DependencyProperty.Register("CommandVisibility", typeof(Visibility), typeof(MemberView), new PropertyMetadata(Visibility.Visible, CommandVisibilityProperty_Changed));

        private static void CommandVisibilityProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null && !(d is MemberView))
                return;

            if (!(e.NewValue is Visibility))
                return;

            MemberView mv = d as MemberView;
            mv.spCommand.Visibility = (Visibility)Enum.Parse(typeof(Visibility), e.NewValue.ToString(), true);
        }

        public MemberView()
        {
            InitializeComponent();
            this.SetValue(MemberView.CommandVisibilityProperty, Visibility.Collapsed);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null || !(DataContext is Member))
                return;

            Member member = DataContext as Member;
            SaveMemberWindow cw = new SaveMemberWindow(member);
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null || !(DataContext is Member))
                return;

            MessageBoxResult res = MessageBox.Show("Deleting member will delete all child members.\nStill you want to delete member?", "Delete Member", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            switch (res)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:
                    Member member = DataContext as Member;
                    BL.BLFactory.GetNewMemberBL().DeleteMember(member.Id, (status) =>
                    {
                        if (status)
                        {
                            MessageBox.Show("Member deleted successfully");
                            MemberTreeWindow.CloseWindow();
                        }
                        else
                            MessageBox.Show("Error! Member is not deleted. Please try again.");
                    });
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
    }
}
