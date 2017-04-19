using BL;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.UserControls
{
    /// <summary>
    /// Interaction logic for MemberTreeMap.xaml
    /// </summary>
    public partial class MemberTreeMap : UserControl
    {
        //private long memId = 8;//13;
        private float height = 70;
        private float width = 400;
        private double margin = 40;
        private IDictionary<int, int> levelCounter;
        private IDictionary<long, MemberView> memberViewTracker;
        private double vCenter;
        private double hCenter;
        private IDictionary<Line, Storyboard> lines;


        private IMemberBL bl;

        public MemberTreeMap()
        {
            InitializeComponent();
        }

        private void initialize()
        {
            bl = BLFactory.GetNewMemberBL();
            canvas.Children.Clear();
            levelCounter = new Dictionary<int, int>();
            memberViewTracker = new Dictionary<long, MemberView>();
            lines = new Dictionary<Line, Storyboard>();
        }

        public void loadMember(long memberId, Window dialog)
        {
            initialize();
            Member member = bl.GetMemberWithParentAndChild(memberId);
            if (member == null)
            {
                MessageBox.Show("No member found with id: " + memberId, "No Member", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            MemberView mv = new MemberView();
            mv.CommandVisibility = Visibility.Visible;
            mv.DataContext = member;
            if (levelCounter.ContainsKey(0))
                levelCounter.Add(0, levelCounter[0] + 1);
            else
                levelCounter.Add(0, 1);
            memberViewTracker.Add(memberId, mv);

            this.canvas.Children.Add(mv);

            loadParents(member);
            loadChildren(member);

            //Set Canvas size.
            int vCount = levelCounter.Count;
            if (vCount % 2 == 0)
                vCount++;
            int hCount = levelCounter.Values.Max();
            this.canvas.Width = hCount * width + 2 * hCount * margin;
            this.canvas.Height = vCount * height + 2 * vCount * margin;

            vCenter = this.canvas.Height / 2;
            hCenter = this.canvas.Width / 2;

            double top = (vCenter - height / 2);
            double left = (hCenter - width / 2);
            mv.SetValue(Canvas.TopProperty, top);
            mv.SetValue(Canvas.LeftProperty, left);

            placeView(member.Parents, -1);
            placeView(member.Children, 1);

            double y = (double)mv.GetValue(Canvas.TopProperty);
            double x = (double)mv.GetValue(Canvas.LeftProperty) + width / 2;
            Point p1 = new Point(x, y);
            joinParents(p1, member.Parents);

            y = (double)mv.GetValue(Canvas.TopProperty) + height;
            x = (double)mv.GetValue(Canvas.LeftProperty) + width / 2;
            p1 = new Point(x, y);
            joinChildren(p1, member.Children);

            //mv.FocusBorder.Visibility = Visibility.Visible;

            //Add selector visual
            SelectorUC selector = new SelectorUC();
            selector.SetValue(Canvas.LeftProperty, left - selector.Width);
            selector.SetValue(Canvas.TopProperty, top + (height - selector.Height) / 2);
            canvas.Children.Add(selector);

            //Animate lines
            if (lines != null)
            {
                foreach (var lineItem in lines)
                {
                    lineItem.Key.BeginStoryboard(lineItem.Value);
                }
            }

            //Add children indicator
            if (member.Children != null && member.Children.Count > 0)
            {
                ChildrenUC cUC = new ChildrenUC();
                cUC.SetValue(Canvas.TopProperty, top + height);
                cUC.SetValue(Canvas.LeftProperty, left + width - cUC.Width + 4);
                canvas.Children.Add(cUC);
            }

            //Add Parent indicator
            if (member.Parents != null && member.Parents.Count > 0)
            {
                ParentUC pUC = new ParentUC();
                pUC.SetValue(Canvas.TopProperty, top - pUC.Height);
                pUC.SetValue(Canvas.LeftProperty, left + width - pUC.Width + 4);
                canvas.Children.Add(pUC);
            }

            if (!dialog.IsActive)
                dialog.ShowDialog();
        }

        private void joinChildren(Point p1, IList<Member> children)
        {
            foreach (Member member in children)
            {
                MemberView mv = memberViewTracker[member.Id];
                if (mv != null)
                {
                    double y = (double)mv.GetValue(Canvas.TopProperty);
                    double x = (double)mv.GetValue(Canvas.LeftProperty) + width / 2;
                    Point p = new Point(x, y);
                    Line line = new Line();
                    line.Stroke = new SolidColorBrush(Colors.White);
                    line.StrokeThickness = 1;
                    line.X1 = p1.X;
                    line.Y1 = p1.Y;
                    line.X2 = p1.X;
                    line.Y2 = p1.Y;
                    this.canvas.Children.Add(line);

                    //Animating line drawing.
                    Storyboard sb = new Storyboard();
                    DoubleAnimation da = new DoubleAnimation(line.Y2, p.Y, new Duration(new TimeSpan(0, 0, 1)));
                    DoubleAnimation da1 = new DoubleAnimation(line.X2, p.X, new Duration(new TimeSpan(0, 0, 1)));
                    Storyboard.SetTargetProperty(da, new PropertyPath("(Line.Y2)"));
                    Storyboard.SetTargetProperty(da1, new PropertyPath("(Line.X2)"));
                    sb.Children.Add(da);
                    sb.Children.Add(da1);
                    lines.Add(line, sb);
                }
            }
        }

        private void joinParents(Point p1, IList<Member> parents)
        {
            foreach (Member member in parents)
            {
                MemberView mv = memberViewTracker[member.Id];
                if (mv != null)
                {
                    double y = (double)mv.GetValue(Canvas.TopProperty) + height;
                    double x = (double)mv.GetValue(Canvas.LeftProperty) + width / 2;
                    Point p = new Point(x, y);
                    Line line = new Line();
                    line.Stroke = new SolidColorBrush(Colors.White);
                    line.StrokeThickness = 1;
                    line.X1 = p1.X;
                    line.Y1 = p1.Y;
                    line.X2 = p1.X;
                    line.Y2 = p1.Y;
                    this.canvas.Children.Add(line);

                    //Animating line drawing.
                    Storyboard sb = new Storyboard();
                    DoubleAnimation da = new DoubleAnimation(line.Y2, p.Y, new Duration(new TimeSpan(0, 0, 1)));
                    DoubleAnimation da1 = new DoubleAnimation(line.X2, p.X, new Duration(new TimeSpan(0, 0, 1)));
                    Storyboard.SetTargetProperty(da, new PropertyPath("(Line.Y2)"));
                    Storyboard.SetTargetProperty(da1, new PropertyPath("(Line.X2)"));
                    sb.Children.Add(da);
                    sb.Children.Add(da1);
                    lines.Add(line, sb);
                }
            }
        }

        private double getTop(int level)
        {
            return (vCenter - height / 2) + level * height + level * margin * 2;
        }
        private double getLeft(int level, bool isEven)
        {
            double left = (hCenter - width / 2) + level * width + level * margin * 2;
            if (isEven)
                left = left + width / 2 + margin;
            //else
            //    left = left + width + 2 + margin * 2;
            return left;
        }

        private void placeView(IList<Member> members, int level)
        {
            double top = getTop(level);
            bool isEven = members.Count % 2 == 0;
            int divider = (int)(members.Count / 2D + 0.5);
            int total = members.Count;
            foreach (Member member in members)
            {
                //if (isEven && divider == total)
                //    divider++;

                int hLevel = divider - total;

                MemberView mv = memberViewTracker[member.Id];
                double left = getLeft(hLevel, isEven);
                mv.SetValue(Canvas.TopProperty, top);
                mv.SetValue(Canvas.LeftProperty, left);
                divider++;
            }
        }

        private void loadParents(Member member)
        {
            if (member == null)
                return;

            foreach (Member parent in member.Parents)
            {
                MemberView mv = new MemberView();
                mv.DataContext = parent;
                mv.SetValue(MemberView.CommandVisibilityProperty, Visibility.Collapsed);
                if (levelCounter.ContainsKey(-1))
                    levelCounter[-1] = levelCounter[-1] + 1;
                else
                    levelCounter.Add(-1, 1);
                if (!memberViewTracker.ContainsKey(parent.Id))
                    memberViewTracker.Add(parent.Id, mv);
                this.canvas.Children.Add(mv);
            }
        }

        private void loadChildren(Member member)
        {
            ////Create children marker
            //if (member.Children != null && member.Children.Count > 0)
            //{
            //    Rectangle rect = new Rectangle();
            //    rect.Width = (member.Children.Count * width) + (member.Children.Count * margin);
            //    rect.Height = height + (margin);
            //    rect.Stroke = new SolidColorBrush(Colors.White);
            //    rect.StrokeThickness = 1;
            //    double top
            //    rect.SetValue(Canvas.TopProperty, margin / 2);
            //    rect.SetValue(Canvas.LeftProperty, margin / 2);
            //    canvas.Children.Add(rect);
            //}

            foreach (Member child in member.Children)
            {
                MemberView mv = new MemberView();
                mv.DataContext = child;
                mv.SetValue(MemberView.CommandVisibilityProperty, Visibility.Collapsed);
                if (levelCounter.ContainsKey(1))
                    levelCounter[1] = levelCounter[1] + 1;
                else
                    levelCounter.Add(1, 1);
                if (!memberViewTracker.ContainsKey(child.Id))
                    memberViewTracker.Add(child.Id, mv);
                this.canvas.Children.Add(mv);
            }
        }

        private void setScroll()
        {
            scroll.ScrollToVerticalOffset(scroll.ScrollableHeight / 2);
            scroll.ScrollToHorizontalOffset(scroll.ScrollableWidth / 2);
        }

        private void scroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            setScroll();
        }
    }
}
