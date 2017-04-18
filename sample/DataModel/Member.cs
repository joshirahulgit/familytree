using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Member : INotifyPropertyChanged
    {
        public Member(long id)
        {
            this.Id = id;
            Parents = new List<Member>();
            Children = new List<Member>();
            Sex = Gender.Male;
            DateOfBirth = DateTime.Today.AddYears(-18);
        }

        public long Id { get; private set; }
        private String firstName;
        private String lastName;
        private DateTime dateOfBirth;
        private Gender sex;
        private IList<Member> parents;
        private IList<Member> children;
        private long motherId;
        private long fatherId;
        public bool HasChild { get; private set; }

        public bool HasChildInDB { get; set; }
        public bool HasParentInDB { get; set; }

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        public DateTime DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }

            set
            {
                dateOfBirth = value;
                RaisePropertyChanged("DateOfBirth");
                RaisePropertyChanged("Age");
            }
        }

        public int Age
        {
            get
            {
                // Save today's date.
                var today = DateTime.UtcNow;
                // Calculate the age.
                var age = today.Year - DateOfBirth.Year;
                // Do stuff with it.
                if (DateOfBirth > today.AddYears(-age))
                    age--;
                return age;
            }
        }

        public Gender Sex
        {
            get
            {
                return sex;
            }

            set
            {
                sex = value;
                RaisePropertyChanged("Sex");
            }
        }

        public IList<Member> Parents
        {
            get
            {
                return parents;
            }

            set
            {
                parents = value;
                RaisePropertyChanged("Parents");
            }
        }

        public IList<Member> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value;
                RaisePropertyChanged("Children");
            }
        }

        public long MotherId
        {
            get
            {
                return motherId;
            }

            set
            {
                motherId = value;
                RaisePropertyChanged("MotherId");
            }
        }

        public long FatherId
        {
            get
            {
                return fatherId;
            }

            set
            {
                fatherId = value;
                RaisePropertyChanged("FatherId");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void setParents(IDictionary<long, Member> members)
        {
            if (MotherId!=0 && members.ContainsKey(MotherId))
            {
                Member mother = members[MotherId];
                mother.HasChild = true;
                Parents.Add(mother);
                mother.AddChild(this);
            }
            if (FatherId!=0 && members.ContainsKey(FatherId))
            {
                Member father = members[FatherId];
                father.HasChild = true;
                Parents.Add(father);
                father.AddChild(this);
            }

        }

        private void AddChild(Member child)
        {
            this.Children.Add(child);
        }

        private void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
