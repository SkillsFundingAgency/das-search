Feature: ViewStandardDetails
	In order to chose a standard
	As an employer
	I want to be able to open standard details page.


@regression
Scenario Outline: Validate Standard details and bespoke content on standard detail page
Given I am on Search landing page
And I enter keyword '<JOBROLE>' in search box
And I click on search button
When I choose any of the standard from search result page
Then I see Standard title displayed
And I should see "Introductory text" on standard detail page
And I should see "Overview of Role" on standard detail page
And I should see "Typical length" on standard detail page
And I should see "What Apprentice will learn" on standard detail page
And I should see "Qualification" on standard detail page
And I should see "Professional registration" on standard detail page
Examples:
| JOBROLE                                     |
| Product Design and Development Engineer     |
| Digital & technology solutions professional |
| Actuary                                     |

@regression
Scenario Outline: Validate Standard detail page which has no bespoke contents
Given I am on Search landing page
And I enter keyword '<JOBROLE>' in search box
And I click on search button
When I choose any of the standard from search result page
Then I see Standard title displayed
And I see level is displayed.
Examples:
| JOBROLE                        |
| Dental Nurse                   |
| Property Maintenance Operative |

@ignore
@regression
Scenario: Validate typical length units is months only.
Given I am on standard search result page
When I click on the Standard title
Then is should see Standard page
And I should see typical length is displayed in months only.

@ignore
@regression
Scenario: Validate professional registration shown when it has data
Given I have choosen a standard which has no progressional registration data populated
When I click on the standard title
Then I should see standard detail page
And I should not see professional registration field on detail page.