Feature: Search Provider by Postcode
	In order to choose from a provider list
	As an employer
	I want to be able to search provider in my area by entering postcode

Scenario: Validate post code field mandatory
Given I am on Standard detail page
When I keep the post code field empty
And I click on search provider button
Then I should see error message "Please enter a valid postcode"


Scenario Outline:Validate post code search inside provider radius.
Given I am on Standard detail page
When I enter post code '<postcode>' which is inside provider radius
And I click on search provider button
Then I should list of providers on result page who operates within entered postcode.
Examples:
|postcode|
|CV1 2wt |
|CV6 1PT|

Scenario: Validate partial post code search inside provider radius
Given I am on Standard detail page
When I enter post code '<postcode>' which is inside provider radius
And I click on search provider button
Then I should list of providers on result page who operates within entered postcode.
Examples:
|postcode|
|CV1 2wt |
|CV6 1PT|
