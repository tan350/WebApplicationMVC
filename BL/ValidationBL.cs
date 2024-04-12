using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VO;
using DL;

namespace BL
{
    public class ValidationBL
    {
        UserDL userDLObject = new UserDL();

        public bool isUserExist(UserVO userVO)
        {
            userDLObject.AuthenticateUser(userVO);
            if (userVO.ErrorMessage != null)
            {
                return false;
            }
            return true;
        }

        public List<StudentVO> ReadAllRecords()
        {
            List<StudentVO> studentRecord = new List<StudentVO>();
            studentRecord = userDLObject.ReadAllRecordsData();

            return studentRecord;
        }

        public void AddNewRecord(StudentVO student)
        { 
            userDLObject.AddRecordData(student);
        }

        public int GetSrNoForNewRecord()
        {
            int nextSrNo = userDLObject.GetLastSrNo();
            return nextSrNo;
        }

        public void UpdateRecord(int SrNo, StudentVO student)
        {
            userDLObject.UpdateRecordData(SrNo,student);
        }

        public StudentVO GetRecordUsingSrNo(int SrNo)
        {
            StudentVO student = new StudentVO();
            student = userDLObject.GetRecordBySrNo(SrNo);
            return student;
        }

        public void  DeleteRecord(int SrNo)
        {
            userDLObject.DeleteRecordData(SrNo);
        }
    }
}
