using System.Collections.Generic;
using DataModel;
using System;

namespace DAL
{
    public interface IMemberDBAgent
    {
        IDictionary<long, Member> GetMembers();
        void SaveMember(Member member,Action<bool,long> result);
        IDictionary<long, Member> GetMemberWithParentAndChild(long memberId);

        void GetMember(long memberId, Action<Member> action);

        void DeleteMember(long memberId, Action<bool> result);
        IList<Member> GetChildMostMembers();
    }
}