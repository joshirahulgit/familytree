using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBAgentFactory
    {
        public static IMemberDBAgent GetNewMemberDBAgent() {
            return new MemberDBAgent();
        }
    }
}
