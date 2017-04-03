using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClientApp.Controls
{
    public class MyTreeView : TreeView
    {
        public MyTreeView() : base()
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
            DependencyProperty.Register("SelectedObject", typeof(Object), typeof(MyTreeView), new PropertyMetadata(null, SelectedObjectProperty_Changed));

        private static void SelectedObjectProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null && d is MyTreeView)
            {
                MyTreeView mtv = d as MyTreeView;
                if (mtv.SelectedItem != null)
                {
                    UnselectIfSelected(mtv);
                }
            }
        }

        private static void UnselectIfSelected(DependencyObject dObj)
        {
            if (dObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dObj); i++)
                {
                    DependencyObject newObj = VisualTreeHelper.GetChild(dObj, i);
                    if (newObj is TreeViewItem) {
                        TreeViewItem item = (TreeViewItem)newObj;
                        item.IsSelected = false;
                    }
                    UnselectIfSelected(newObj);
                }
            }
        }

        private void MyTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetValue(SelectedObjectProperty, e.NewValue);
        }
    }
}
