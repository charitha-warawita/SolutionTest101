using MicroService101.Interfaces;
using MicroService101.Models;

namespace MicroService101.Services
{
    public class StudentService : IStudentService
    {
        List<Student> studentList;
        public StudentService()
        {
            studentList = new List<Student> { };
            studentList.Add(new Student
            {
                Id = 1,
                Name = "Yash",
                remarks = "Lives in India"
            });

            studentList.Add(new Student
            {
                Id = 2,
                Name = "Hyma",
                remarks = "Lives in UK"
            });
        }
        public List<Student> GetAllStudents()
        {
            return studentList;
        }

        public string GetStudentInfoById(int id)
        {
            var result = studentList.Where(x => x.Id == id).Select(x => x.remarks).FirstOrDefault();
            return result != null ? result : "";
            //throw new NotImplementedException();
        }
    }
}
