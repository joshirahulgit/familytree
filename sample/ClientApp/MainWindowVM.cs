using BL;
using ClientApp.Helper;
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
    class MainWindowVM : ViewModelBase
    {
        private IMemberBL memberBL;

        private IList<Member> members;

        private bool isEditable;

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

        public ICommand EditMemberCmd
        {
            get
            {
                return new EditMemberCommand(this);
            }
        }

        public ICommand AddNewMemberCmd
        {
            get
            {
                return new AddNewMemberCommand(this);
            }
        }

        public ICommand CancelEditCmd
        {
            get
            {
                return new CancelEditCommand(this);
            }
        }

        public bool IsEditable
        {
            get
            {
                return isEditable;
            }

            set
            {
                isEditable = value;
                RaisePropertyChanged("IsEditable");
            }
        }

        private Member toEditMember;


        public MainWindowVM()
        {
            IsEditable = false;
            memberBL = BLFactory.GetNewMemberBL();
            ReadAndSetTree();
        }

        private void ReadAndSetTree()
        {
            this.Members = memberBL.getAllMemberTree();
        }
    }

    class EditMemberCommand : ICommand
    {
        private MainWindowVM mainVM;

        public event EventHandler CanExecuteChanged;

        public EditMemberCommand(MainWindowVM mainVM)
        {
            this.mainVM = mainVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.mainVM.IsEditable=true;
        }
    }

    class AddNewMemberCommand : ICommand
    {
        private MainWindowVM mainVM;

        public event EventHandler CanExecuteChanged;

        public AddNewMemberCommand(MainWindowVM mainVM)
        {
            this.mainVM = mainVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mainVM.IsEditable = true;
            this.mainVM.ToEditMember = new Member(0);
        }
    }

    class CancelEditCommand : ICommand
    {
        private MainWindowVM mainVM;

        public event EventHandler CanExecuteChanged;

        public CancelEditCommand(MainWindowVM mainVM)
        {
            this.mainVM = mainVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mainVM.IsEditable = false;
            this.mainVM.ToEditMember = null;
        }
    }
}
