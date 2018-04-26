Feature: SearchFeature
	As a user
	I want to be able to find and read about apprenticeshi trainings

Scenario: Sort Apprenticeship Search Results Page 
	Given I navigated to the Search page
	When  I search for 'account'
	And I chooes the first result
	Then I see the apprenticeship page with heading 'Accounting'