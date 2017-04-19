using ClientApp.UserControls;
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
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for SaveMemberWindow.xaml
    /// </summary>
    public partial class SaveMemberWindow : Window
    {


        public SaveMemberWindow()
        {
            InitializeComponent();
            (SaveMemberView.DataContext as SaveMemberViewModel).SelectedMember = new Member(0);
            (SaveMemberView.DataContext as SaveMemberViewModel).SetCloseWindowAction(CloseWindowAction_Handler);
        }

        private void CloseWindowAction_Handler()
        {
            this.Close();
        }

        public SaveMemberWindow(Member selectedMember):this()
        {
            if (selectedMember!=null)
                (SaveMemberView.DataContext as SaveMemberViewModel).SelectedMember = selectedMember;
        }
    }
}
