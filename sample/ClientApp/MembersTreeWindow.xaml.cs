using BL;
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
    /// Interaction logic for MembersTreeWindow.xaml
    /// </summary>
    public partial class MembersTreeWindow : Window
    {
        private IMemberBL bl;

        public MembersTreeWindow()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            bl = BLFactory.GetNewMemberBL();

            IList<Member> members = bl.GetChildMostMembers();
            TreeMainNode maleNode = new TreeMainNode();
            maleNode.txlCaption.Text = "Male";
            mainCanvas.Children.Add(maleNode);

        }
    }
}
