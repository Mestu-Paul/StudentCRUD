using A.Contracts.Contracts;
using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using C.BusinessLogic.ILoigcs;
using Microsoft.AspNetCore.JsonPatch;

namespace C.BusinessLogic.Logics
{
    public class StudentLogic : IStudentLogic
    {
        private readonly IStudentDataAccess _studentsService;
        private readonly StudentResponse _studentResponse = new StudentResponse();

        public StudentLogic(IStudentDataAccess studentsService)
        {
            _studentsService = studentsService;
        }

        public async Task<StudentResponse> CreateNewStudentAsync(Student student)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;

            student.CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"); ;
            student.LastUpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                await _studentsService.CreateNewStudentAsync(student);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "Created a new student";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while creating a new student";
            }
            return _studentResponse;
        }
        
        public async Task<StudentResponse> GetAllStudentsAsync()
        {
            try
            {
                _studentResponse.students = await _studentsService.GetAllStudentsAsync();
                _studentResponse.isSuccess = true;
                _studentResponse.message = "";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something error while call GetAllStudents API";
                _studentResponse.isSuccess = false;
                _studentResponse.students = null;
            }
            return _studentResponse;

        }

        public async Task<StudentResponse> GetStudentsPagedAsync(int pageNumber, int pageSize)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            if (pageNumber <= 0 || pageSize <= 0)
            {
                _studentResponse.message = "Page number and size can not be 0 or negative";
                return _studentResponse;
            }
            try
            {
                _studentResponse.students = await _studentsService.GetStudentsPagedAsync(pageNumber, pageSize);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "";
            }
            catch (ArgumentOutOfRangeException e)
            {
                _studentResponse.message = "Invalid page number or size";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while call GetStudents API";
            }
            return _studentResponse;

        }

        public async Task<long> GetTotalNumberOfStudentsAsync()
        {
            return await _studentsService.GetTotalNumberOfStudentsAsync();
        }

        public async Task<StudentResponse> UpdateStudentSingleAttributeAsync(string id, string fieldName, string fieldValue)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            if (id.Length <= 0)
            {
                _studentResponse.message = "ID can not be empty!";
                return _studentResponse;
            }
            try
            {
                await _studentsService.UpdateStudentSingleAttributeAsync(id, fieldName, fieldValue);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "Updated students information";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while updating students information";
            }
            return _studentResponse;
        }

        public async Task<StudentResponse> UpdateStudentAsync(string id, Student student)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            if (id.Length <= 0)
            {
                _studentResponse.message = "ID can not be empty!";
                return _studentResponse;
            }
            try
            {
                await _studentsService.UpdateStudentAsync(id,student);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "Updated students information";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while updating students information";
            }
            return _studentResponse;
        }

        public async Task<StudentResponse> DeleteStudentAsync(string id)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            try
            {
                bool isDeleted = await _studentsService.DeleteStudentAsync(id);
                _studentResponse.isSuccess |= isDeleted;
                if (isDeleted)
                {
                    _studentResponse.message = "Successfully deleted";
                }
                else
                {
                    _studentResponse.message = "Invalid student information or not found";
                }
            }
            catch (FormatException e)
            {
                _studentResponse.message = "Invalid  id format";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while deleting a students information";
            }
            return _studentResponse;
        }
    }
}
