Feature: Search Provider by Postcode
	In order to choose from a provider list
	As an employer
	I want to be able to search provider in my area by entering postcode
@ignore
Scenario: Validate post code field mandatory
Given I am on Standard detail page
When I keep the post code field empty
And I click on search provider button
Then I should see error message "Please enter a valid postcode"

@ignore
Scenario Outline:Validate post code search inside provider radius.
Given I am on Standard detail page
When I enter post code '<postcode>' which is inside provider radius
And I click on search provider button
Then I should list of providers on result page who operates within entered postcode.
Examples:
|postcode|
|CV1 2wt |
|CV6 1PT|
@ignore
Scenario: Validate partial post code search inside provider radius
Given I am on Standard detail page
When I enter post code '<postcode>' which is inside provider radius
And I click on search provider button
Then I should list of providers on result page who operates within entered postcode.
Examples:
|postcode|
|CV1 2wt |
|CV6 1PT|

Scenario Outline: Search ASPIRE ACHIEVE ADVANCE LIMITED Provider by postcode falling on Provider radius
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "aspire archive advance limited" in provider results page.
Examples:
| Postcode| id |
| LN76HN  | 25 |



Scenario Outline: Search Provider by postcode outside  Provider radius 60 miles
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should not see provider "aspire archive advance limited" in provider results page.
And I should see provider "millbrook management services limited" in provider results page.
Examples:
| Postcode | id |
| LN76HJ   | 25 |


Scenario Outline:Search Provider by postcode falling inside Provider radius
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "aspire archive advance limited" in provider results page.
And I should see provider "millbrook management services limited" in provider results page.
Examples:
| Postcode| id |
| DN209NH | 25 |

Scenario Outline:Search Provider by postcode falling inside more than one provider radius.
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "skills team ltd" in provider results page.
And I should see provider "aspire achieve advance limited" in provider results page.
Examples:
| Postcode| id |
| NW66AY | 25 |