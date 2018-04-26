Feature: SearchTrainingCourse
	As a user
	I want to be able to find and read about apprenticeshi trainings

Scenario: Find course and serach for providers
	Given I navigated to the Search page
	When  I search for 'account'
	And I chooes the first result
	Then I see the apprenticeship page with heading 'Accounting'
	And I see the apprenticeship page with level '3 (equivalent to A levels at grades A to E)'
	And I see the apprenticeship page with Typical length '18 months'
	Then I click on Find training providers
	And I am on the Find a training provider page
	And I add postcode 'CV1 5FB'
	And I click the search button
	Then I am on the Provider Location Search Result Page