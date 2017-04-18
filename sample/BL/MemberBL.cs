using DAL;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    internal class MemberBL : IMemberBL
    {

        private IMemberDBAgent dbAgent;

        public MemberBL()
        {
            dbAgent = DBAgentFactory.GetNewMemberDBAgent();
        }

        public IList<Member> getAllMemberTree()
        {
            IDictionary<long, Member> members = createMemberTree(dbAgent.GetMembers());

            return members.Select(t1 => t1.Value).Where(t2 => !t2.HasChild).ToList();
        }

        private IDictionary<long, Member> createMemberTree(IDictionary<long, Member> members)
        {
            foreach (KeyValuePair<long, Member> member in members)
            {
                member.Value.setParents(members);
            }

            return members;
        }

        public Member GetMemberWithParentAndChild(long memberId)
        {
            IDictionary<long, Member> members = createMemberTree(dbAgent.GetMemberWithParentAndChild(memberId));
            if (members != null && members.ContainsKey(memberId))
                return members[memberId];
            return null;
        }

        public void SaveMember(Member member, Action<bool,long> result)
        {
            dbAgent.SaveMember(member,result);
        }

        public void GetMember(long memberId, Action<Member> action)
        {
            dbAgent.GetMember(memberId, action);
        }

        public void DeleteMember(long memberId, Action<bool> result)
        {
            dbAgent.DeleteMember(memberId, result);
        }

        public IList<Member> GetChildMostMembers()
        {
            return dbAgent.GetChildMostMembers();
        }
    }
}
