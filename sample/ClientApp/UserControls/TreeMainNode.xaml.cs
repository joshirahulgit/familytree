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

namespace ClientApp.UserControls
{
    /// <summary>
    /// Interaction logic for TreeMainNode.xaml
    /// </summary>
    public partial class TreeMainNode : UserControl
    {
        public TreeMainNode()
        {
            InitializeComponent();
        }
        public TreeMainNode(String caption):this()
        {
            this.txlCaption.Text = caption;
        }
    }
}
