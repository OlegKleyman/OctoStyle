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
