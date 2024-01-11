using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A.Contracts.Models;

namespace A.Contracts.Contracts
{
    public class StudentResponse
    {
        public List<Student> students { get; set; } = null;
        public string message { get; set; } = string.Empty;
        public bool isSuccess { get; set; } = false;
    }
}
