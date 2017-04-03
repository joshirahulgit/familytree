using DAL;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MemberBL
    {

        private IMemberDBAgent dbAgent;

        public MemberBL()
        {
            dbAgent = DBAgentFactory.GetNewMemberDBAgent();
        }

        public IList<Member> getAllMemberTree()
        {
            IDictionary<long, Member> members = dbAgent.GetMembers();
            foreach (KeyValuePair<long, Member> member in members)
            {
                member.Value.setParents(members);
            }

            return members.Select(t1 => t1.Value).Where(t2 => !t2.HasChild).ToList();
        }

        public bool SaveMember(Member member)
        {
            return dbAgent.SaveMember(member);
        }
    }
}
