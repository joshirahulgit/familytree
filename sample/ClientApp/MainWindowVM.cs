using DAL;
using DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private MemberDBAgent dbAgent;

        private IList<Member> members;

        public event PropertyChangedEventHandler PropertyChanged;

        public Member SelectedMember { get; set; }

        public IDictionary<Gender, string> AllGenders
        {
            get
            {
                IDictionary<Gender, string> items = new Dictionary<Gender, string>();
                items.Add(Gender.Male, "Male");
                items.Add(Gender.Female, "Female");
                return items;
            }
        }

        private ICommand _saveMemberCmd;

        public IList<Member> Members
        {
            get
            {
                return members;
            }

            set
            {
                members = value;
                RaisePropertyChanged("Members");
            }
        }

        public Member ToEditMember
        {
            get
            {
                return toEditMember;
            }

            set
            {
                toEditMember = value;
                RaisePropertyChanged("ToEditMember");
            }
        }

        public ICommand SaveMemberCmd
        {
            get
            {
                return new SaveMemberCommand(this);
            }
        }

        private Member toEditMember;


        public MainWindowVM()
        {
            dbAgent = new MemberDBAgent();
            ToEditMember = new Member(0);
            ReadAndSetTree();
        }

        private void ReadAndSetTree()
        {
            IDictionary<long, Member> members = dbAgent.GetMembers();
            foreach (KeyValuePair<long, Member> member in members)
            {
                member.Value.setParents(members);
            }

            this.Members = members.Select(t1 => t1.Value).Where(t2 => !t2.HasChild).ToList();
        }

        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public void SaveToEditMember()
        {
            if (this.dbAgent.SaveMember(ToEditMember)) { 
                ReadAndSetTree();
                ToEditMember = new Member(0);
            }
        }
    }
    class SaveMemberCommand : ICommand
    {
        private MainWindowVM mainVM;

        public event EventHandler CanExecuteChanged;

        public SaveMemberCommand(MainWindowVM mainVM)
        {
            this.mainVM = mainVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.mainVM.SaveToEditMember();
        }
    }
}
