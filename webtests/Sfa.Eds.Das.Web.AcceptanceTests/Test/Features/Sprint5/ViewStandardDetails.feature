Feature: ViewStandardDetails
	In order to chose a standard
	As an employer
	I want to be able to open standard details page.

@Feature-Standard_details
@regression
Scenario Outline: Validate Standard details and bespoke content on standard detail page
Given I am on standard search result page
When I click on any standard link
Then I should be able to navigate to Standard detail page 
And I should see following '<Bespoke>' content
Examples:
| Bespoke                     |
| Introductory text           |
| Typical length              |
| Entry requirements          |
| What apprentices will learn |
| Qualifications              |
| Professional registration   |

@Feature-Standard_details
@regression
Scenario: Validate Standard detail page which has no bespoke contents
Given I am on standard search result page
When I click the standard title which has no bespoke content. 
Then I should see standard detail 
And I see only Standard title displayed
And I see level is displayed.

@Feature-Standard_details
@regression
Scenario: Validate typical length units is months only.
Given I am on standard search result page
When I click on the Standard title
Then is should see Standard page
And I should see typical length is displayed in months only.

@Feature-Standard_details
@regression
Scenario: Validate professional registration shown when it has data
Given I have choosen a standard which has no progressional registration data populated
When I click on the standard title
Then I should see standard detail page
And I should not see professional registration field on detail page.