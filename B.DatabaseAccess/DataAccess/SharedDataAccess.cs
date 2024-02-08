using A.Contracts.Entities;
using B.DatabaseAccess.IDataAccess;

namespace B.DatabaseAccess.DataAccess
{
    public class SharedDataAccess : ISharedDataAccess
    {
        private readonly IAccountDataAccess _account;
        private readonly IStudentDataAccess _student;
        private readonly ITeacherDataAccess _teacher;
        public SharedDataAccess(IAccountDataAccess account, IStudentDataAccess student, ITeacherDataAccess teacher)
        {
            _account = account;
            _student = student;
            _teacher = teacher;
        }
        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await _account.GetUserAsync(username);
            if (user == null)
            {
                return false;
            }

            if (user.Role == "teacher")
            {
                var teacher =  await _teacher.GetTeacherAsync(username);
                await _teacher.DeleteTeacherAsync(teacher.Id);
            }
            else if (user.Role == "student")
            {
                var student = await _student.GetStudentAsync(username);
                await _student.DeleteStudentAsync(student.Id);
            }

            return await _account.DeleteUserAsync(username);
        }

        public async Task CreateNewUser(string username, string role)
        {
            if (role == "teacher")
            {
                Teacher teacher = new Teacher();
                teacher.Username = username;
                await _teacher.CreateNewTeacherAsync(teacher);
            }
            else if (role == "student")
            {
                Student student = new Student();
                student.Username = username;
                await _student.CreateNewStudentAsync(student);
            }
        }
    }
}
