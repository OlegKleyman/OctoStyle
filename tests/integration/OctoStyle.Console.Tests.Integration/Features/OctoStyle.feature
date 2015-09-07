@octoStyle
Feature: OctoStyle
	As a user I want to run the OctoStyle
	application and have it comment on any style cop
	issues it finds on a pull request

@pullRequest
Scenario: Pull request with issues
	Given I have a pull request with stylistic problems	
	When I run the OctoStyle using the StyleCop engine
	Then there should be comments on the pull request on the lines of the found violations
	| File                                 | Position | Message                                                                                                          |
	| src/TestLibrary/TestClass.cs         | 5        | SA1600 - The method must have a documentation header.                                                            |
	| src/TestLibrary/TestClass.cs         | 9        | SA1513 - Statements or elements wrapped in curly brackets must be followed by a blank line.                      |
	| src/TestLibrary/Nested/TestClass2.cs | 1        | Renamed files not supported.                                                                                     |
	| src/TestLibrary/Nested/TestClass3.cs | 1        | SA1633 - The file has no header, the header Xml is invalid, or the header is not located at the top of the file. |
	| src/TestLibrary/Nested/TestClass3.cs | 1        | SA1200 - All using directives must be placed inside of the namespace.                                            |
	| src/TestLibrary/Nested/TestClass3.cs | 2        | SA1200 - All using directives must be placed inside of the namespace.                                            |
	| src/TestLibrary/Nested/TestClass3.cs | 3        | SA1200 - All using directives must be placed inside of the namespace.                                            |
	| src/TestLibrary/Nested/TestClass3.cs | 4        | SA1200 - All using directives must be placed inside of the namespace.                                            |
	| src/TestLibrary/Nested/TestClass3.cs | 5        | SA1200 - All using directives must be placed inside of the namespace.                                            |
	| src/TestLibrary/Nested/TestClass3.cs | 9        | SA1600 - The class must have a documentation header.                                                             |
	| src/TestLibrary/Nested/TestClass3.cs | 9        | SA1400 - The class must have an access modifier.                                                                 |

@pullRequest
Scenario: Pull request with issues using code analysis
	Given I have a pull request with stylistic problems	
	When I run the OctoStyle using the Roslyn engine
	Then there should be comments on the pull request on the lines of the found violations
	| File                                 | Position | Message                                                                                                         |
	| src/TestLibrary/TestClass.cs         | 5        | SA1600 - A C# code element is missing a documentation header.                                                   |
	| src/TestLibrary/TestClass.cs         | 10       | SA1513 - A closing curly bracket within a C# element, statement, or expression is not followed by a blank line. |
	| src/TestLibrary/Nested/TestClass2.cs | 1        | Renamed files not supported.                                                                                    |
	| src/TestLibrary/Nested/TestClass3.cs | 1        | SA1633 - A C# code file is missing a standard file header.                                                      |
	| src/TestLibrary/Nested/TestClass3.cs | 1        | SA1200 - A C# using directive is placed outside of a namespace element.                                         |
	| src/TestLibrary/Nested/TestClass3.cs | 2        | SA1200 - A C# using directive is placed outside of a namespace element.                                         |
	| src/TestLibrary/Nested/TestClass3.cs | 3        | SA1200 - A C# using directive is placed outside of a namespace element.                                         |
	| src/TestLibrary/Nested/TestClass3.cs | 4        | SA1200 - A C# using directive is placed outside of a namespace element.                                         |
	| src/TestLibrary/Nested/TestClass3.cs | 5        | SA1200 - A C# using directive is placed outside of a namespace element.                                         |
	| src/TestLibrary/Nested/TestClass3.cs | 9        | SA1600 - A C# code element is missing a documentation header.                                                   |
	| src/TestLibrary/Nested/TestClass3.cs | 9        | SA1400 - The access modifier for a C# element has not been explicitly defined.                                  |