namespace OctoStyle.Core
{
    using System.Threading.Tasks;

    public interface IPullRequestRetriever
    {
        Task<GitHubPullRequest> Retrieve(int number);
    }
}