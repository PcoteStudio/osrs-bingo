namespace Bingo.Api.Data;

[Serializable]
public class RelatedEntitiesNotLoadedException : Exception
{
    public RelatedEntitiesNotLoadedException(string variableName, string relatedEntityName)
        : base($"{relatedEntityName} not loaded on {variableName}")
    {
        VariableName = variableName;
        RelatedEntityName = relatedEntityName;
    }

    public RelatedEntitiesNotLoadedException(string variableName, string relatedEntityName, string message)
        : base($"{relatedEntityName} not loaded on {variableName}: {message}")
    {
        VariableName = variableName;
        RelatedEntityName = relatedEntityName;
    }

    public RelatedEntitiesNotLoadedException(string variableName)
        : base($"{variableName} was not loaded on entity. Probably a missing .Include() .ThenInclude()")
    {
        VariableName = variableName;
    }

    public string VariableName { get; }
    public string? RelatedEntityName { get; }
}