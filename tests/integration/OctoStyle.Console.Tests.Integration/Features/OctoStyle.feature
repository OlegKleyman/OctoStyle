@octoStyle
Feature: OctoStyle
	As a user I want to run the OctoStyle
	application and have it comment on any style cop
	issues it finds on a pull request

@ignore
@pullRequest
Scenario: Pull request with issues
	Given I have a pull request with stylistic problems	
	When I run the OctoStyle
	Then there should be comments on the pull request on the lines of the found violations
