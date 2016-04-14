Feature: SearchProvider-end to end test
	In order to choose from provider list
	As an employer
	I want to be able to search all providers for a given standard

@smoke
Scenario Outline:Show available providers for given standard -End to End Test
	Given I'm on Search landing page
	And I enter keyword '<JOBROLE>' in search box
	And I click on search button
	When I pick any of the standard from search result page
	And I enter '<Postcode>' in provider search box
    And I search Search for provider
	Then I should see all providers in result page
	#And I should see all providers listed in Alphabetical order 
Examples:
| JOBROLE                                     | Postcode |
| Product Design and Development Engineer     | CV1 2wt  |
| Digital & technology solutions professional | CV6 1PT  |


