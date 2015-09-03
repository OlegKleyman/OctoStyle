﻿@octoStyle
Feature: OctoStyle
	As a user I want to run the OctoStyle
	application and have it comment on any style cop
	issues it finds on a pull request

@pullRequest
Scenario: Pull request with issues
	Given I have a pull request with stylistic problems	
	When I run the OctoStyle using the StyleCop engine
	Then there should be comments on the pull request on the lines of the found violations

@pullRequest
Scenario: Pull request with issues using code analysis
	Given I have a pull request with stylistic problems	
	When I run the OctoStyle using the CodeAnalysis engine
	Then there should be comments on the pull request on the lines of the found violations
