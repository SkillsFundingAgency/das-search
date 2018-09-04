Feature: BackLinks
	As an employer
	I want to be able to use back links
	So that I can return to the previous page

Jira: https://skillsfundingagency.atlassian.net/browse/CI-1027

Scenario: Apprenticeship search results
Given I'm on the Apprenticeship search results page
When I click on the back link
Then I am taken to the FAT search by keywords page


Scenario: Apprenticeship summary page
Given I have done an apprenticeship search and clicked on first result
And I'm on the Apprenticeship summary page
When I click on the back link
Then I am taken to the Apprenticeship search results page

Scenario: Provider search page from Apprenticeship Summary
Given that I'm on the Find a training provider search page
When I click on the back link
Then I am taken to the Apprenticeship summary page

Scenario: Provider summary page
Given I have done a postcode search
And that I'm on the Provider summary page
When I click on the back link
Then I am taken to the Postcode search results page

Scenario: Provider 'shop window' page
Given that Im on the Provider shop window page
When I click on the back link
Then I am taken to the Find a training provider by name page

Scenario: Provider search apprenticeship summary page
Given I have come from the Provider shop window page
And I'm on the Apprenticeship summary page
When I click on the back link
Then I am taken to the Provider shop window page

Scenario: Postcode search results page
Given that I'm on the Postcode search results page
When I click on the back link
Then I am taken to the Postcode search page

Scenario: Find training provider by name search page
Given that I'm on the Find a training provider by name page
When I click on the back link
Then I am taken to the provider search page

Scenario: Find apprenticeship search page
Given that I'm on the Find apprenticeship search page
When I click on the back link
Then I am taken to the search type selection page