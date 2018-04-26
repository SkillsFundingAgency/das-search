Feature: Sorting
	As an Employer
	In order to narrow the list of Standards or Framework
	I need to be able to sort results by high to low
	I need to be able to sort results by low to high
	So I can see only relevant levels listed first

Scenario: Sort Apprenticeship Search Results Page 
	Given I navigated to the Search page
	When  I search for 'account'
	Then  I am on the Search Results page
	When  I choose High to Low Option Selector
	Then  I am on the Search Results page
	When  I choose Low to High Option Selector
	Then  I am on the Search Results page