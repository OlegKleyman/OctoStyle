﻿namespace OctoStyle.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public static class RenamedPullRequestCommenterTest
    {
        [Test]
        public static async void CreateShouldCreateComment()
        {
            PullRequestCommenter commenter = GetRenamedPullRequestCommenter();
            var pullRequestFile = new GitHubPullRequestFile(
                "src/TestLibrary/Nested/TestClass2.cs",
                GitHubPullRequestFileStatus.Renamed,
                1,
                FileContents.TestClass2CsDiff);

            var comment =
                (await
                 commenter.Create(
                     new GitHubPullRequest(
                     1,
                     "123",
                     new List<GitHubPullRequestFile>(),
                     FileContents.TestClass2CsDiff,
                     new GitHubPullRequestBranches("test_branch", "master")),
                     pullRequestFile,
                     null,
                     @"C:\repo\TestLibrary\Nested\TestClass2.cs")).ToList();

            Assert.That(comment.Count, Is.EqualTo(1));
            Assert.That(comment[0].Path, Is.EqualTo("src/TestLibrary/Nested/TestClass2.cs"));
            Assert.That(comment[0].Body, Is.EqualTo("Renamed files not supported."));
            Assert.That(comment[0].Id, Is.EqualTo(1));
            Assert.That(comment[0].Position, Is.EqualTo(1));
        }

        private static RenamedPullRequestCommenter GetRenamedPullRequestCommenter()
        {
            var pullRequestCommentClient = new Mock<IPullRequestReviewCommentsClient>();
            pullRequestCommentClient.Setup(
                client =>
                client.Create(
                    "OlegKleyman",
                    "OctoStyle",
                    1,
                    It.Is<PullRequestReviewCommentCreate>(
                        create =>
                        create.Path == "src/TestLibrary/Nested/TestClass2.cs"
                        && create.Body == "Renamed files not supported." && create.CommitId == "123"
                        && create.Position == 1)))
                .ReturnsAsync(
                    new PullRequestReviewComment(
                        null,
                        1,
                        null,
                        "src/TestLibrary/Nested/TestClass2.cs",
                        1,
                        null,
                        "1",
                        null,
                        null,
                        "Renamed files not supported.",
                        default(DateTimeOffset),
                        default(DateTimeOffset),
                        null,
                        null));

            return new RenamedPullRequestCommenter(
                pullRequestCommentClient.Object,
                new GitHubRepository("OlegKleyman", "OctoStyle"));
        }
    }
}