// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Not for this solution")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "OctoStyle.Core.Borrowed", Justification = "Borrowed code should be segregated")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "OctoStyle.Core.Borrowed.Diff.#CreateDiff(System.String[],System.String[])", Justification = "Borrowed class")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Scope = "member", Target = "OctoStyle.Core.Borrowed.Diff.#CreateDiff(System.String[],System.String[])", Justification = "Borrowed class")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DiffRetriever", Scope = "member", Target = "OctoStyle.Core.PullRequestCommenterFactory.#GetCommenter(OctoStyle.Core.GitHubPullRequestFileStatus)", Justification = "DiffRetriever is a property name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "OctoStyle.Core.IGitHubDiffRetriever.#RetrieveAsync(System.String,System.String,System.String)", Justification = "Task<> is acceptable")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "OctoStyle.Core.PullRequestCommenter.#Create(OctoStyle.Core.GitHubPullRequestFile,OctoStyle.Core.ICodeAnalyzer,System.String)", Justification = "Task<> is acceptable")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Scope = "member", Target = "OctoStyle.Console.Arguments.#Login", Justification = "Login is the correct term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Scope = "member", Target = "OctoStyle.Console.Tests.Integration.FeatureContextExtended.#GitLogin", Justification = "Login is the correct term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "OctoStyle.Console.Tests.Integration.FeatureSteps.OctoStyleSteps.#ThenThereShouldBeCommentsOnThePullRequestOnTheLinesOfTheFoundViolations()", Justification = "Complexity is acceptable for the test as it's doing assertions on several lists via LINQ.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "OctoStyle.Core.Tests.Integration.FileSystemManagerTests.#GetFilesShouldReturnFilesInDirectoryCases", Justification = "Used via reflection with NUnit")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Scope = "member", Target = "OctoStyle.Core.Tests.Integration.FileSystemManagerTests.#GetFilesShouldReturnFilesInDirectory(System.String,System.String,System.String[])", Justification =  "Test case arguments don't need validation.")]
