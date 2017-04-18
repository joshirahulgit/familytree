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
    /// Interaction logic for SaveMemberView.xaml
    /// </summary>
    public partial class SaveMemberView : UserControl
    {
        public SaveMemberView()
        {
            InitializeComponent();
        }

        private void textBox_PreviewTextInput_HandleNumberOnly(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
