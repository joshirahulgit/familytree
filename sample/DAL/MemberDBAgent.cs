using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MemberDBAgent
    {
        private static string CS;

        static MemberDBAgent()
        {
            CS = System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        }

        public IDictionary<long, Member> GetMembers()
        {
            IDictionary<long, Member> members = new Dictionary<long, Member>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                string query = @"Select Id, FirstName,LastName, DateOfBirth, Sex, MotherId, FatherId from dbo.Member";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    long id = long.Parse(reader["Id"].ToString());
                    Member member = new Member(id);
                    member.FirstName = (String)reader["FirstName"];
                    member.LastName = (String)reader["LastName"];
                    member.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    member.Sex = reader["Sex"].ToString().Equals("M") ? Gender.Male : Gender.Female;
                    if (reader["MotherId"] != DBNull.Value)
                        member.MotherId = long.Parse(reader["MotherId"].ToString());
                    if (reader["FatherId"] != DBNull.Value)
                        member.FatherId = long.Parse(reader["FatherId"].ToString());
                    members.Add(member.Id, member);

                }
                con.Close();
            }
            return members;
        }

        public bool SaveMember(Member member)
        {
            SqlInt32 mId = member.MotherId != null ? SqlInt32.Parse(member.MotherId.ToString()) : SqlInt32.Null;
            SqlInt32 fId = member.FatherId != null ? SqlInt32.Parse(member.FatherId.ToString()) : SqlInt32.Null;
            string sex = member.Sex.Equals(Gender.Male) ? "M" : "F";
            SqlDateTime dob = new SqlDateTime(member.DateOfBirth);
            using (SqlConnection con = new SqlConnection(CS))
            {
                string query = "";
                if (member.Id == 0)
                    query = @"INSERT INTO dbo.Member
                                       (FirstName,
                                        LastName,
                                        DateOfBirth,
                                        Sex,
                                        MotherId,
                                        FatherId)
                                 VALUES
                                       ('" + member.FirstName + @"',
                                        '" + member.LastName + @"',
                                        '" + dob + @"',
                                        '" + sex + @"',
                                        " + mId + @",
                                        " + fId + @");";
                else
                    query = @"UPDATE dbo.Member
                           SET FirstName = '" + member.FirstName + @"',
                               LastName = '" + member.LastName + @"',
                               DateOfBirth = '" + dob + @"',
                               Sex = '" + sex + @"',
                               MotherId = " + mId + @",
                               FatherId =  " + fId + @"
                         WHERE Id=" + member.Id + ";";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int count = cmd.ExecuteNonQuery();
                con.Close();
                if (count == 1)
                    return true;
                return false;
            }
        }
    }
}
