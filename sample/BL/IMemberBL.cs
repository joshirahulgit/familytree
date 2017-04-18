using System.Collections.Generic;
using DataModel;
using System;

namespace BL
{
    public interface IMemberBL
    {
        IList<Member> getAllMemberTree();
        void SaveMember(Member member, Action<bool,long> result);
        Member GetMemberWithParentAndChild(long memberId);

        void GetMember(long memberId,Action<Member> action);

        void DeleteMember(long memberId, Action<bool> result);
    }
}