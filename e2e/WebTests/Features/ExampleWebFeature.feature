@web
Feature: ExampleWebFeature
	In order to create a basic example of automating a web applications
	As a tester
	I want to be able to perform some automated tasks

@web
Scenario Outline: Login
	Given I have entered username '<username>' and password '<password>'
	When I login
	Then I should be informed that login '<expected_result>'

	Examples: 
	| testing                 | username | password             | expected_result |
	| valid combination       | tomsmith | SuperSecretPassword! | passed          |
	| invalid combination 1   | test     | test                 | failed          |
	| special characters      | $$$      | SuperSecretPassword! | failed          |
	| set to delberately fail | abc      | def                  | passed          |