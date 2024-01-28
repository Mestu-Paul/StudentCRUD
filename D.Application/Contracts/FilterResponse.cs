using A.Contracts.Models;

namespace D.Application.Contracts
{
    public class FilterResponse<T>
    {
        public FilterResponse(Tuple<List<T>,long> result, long pageSize)
        {
            Members = result.Item1;
            TotalPages = (result.Item2+pageSize-1)/pageSize;
        }
        public List<T> Members { get; set; }
        public long TotalPages { get; set; }
    }
}
