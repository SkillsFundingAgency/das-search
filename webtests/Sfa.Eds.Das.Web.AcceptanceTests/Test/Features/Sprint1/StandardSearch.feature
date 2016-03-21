Feature: Search for a Standard by keyword
	In order to find an apprenticeship as per my needs
	As an employer 
	I want to be able to search from available Standards


@smoke
Scenario: Verify landing page
When I am on Search landing page
Then I should be able to see home page with title as "Home Page - Employer Apprenticeship Search"


@regression
Scenario Outline: Search Standard by keyword
	Given I am on Search landing page
	And I enter keyword '<JOBROLE>' in search box
	When I click on search button
	Then I should see matching '<JOBROLE>' standards on result page
Examples:
| JOBROLE                |
| Actuarial Technician   |
| Financial Adviser      |
| Software Engineer      |
| manufacturing engineer |
| Legal Services         |
| Designer               |
| dental nurse           |
| Electrician            |
| Car Mechanic           |
| manager                |
| Actuarial              |

@regression
Scenario Outline: Search Standard by keyword Actuarial
	Given I am on Search landing page
	And I enter keyword '<Keyword>' in search box
	When I click on search button
	Then I should see matching Standard 'Actuarial Technician' standards on result page
Examples:
| Keyword       |
| Actuarial     |
| Actuary       |
| mathematician |

@regression
Scenario Outline: Search Standard by keyword Aeorospace
	Given I am on Search landing page
	And I enter keyword '<Keyword>' in search box
	When I click on search button
	Then I should see matching Standard 'Aerospace Manufacturing Fitter' standards on result page
	And I should see matching Standard 'Aerospace Engineer' standards on result page
	And I should see matching Standard 'Aerospace Software Development Engineer' standards on result page	
Examples:
| Keyword                        |
| aircraft mechanic              |
| Aerospace Manufacturing Fitter |
| aircraft manufacture mechanic  |


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






