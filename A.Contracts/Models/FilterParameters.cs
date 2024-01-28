namespace A.Contracts.Models
{
    public class FilterParameters
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public string Department { get; set; }

        public string Gender { get; set; }
    }
}
