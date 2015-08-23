namespace OctoStyle.Core
{
    using System.Collections.Generic;

    public interface IDiffParser
    {
        IDictionary<string, string> Split(string diff);
    }
}