namespace OctoStyle.Core
{
    public interface IPullRequestCommenterFactory
    {
        PullRequestCommenter Get(GitPullRequestFileStatus status);
    }
}