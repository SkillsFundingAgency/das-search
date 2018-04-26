Feature: SearchProviderLocation
	As a user
	I want to be able to find providers close to where I am

Scenario: Reading satisfaction and Achievement rates
	Given I have picked course '454-2-1' and search for postcode 'CV1+5FB'
	Then I am on the Provider Location Search Result Page
	And the satisfaction rates are populated
	And the achievement rates are populated
