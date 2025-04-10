namespace CommonLibrary.Constants;

public class CacheKeyConstants(string entityName)
{
    public readonly string GetAll = $"{entityName}GetAll";
    public readonly string GetById = $"{entityName}GetById-";

}
