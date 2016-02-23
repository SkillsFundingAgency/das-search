Feature: SearchProvider-end to end test
	In order to choose from provider list
	As an employer
	I want to be able to search all providers for a given standard

@regression
Scenario Outline:Show available providers for given standard -End to End Test
	Given I am on Search landing page
	And I enter keyword '<JOBROLE>' in search box
	And I click on search button
	When I choose any of the standard from search result page
	And I click on search under provider search section
	Then I should all providers in result page
	And I should see all providers listed in Alphabetical order 
Examples:
| JOBROLE                                     |
| Product Design and Development Engineer     |
| Digital & technology solutions professional |


