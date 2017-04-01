using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientApp.Controls
{
    public class MyTreeView:TreeView
    {
        public MyTreeView():base()
        {
            this.SelectedItemChanged += MyTreeView_SelectedItemChanged;
        }

        public Object SelectedObject
        {
            get { return (Object)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(Object), typeof(MyTreeView), new PropertyMetadata(null));

        private void MyTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetValue(SelectedObjectProperty, e.NewValue);
        }
    }
}
