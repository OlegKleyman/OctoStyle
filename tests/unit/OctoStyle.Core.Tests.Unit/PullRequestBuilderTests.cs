namespace OctoStyle.Core.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Moq;

    using NUnit.Framework;

    using Octokit;

    [TestFixture]
    public class PullRequestBuilderTests
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter",
            Justification = StyleCopConstants.LocalConstantJustification)]
        [Test]
        public void BuildShouldReturnPullRequest()
        {
            var builder = this.GetPullRequestBuilder();

            const string pullRequestBranch = "test_branch";
            const string mergeBranch = "master";
            const string firstModifiedFileName = "src/TestLibrary/TestClass.cs";
            const int pullRequestNumber = 1;
            const string renamedFileName = "src/TestLibrary/Nested/TestClass2.cs";
            const string addedFileName = "src/TestLibrary/Nested/TestClass3.cs";
            const string secondModifiedFileName = "src/TestLibrary/TestLibrary.csproj";
            const int renamedChanges = 9;
            const int addedChanges = 12;
            const int firstModifiedChanges = 8;
            const int secondModifiedChanges = 7;
            var renamedContentUrl = new Uri("http://321");
            var addedContentUrl = new Uri("http://421");
            var firstModifiedContentUrl = new Uri("http://521");
            var secondModifiedContentUrl = new Uri("http://621");
            var branches = new GitHubPullRequestBranches(pullRequestBranch, mergeBranch);

            var files = new List<PullRequestFile>
                            {
                                GetPullRequestFile(
                                    "321",
                                    renamedFileName,
                                    "renamed",
                                    renamedContentUrl,
                                    renamedChanges),
                                GetPullRequestFile(
                                    "421",
                                    addedFileName,
                                    "added",
                                    addedContentUrl,
                                    addedChanges),
                                GetPullRequestFile(
                                    "521",
                                    firstModifiedFileName,
                                    "modified",
                                    firstModifiedContentUrl,
                                    firstModifiedChanges),
                                GetPullRequestFile(
                                    "621",
                                    secondModifiedFileName,
                                    "modified",
                                    secondModifiedContentUrl,
                                    secondModifiedChanges)
                            };

            const string lastCommitId = "02ccfbd76f155afa574a4291b6c5c0602c384c77";

            var pullRequest = builder.Build(
                pullRequestNumber,
                lastCommitId,
                files,
                FileContents.FullDiff,
                branches);

            Assert.That(pullRequest, Is.Not.Null);
            Assert.That(pullRequest.Diff, Is.EqualTo(FileContents.FullDiff));
            Assert.That(pullRequest.LastCommitId, Is.EqualTo(lastCommitId));
            Assert.That(pullRequest.Number, Is.EqualTo(pullRequestNumber));
            Assert.That(pullRequest.Branches, Is.Not.Null);
            Assert.That(pullRequest.Branches.Branch, Is.EqualTo(pullRequestBranch));
            Assert.That(pullRequest.Branches.MergeBranch, Is.EqualTo(mergeBranch));
            Assert.That(pullRequest.Files, Is.Not.Null);
            Assert.That(pullRequest.Files.Count, Is.EqualTo(4));
            
            Assert.That(pullRequest.Files[0].FileName, Is.EqualTo(renamedFileName));
            Assert.That(pullRequest.Files[0].Status, Is.EqualTo(GitHubPullRequestFileStatus.Renamed));
            Assert.That(pullRequest.Files[0].Changes, Is.EqualTo(renamedChanges));
            Assert.That(pullRequest.Files[0].Diff, Is.EqualTo(FileContents.TestClass2CsDiff));

            Assert.That(pullRequest.Files[1].FileName, Is.EqualTo(addedFileName));
            Assert.That(pullRequest.Files[1].Status, Is.EqualTo(GitHubPullRequestFileStatus.Added));
            Assert.That(pullRequest.Files[1].Changes, Is.EqualTo(addedChanges));
            Assert.That(pullRequest.Files[1].Diff, Is.EqualTo(FileContents.TestClass3CsDiff));

            Assert.That(pullRequest.Files[2].FileName, Is.EqualTo(firstModifiedFileName));
            Assert.That(pullRequest.Files[2].Status, Is.EqualTo(GitHubPullRequestFileStatus.Modified));
            Assert.That(pullRequest.Files[2].Changes, Is.EqualTo(firstModifiedChanges));
            Assert.That(pullRequest.Files[2].Diff, Is.EqualTo(FileContents.TestClassCsDiff));

            Assert.That(pullRequest.Files[3].FileName, Is.EqualTo(secondModifiedFileName));
            Assert.That(pullRequest.Files[3].Status, Is.EqualTo(GitHubPullRequestFileStatus.Modified));
            Assert.That(pullRequest.Files[3].Changes, Is.EqualTo(secondModifiedChanges));
            Assert.That(pullRequest.Files[3].Diff, Is.EqualTo(FileContents.TestLibraryCsprojDiff));
        }

        private static PullRequestFile GetPullRequestFile(
            string sha,
            string fileName,
            string status,
            Uri contentUrl,
            int changes)
        {
            return new PullRequestFile(sha, fileName, status, 0, 0, changes, null, null, contentUrl, null);
        }

        private IPullRequestBuilder GetPullRequestBuilder()
        {
            var parser = new Mock<IDiffParser>();
            parser.Setup(diffParser => diffParser.Split(FileContents.FullDiff))
                .Returns(
                    new Dictionary<string, string>
                        {
                            { "src/TestLibrary/Nested/TestClass2.cs", FileContents.TestClass2CsDiff },
                            { "src/TestLibrary/Nested/TestClass3.cs", FileContents.TestClass3CsDiff },
                            { "src/TestLibrary/TestClass.cs", FileContents.TestClassCsDiff },
                            { "src/TestLibrary/TestLibrary.csproj", FileContents.TestLibraryCsprojDiff }
                        });
            return new PullRequestBuilder(parser.Object);
        }
    }
}
