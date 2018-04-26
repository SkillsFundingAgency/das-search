Feature: Filtering
	As an Employer
	In order to narrow the list of Standards or Framework
	I need to be able to filter by Apprenticeship Level
	So I can see only relevant levels listed first

Scenario: Filter Apprenticeship Search Results Page Low to High 
	Given I navigated to the Search page
	When  I choose Search Button
	Then  I am on the Search Results page
	And all elements exists on page
	When  I choose Level 2 Checkbox
	And all results are level 2

Scenario: Filter Apprenticeship Search Results Page High to Low
	Given I navigated to the Search Results page
	When  I choose Level 7 Checkbox
	And all results are level 7