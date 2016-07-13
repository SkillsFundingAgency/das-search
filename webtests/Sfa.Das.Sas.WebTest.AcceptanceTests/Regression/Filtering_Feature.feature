Feature: Filtering
	As an Employer
	In order to narrow the list of Standards or Framework
	I need to be able to filter by Apprenticeship Level
	So I can see only relevant levels listed first

#Tests filtering by level works
@Regression
Scenario: Filter Apprenticeship Search Results Page Low to High 
	Given I navigated to the Search page
	When  I choose Search Button
	Then  I am on the Search Results page
	When  I wait for the view to become active	
	Then I see
	| Field                 | Rule   | Value |
	| First Standard Result | Exists | True  |
	| Sorting Dropdown      | Exists | True  |
	| Filter Block          | Exists | True  |
	
	When  I choose Level 2 Checkbox
	And   I choose Update Results Button
	And  I wait for the view to become active
	Then  I see
	| Field               | Rule     | Value                                     |
	| Level Of Top Result | contains | 2 (equivalent to GCSEs at grades A* to C) |
@Regression 
Scenario: Filter Apprenticeship Search Results Page High to Low
	Given I navigated to the Search Results page
	When  I choose Level 7 Checkbox
	And   I choose Update Results Button
	When  I wait for the view to become active
	Then  I see
	| Field               | Rule     | Value                             |
	| Level Of Top Result | contains | 7 (equivalent to master’s degree) |
