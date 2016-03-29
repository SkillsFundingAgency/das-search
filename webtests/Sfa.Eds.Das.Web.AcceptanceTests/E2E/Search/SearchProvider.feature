Feature: Search Provider
	In order to choose from provider list
	As an employer
	I want to be able to search all providers for a given standard

@e2e
Scenario Outline:Show available providers for given standard
	Given I am on Search landing page
	When I enter keyword '<JOBROLE>' in search box
	And I click on search button
	Then I am on the Search results page

	When I choose any of the standard from search result page
	Then I am on a Standard details page

	When I enter '<Postcode>' in provider search box
    And I search Search for provider
	Then I am on the provider results page 
	And I should see all providers in result page
Examples:
| JOBROLE                                     | Postcode |
| Digital & technology solutions professional | CV6 1PT  |


