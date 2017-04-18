using DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class MemberDBAgent : IMemberDBAgent
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
                    member.LastName = reader["LastName"] == DBNull.Value ? String.Empty : (String)reader["LastName"];
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

        public void SaveMember(Member member, Action<bool, long> result)
        {
            SqlInt32 mId = member.MotherId != 0 ? SqlInt32.Parse(member.MotherId.ToString()) : SqlInt32.Null;
            SqlInt32 fId = member.FatherId != 0 ? SqlInt32.Parse(member.FatherId.ToString()) : SqlInt32.Null;
            string sex = member.Sex.Equals(Gender.Male) ? "M" : "F";
            SqlDateTime dob = new SqlDateTime(member.DateOfBirth);
            using (var connection = new SqlConnection(CS))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_saveMember";
                SqlParameter MemberID = command.Parameters.Add(new SqlParameter("@MemberID", SqlDbType.Int));
                MemberID.Direction = ParameterDirection.InputOutput;
                MemberID.Value = member.Id;
                SqlParameter FirstName = command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50));
                FirstName.Direction = ParameterDirection.Input;
                FirstName.Value = member.FirstName;
                SqlParameter LastName = command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50));
                LastName.Direction = ParameterDirection.Input;
                LastName.Value = member.LastName == null ? SqlString.Null : member.LastName;
                SqlParameter DateOfBirth = command.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.DateTime));
                DateOfBirth.Direction = ParameterDirection.Input;
                DateOfBirth.Value = dob;
                SqlParameter Sex = command.Parameters.Add(new SqlParameter("@Sex", SqlDbType.NVarChar, 1));
                Sex.Direction = ParameterDirection.Input;
                Sex.Value = sex;
                SqlParameter MMemberID = command.Parameters.Add(new SqlParameter("@MMemberID", SqlDbType.Int));
                MMemberID.Direction = ParameterDirection.Input;
                MMemberID.Value = mId;
                SqlParameter FMemberID = command.Parameters.Add(new SqlParameter("@FMemberID", SqlDbType.Int));
                FMemberID.Direction = ParameterDirection.Input;
                FMemberID.Value = fId;
                SqlParameter IsSuccess = command.Parameters.Add(new SqlParameter("@IsSuccess", SqlDbType.Int));
                IsSuccess.Direction = ParameterDirection.Output;

                connection.Open();
                int res = command.ExecuteNonQuery();
                connection.Close();

                if (res > 0 && result != null)
                {
                    result(true, long.Parse(MemberID.Value.ToString()));
                }
                else
                {
                    result(false, 0);
                }
            }
        }

        public IDictionary<long, Member> GetMemberWithParentAndChild(long memberId)
        {
            IDictionary<long, Member> members = new Dictionary<long, Member>();

            using (SqlConnection con = new SqlConnection(CS))
            using (var cmd = con.CreateCommand())
            {
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Select * from dbo.getMemberIncludingParentAndChildren(" + memberId + ")";

                //SqlParameter paremeter = cmd.Parameters.Add("@MemberID", SqlDbType.Int);
                //paremeter.Direction = ParameterDirection.Input;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Member member = readerToMember(reader);

                    members.Add(member.Id, member);
                }
                con.Close();
            }
            return members;
        }

        private static Member readerToMember(SqlDataReader reader)
        {
            long id = reader.GetInt32(reader.GetOrdinal("MemberID"));
            Member member = new Member(id);
            member.FirstName = (String)reader["FirstName"];
            member.LastName = reader["LastName"] == DBNull.Value ? String.Empty : (String)reader["LastName"];
            member.DateOfBirth = (DateTime)reader["DateOfBirth"];
            member.Sex = reader["Sex"].ToString().Equals("M") ? Gender.Male : Gender.Female;
            if (reader["MMemberID"] != DBNull.Value)
                member.MotherId = long.Parse(reader["MMemberID"].ToString());
            if (reader["FMemberID"] != DBNull.Value)
                member.FatherId = long.Parse(reader["FMemberID"].ToString());
            member.HasChildInDB = Boolean.Parse(reader["hasChild"].ToString());
            member.HasParentInDB = Boolean.Parse(reader["hasParent"].ToString());
            return member;
        }

        public void GetMember(long memberId, Action<Member> action)
        {
            using (var connection = new SqlConnection(CS))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from getMember(" + memberId + ")";

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Member member = null;
                if (reader.Read())
                {
                    member = readerToMember(reader);
                };
                connection.Close();
                if (action != null)
                    action(member);
            }
        }

        public void DeleteMember(long memberId, Action<bool> result)
        {
            using (var connection = new SqlConnection(CS))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_deleteMember";
                SqlParameter MemberID = command.Parameters.Add(new SqlParameter("@MemberID", SqlDbType.Int));
                MemberID.Direction = ParameterDirection.Input;
                MemberID.Value = memberId;
                SqlParameter IsSuccess = command.Parameters.Add(new SqlParameter("@IsSuccess", SqlDbType.Bit));
                IsSuccess.Direction = ParameterDirection.Output;
                connection.Open();
                int res = command.ExecuteNonQuery();
                connection.Close();
                if (result != null)
                    result((bool)IsSuccess.Value);
            }
        }

        public IList<Member> GetChildMostMembers()
        {
            IList<Member> members = new List<Member>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                string query = @"SELECT * FROM [dbo].[getChildMostMembers] ()";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Member member = readerToMember(reader);
                    members.Add(member);
                }
                con.Close();
            }
            return members;
        }
    }
}
