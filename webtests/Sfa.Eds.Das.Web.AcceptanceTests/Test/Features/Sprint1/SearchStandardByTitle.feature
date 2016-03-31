Feature: Search standards by title
	In order to choose an apprenticeship
	As an employer 
	I want to be able to Search by title


@smoke
Scenario Outline:Search By title1
	Given I am on Search landing page
	And I enter keyword '<title>' in search box
	When I click on search button
	Then I should see matching '<title>' standards on result page
	Examples: 
	| title                  |
	| Actuarial Technician   |


@regression
Scenario Outline:Search By title
	Given I am on Search landing page
	And I enter keyword '<title>' in search box
	When I click on search button
	Then I should see matching '<expected>' standards on result page
	Examples: 
	| title                  | expected                                                         |
	| actuarial technician   | Actuarial Technician                                             |
	| financial adviser      | Financial Services Customer Adviser                                                 |
	| software engineer      | Aerospace Software Development Engineer                          |
	| car mechanic           | Motor Vehicle Service and Maintenance Technician [light vehicle] |
	| manufacturing engineer | Aerospace Engineer                                               |
	| manager                | Senior Housing Property Management                               |
	| legal services         | Legal Services: Criminal Prosecution                             |
	| designer               | Design: Design                                                   |
	| dental nurse           | Dental Nurse                                                     |
	| electrician            |Installation Electrician/Maintenance Electrician                                                |

@ignore
Scenario: Standards searched to display level informaiton
Given I am on Search landing page
And I enter keyword '{p0}' in search box
When I click on search button
Then I should see level information againts again standards found


