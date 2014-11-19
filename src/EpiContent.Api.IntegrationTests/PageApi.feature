Feature: PageApi

Scenario: Should see the EPiServer login page
	Given I navigated to the login page
	Then I see
	| Field    | Rule   | Value |
	| UserName | Exists | true  |
	#Given There is a page using the StartPage template

Scenario: Should create a page by the template
	Given I create a Start page
	When I navigate to the Start page

