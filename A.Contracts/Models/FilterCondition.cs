using MongoDB.Driver;

public class FilterCondition
{
    public string Field { get; set; }
    public string Operator { get; set; }
    public string Value { get; set; }

    public FilterDefinition<T> GetFilterDefinition<T>(FilterDefinitionBuilder<T> builder)
    {
        switch (Operator.ToLower())
        {
            case "eq":
                return builder.Eq(Field, Value);
            case "ne":
                return builder.Ne(Field, Value);
            // Add more cases for other operators as needed
            default:
                throw new ArgumentException($"Unsupported operator: {Operator}");
        }
    }
}