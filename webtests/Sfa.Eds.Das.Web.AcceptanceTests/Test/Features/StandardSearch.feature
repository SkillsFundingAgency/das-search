Feature: Standards Search
	In order to find an apprenticeship as per my needs
	As an employer 
	I want to be able to search from available Standards

@Smoke
Scenario Outline: Search by keyword
	Given I am on Search landing page
	And I enter keyword '<JOBROLE>' in search box
	When I click on search button
	Then I should see matching '<JOBROLE>' standards on result page
Examples:
| JOBROLE              |
| Mechtronics Engineer |
| Software Engineer    |
| Mechanical Engineer  |
| Health carer         |
| Charted Accountant   |
| Accountancy          |

@smoke
Scenario Outline: Search result Page to have best match 
Given I am on Search landing page
When I enter keyword '<keyword>'
And I click on search button
Then I should result search page
And it should list most best match '<keyword>' on top of the search list
Examples:
| keyword             |
| Mechanical Engineer |
| Software Engineer   |

@smoke


Scenario: Search result to have title matched standards on top list
Given I am on Search landing page
When 
Then 











