using MicroService101.Models;

namespace MicroService101.Interfaces
{
    public interface IStudentService
    {
        string GetStudentInfoById(int id);
        List<Student> GetAllStudents(); 
    }
}
