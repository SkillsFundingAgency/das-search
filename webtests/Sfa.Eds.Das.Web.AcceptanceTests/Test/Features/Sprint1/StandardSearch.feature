Feature: Search for a Standard by keyword
	In order to find an apprenticeship as per my needs
	As an employer 
	I want to be able to search from available Standards


@smoke
Scenario: Verify landing page
When I am on Search landing page
Then I should be able to see home page with title as "Home Page - Employer Apprenticeship Search"



Scenario Outline: Search Standard by keyword
	Given I am on Search landing page
	And I enter keyword '<JOBROLE>' in search box
	When I click on search button
	Then I should see matching '<JOBROLE>' standards on result page
Examples:
| JOBROLE                |
| Actuarial technician   |
| Financial Adviser      |
| software engineer      |
| manufacturing engineer |
| Legal Services         |
| Designer               |
| dental nurse           |
| Electrician            |
| car mechanic           |
| Manager                |

@ignore
Scenario Outline: Verify invalid search
Given I am on Search landing page
And I enter keyword '<JOBROLE>' in search box
When I click on search button 
Then I should see message "Total results found: 0"
Examples: 
| JOBROLE     |
| kdjfdkfjdfk |
| 1232322     |


@ignore
Scenario Outline: Validate result count
	Given I am on Search landing page
	And I enter keyword '<JOBROLE>' in search box
	When I click on search button
	Then I should see standards count on result page
Examples:
| JOBROLE              |
| Actuarial Technician | 
| Financial Adviser    | 
| software engineer    | 


@ignore
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













