namespace OctoStyle.Core
{
    public interface IPathResolver
    {
        string GetPath(string initialPath, string fileFilter);
    }
}