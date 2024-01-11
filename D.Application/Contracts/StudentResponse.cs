using A.Contracts.Models;

namespace D.Application.Contracts
{
    public class StudentResponse
    {
        public List<Student> students { get; set; } = null;
        public string message { get; set; } = string.Empty;
        public bool isSuccess { get; set; } = false;
    }
}
