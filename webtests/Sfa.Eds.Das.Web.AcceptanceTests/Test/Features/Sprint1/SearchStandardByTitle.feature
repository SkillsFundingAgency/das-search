Feature: Search standards by title
	In order to choose an apprenticeship
	As an employer 
	I want to be able to Search by title


Scenario Outline:Search By title
	Given I am on Search landing page
	And I enter keyword '<title>' in search box
	When I click on search button
	Then I should see matching '<title>' standards on result page
	Examples: 
	| title                  |
	| Actuarial Technician   |
	| Financial Adviser      |
	| software engineer      |
	| car mechanic           |
	| manufacturing engineer |
	| Manager                |
	| Legal Services         |
	| Designer               |
	| Dental nurse           |
	| Electrician            |

Scenario: Standards searched to display level informaiton
Given I am on Search landing page
And I enter keyword '{p0}' in search box
When I click on search button
Then I should see level information againts again standards found


