using BL;
using ClientApp.Helper;
using DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClientApp.UserControls
{
    class SaveMemberViewModel : ViewModelBase
    {
        private IMemberBL memberBL;

        public SaveMemberViewModel()
        {
            memberBL = BLFactory.GetNewMemberBL();
        }

        private bool? isMotherIdGood;

        private bool? isFatherIdGood;

        private Member selectedMember;

        public Member SelectedMember
        {
            get { return selectedMember; }
            set
            {
                if (selectedMember != null)
                    selectedMember.PropertyChanged -= SelectedMember_PropertyChanged;
                selectedMember = value;
                if (selectedMember != null)
                    selectedMember.PropertyChanged += SelectedMember_PropertyChanged;
                RaisePropertyChanged("SelectedMember");
            }
        }

        private void SelectedMember_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Member member = sender as Member;
            if (e.PropertyName == "MotherId")
            {
                if (member.MotherId != 0)
                {
                    //Check if mother Id exist as member id and is female.
                    memberBL.GetMember(((Member)sender).MotherId, memberRes =>
                    {
                        if (memberRes != null && memberRes.Sex.Equals(Gender.Female))
                        {
                            IsMotherIdGood = true;
                        }
                        else
                        {
                            IsMotherIdGood = false;
                        }
                    });

                }
                else
                {
                    IsMotherIdGood = null;
                }
            }

            if (e.PropertyName == "FatherId")
            {
                if (member.FatherId != 0)
                {
                    //Check if mother Id exist as member id and is female.
                    memberBL.GetMember(((Member)sender).FatherId, memberRes =>
                    {
                        if (memberRes != null && memberRes.Sex.Equals(Gender.Male))
                        {
                            IsFatherIdGood = true;
                        }
                        else
                        {
                            IsFatherIdGood = false;
                        }
                    });

                }
                else
                {
                    IsFatherIdGood = null;
                }
            }
        }

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

        public ICommand SaveMemberCmd
        {
            get
            {
                return new SaveMemberCommand(this);
            }
        }

        public bool? IsMotherIdGood
        {
            get
            {
                return isMotherIdGood;
            }

            set
            {
                isMotherIdGood = value;
                RaisePropertyChanged("IsMotherIdGood");
            }
        }

        public bool? IsFatherIdGood
        {
            get
            {
                return isFatherIdGood;
            }

            set
            {
                isFatherIdGood = value;
                RaisePropertyChanged("IsFatherIdGood");
            }
        }

        public void SaveSelectedMember()
        {
            //Validate all fields
            StringBuilder errorSB = new StringBuilder();
            if (String.IsNullOrEmpty(SelectedMember.FirstName))
                errorSB.Append("'First Name' is required.");

            if (SelectedMember.DateOfBirth == null)
            {
                if (errorSB.Length > 0)
                    errorSB.AppendLine();
                errorSB.Append("'Date of birth' is required.");
            }

            if (IsFatherIdGood!=null && !IsFatherIdGood.Value) {
                if (errorSB.Length > 0)
                    errorSB.AppendLine();
                errorSB.Append("'Father id' can be '0' or an exiting male member id.");
            }

            if (IsMotherIdGood != null && !IsMotherIdGood.Value)
            {
                if (errorSB.Length > 0)
                    errorSB.AppendLine();
                errorSB.Append("'Mother id' can be '0' or an exiting female member id.");
            }

            if (errorSB.Length > 0)
            {
                MessageBox.Show(errorSB.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Show success message and ask to view member.
            memberBL.SaveMember(SelectedMember, (res, memberId) =>
            {
                if (res)
                {
                    SelectedMember.PropertyChanged -= SelectedMember_PropertyChanged;
                    SelectedMember = new Member(0);
                    SelectedMember.PropertyChanged -= SelectedMember_PropertyChanged;
                    MessageBox.Show("New member created with Id: " + memberId);
                }
                else
                {
                    MessageBox.Show("Problem while creating member! Please retry.");
                }
            });
        }
    }

    class SaveMemberCommand : ICommand
    {
        private SaveMemberViewModel viewModel;

        public event EventHandler CanExecuteChanged;

        public SaveMemberCommand(SaveMemberViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.viewModel.SaveSelectedMember();
        }
    }
}
