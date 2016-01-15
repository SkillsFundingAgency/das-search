Feature: Standards Search
	In order to find an apprenticeship as per my needs
	As an employer 
	I want to be able to search from available Standards


@Smoke
Scenario: Verify landing page
When I am on Search landing page
Then I should be able to see home page with title as "Home Page - Employer Apprenticeship Search"



@web
Scenario Outline: Search by keyword
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

@web
Scenario: Verify invalid search
Given I am on Search landing page
And I enter keyword '<JOBROLE>' in search box
When I click on search button 
Then I should see message "Total Results found:0"

@web
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


@web
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













