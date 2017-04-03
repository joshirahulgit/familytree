using System.Collections.Generic;
using DataModel;

namespace BL
{
    public interface IMemberBL
    {
        IList<Member> getAllMemberTree();
        bool SaveMember(Member member);
    }
}