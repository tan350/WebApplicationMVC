using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using VO;

namespace DL
{
    public class UserDL
    {
        string connectionString = @"Data Source=(localdb)\MSSqlLocalDb;Database=PracticeDB;Integrated Security=True;TrustServerCertificate=True;";


        public void AuthenticateUser(UserVO userVO)
        {
            string queryString = @"Select COUNT(*) From Users Where UserRole = @UserRole AND UserName = @UserName AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@UserRole", userVO.UserRole);
                command.Parameters.AddWithValue("@UserName", userVO.UserName);
                command.Parameters.AddWithValue("@Password", userVO.Password);

                connection.Open();

                int count = (int)command.ExecuteScalar();

                if (count == 0)
                {
                    userVO.ErrorMessage="Credentials are not valid";  
                }
            }
        }

        public List<StudentVO> ReadAllRecordsData()
        {
            List<StudentVO> studentRecord = new List<StudentVO>();
            string queryString = @"Select * From Student";
            using (SqlConnection connection=new SqlConnection(connectionString)) 
            {
                SqlCommand command = new SqlCommand(queryString,connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                { 
                    StudentVO student = new StudentVO();
                    student.SrNo = Convert.ToInt32(reader["SrNo"]);
                    student.Name= reader["Name"].ToString();
                    student.Course = reader["Course"].ToString();
                    student.College = reader["College"].ToString();
                    studentRecord.Add(student);
                }
                
            }
            return studentRecord;
        }

        public void AddRecordData(StudentVO student)
        {
            string queryString = @"Insert Into Student (Name,Course, College) Values (@Name,@Course,@College);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                command.Parameters.AddWithValue("@Name",student.Name);
                command.Parameters.AddWithValue("@Course",student.Course);
                command.Parameters.AddWithValue("@College",student.College);

                command.ExecuteNonQuery();

            }
        }

        public int GetLastSrNo()
        { 
            List<StudentVO> allRecords =new List<StudentVO>();
            allRecords = ReadAllRecordsData();
            int lastSrNo;

            if (allRecords.Any())
            {
                var sortedRecords= allRecords.OrderByDescending(x => x.SrNo);
                lastSrNo =sortedRecords.First().SrNo;
            }
            else 
            {
                lastSrNo = 0;
            }
            int nextSrNo= lastSrNo + 1;
            return nextSrNo;
        }

        public void UpdateRecordData(int SrNo, StudentVO student)
        {
            string queryString = @"Update Student set Name = @Name, Course = @Course, College = @College where SrNo = @SrNo";

            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                SqlCommand command = new SqlCommand(queryString,connection);
                connection.Open();

                command.Parameters.AddWithValue("@SrNo",SrNo);
                command.Parameters.AddWithValue("@Name",student.Name);
                command.Parameters.AddWithValue("@Course",student.Course);
                command.Parameters.AddWithValue("@College",student.College);

                command.ExecuteNonQuery();
            }
        }

        public StudentVO GetRecordBySrNo(int SrNo) 
        {
            string queryString = "Select * From Student Where SrNo=@SrNo";

            StudentVO student;

            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                using (SqlCommand command = new SqlCommand(queryString, connection)) 
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@SrNo", SrNo);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        student = new StudentVO
                        {
                            SrNo = (int)reader["SrNo"],
                            Name = reader["Name"].ToString(),
                            Course = reader["Course"].ToString(),
                            College = reader["College"].ToString()
                        };
                    }
                    else 
                    {
                        throw new Exception("Record not found");
                    }
                }
                
            }
            return student;
        }

        public void DeleteRecordData( int SrNo)
        {
            string queryString = "Delete From Student Where SrNo = @SrNo";

            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                using (SqlCommand command = new SqlCommand(queryString, connection)) 
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@SrNo",SrNo);
                    command.ExecuteNonQuery();
                }
            }
        
        }
    }
}
