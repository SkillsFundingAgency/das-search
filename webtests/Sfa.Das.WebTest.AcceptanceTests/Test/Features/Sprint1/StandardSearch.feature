Feature: Search for a Standard by keyword
	In order to find an apprenticeship as per my needs
	As an employer 
	I want to be able to search from available Standards


@smoke
Scenario: Verify landing page
When I'm on Search landing page
Then I should be able to see home page with title as "Home Page - Employer Apprenticeship Search"


@regression
Scenario Outline: Search Standard by keyword
	Given I'm on Search landing page
	And I enter keyword '<JOBROLE>' in search box
	When I click on search button
	Then I'm on the Search results page
	Then I should see matching '<ExpectedResult>' standards on result page
Examples:
| JOBROLE                | ExpectedResult                                                   |
| Actuarial Technician   | Actuarial Technician                                             |
| Financial Adviser      | Financial Services Customer Adviser                              |
| Software Engineer      | Aerospace Software Development Engineer                          |
| manufacturing engineer | Manufacturing Engineer                                           |
| Legal Services         | Legal Services: Debt Recovery and Insolvency                     |
| Designer               | Product Design and Development Engineer                          |
| dental nurse           | Dental Nurse                                                     |
| Electrician            | Installation Electrician/Maintenance Electrician                 |
| Car Mechanic           | Motor Vehicle Service and Maintenance Technician [light vehicle] |
| manager                | Express Logistics: Operational Manager                           |  
| Actuarial              | Actuarial Technician                                             |

@regression
Scenario Outline: Search Standard by keyword Actuarial
	Given I'm on Search landing page
	And I enter keyword '<Keyword>' in search box
	When I click on search button
	Then I'm on the Search results page
	Then I should see matching Standard 'Actuarial Technician' standards on result page
Examples:
| Keyword       |
| Actuarial     |
| Actuary       |
| mathematician |

@regression
Scenario Outline: Search Standard by keyword Aeorospace
	Given I'm on Search landing page
	And I enter keyword '<Keyword>' in search box
	When I click on search button
	Then I'm on the Search results page
	Then I should see matching Standard 'Aerospace Manufacturing Fitter' standards on result page
	And I should see matching Standard 'Aerospace Engineer' standards on result page
	And I should see matching Standard 'Aerospace Software Development Engineer' standards on result page	
Examples:
| Keyword                        |
| aircraft mechanic              |
| Aerospace Manufacturing Fitter |
| aircraft manufacture mechanic  |


@regression
Scenario Outline: Verify invalid search
Given I'm on Search landing page
And I enter keyword '<JOBROLE>' in search box
When I click on search button 
Then I'm on the Search results page
And I should see message "There are no apprenticeships matching your search for '<JOBROLE>'"
Examples: 
| JOBROLE     |
| kdjfdkfjdfk |
| 1232322     |

@ignore
Scenario Outline: Validate result count
	Given I'm on Search landing page
	And I enter keyword '<JOBROLE>' in search box
	When I click on search button
	Then I'm on the Search results page
	And I should see standards count on result page
Examples:
| JOBROLE              |
| Actuarial Technician | 
| Financial Adviser    | 
| software engineer    | 


@regression
Scenario Outline: Search result Page to have best match 
Given I'm on Search landing page
And I enter keyword '<JOBROLE>' in search box
And I click on search button
Then I'm on the Search results page
And I should see  best match '<JOBROLE>' is on top of the search list
Examples:
| JOBROLE             |
| Actuarial Technician |
| Software Developer   |







