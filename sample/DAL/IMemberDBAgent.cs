using System.Collections.Generic;
using DataModel;

namespace DAL
{
    public interface IMemberDBAgent
    {
        IDictionary<long, Member> GetMembers();
        bool SaveMember(Member member);
    }
}