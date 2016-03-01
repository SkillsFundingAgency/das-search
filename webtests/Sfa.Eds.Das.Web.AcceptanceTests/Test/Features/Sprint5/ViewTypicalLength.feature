Feature: ViewTypicalLength
	In order to choose a standard
	As an employer 
	I want to be able to see typical lengh againts each standard

@regression
Scenario Outline: View typical lengh info on search result page.
    Given I am on Search landing page
	And I enter keyword '<title>' in search box
	When I click on search button
	Then I should see matching '<title>' standards on result page
	And I should see typical length on result page.
	Examples: 
	| title                |
	| actuarial technician |
	
