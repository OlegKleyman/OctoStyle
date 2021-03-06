﻿namespace OctoStyle.Core
{
    /// <summary>
    /// Represents a pull request commenter factory.
    /// </summary>
    public interface IPullRequestCommenterFactory
    {
        /// <summary>
        /// Gets a <see cref="PullRequestCommenter"/>.
        /// </summary>
        /// <param name="status">The target <see cref="GitHubPullRequestFileStatus"/> of file to comment on.</param>
        /// <returns>A <see cref="PullRequestCommenter"/>.</returns>
        PullRequestCommenter GetCommenter(GitHubPullRequestFileStatus status);
    }
}