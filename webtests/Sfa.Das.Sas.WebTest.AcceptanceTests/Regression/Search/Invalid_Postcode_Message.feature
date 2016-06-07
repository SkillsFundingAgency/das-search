Feature: Invalid Postcode Error Message
	As a user
	When I enter an invalid postcode
	I expect a warning message

#@Regression @PreProd @Prod
#This scenario checks error message displayed for invalid postcode
Scenario:Should display error for invalid postcode
	Given I navigated to the Search page
	When I enter data
         | Field | Value |
         |    Search Box   |   Business    |
	And I choose Search Button
	Then I am on the Search Results page
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Search Page Button
	Then I am on the Provider Search page
	When I enter data
		| Field      | Value     |
		| Postcode Search Box | X123 |
	And I choose Search Button
	When I wait for the view to become active	
	Then I see
	| Field         | Rule     | Value            |
	| error message | contains | Invalid postcode |

#Next Scenraio Checks core functionality is not broken
	#@Regression @PreProd @Prod
Scenario:Should not display error for valid postcode
	Given I navigated to the Search page
	When I enter data
         | Field | Value |
         |    Search Box   |   Digital & technology solutions professional    |
	And I choose Search Button
	Then I am on the Search Results page
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Search Page Button
	Then I am on the Provider Search page
	When I enter data
		| Field      | Value   |
		| Postcode Search Box | NE1 8ST |
	And I choose Search Button
	Then I am on the Standard Provider Results page
	When I choose First Provider Link
	Then I am on the Provider Details page
	