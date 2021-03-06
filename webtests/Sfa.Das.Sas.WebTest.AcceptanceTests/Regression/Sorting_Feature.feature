﻿Feature: Sorting
	As an Employer
	In order to narrow the list of Standards or Framework
	I need to be able to sort results by high to low
	I need to be able to sort results by low to high
	So I can see only relevant levels listed first
#WIP
#Tests Sorting  works
@Regression
Scenario: Sort Apprenticeship Search Results Page 
	Given I navigated to the Search page
	When  I choose Search Button
	Then  I am on the Search Results page
	When  I wait for the view to become active
	Then I see
	| Field                 | Rule   | Value |
	| First Standard Result | Exists | True  |
	| Sorting Dropdown      | Exists | True  |
	
	When  I choose High To Low Option Selector	
	And   I choose Update Results Button	
	And  I wait for the view to become active
	Then  I am on the Search Results page
	When  I choose Low to High Option Selector
	And  I wait for the view to become active	
	When  I choose Update Results Button	
	Then  I am on the Search Results page
	
