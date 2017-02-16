Feature: Search_Feature
	As an employer I want to be able to use the service
	To search for different Standards
	To search for different Frameworks
	To search for different Providers in different postcodes 	
	
	

#Test that Search Functionality still works
@Regression
Scenario Outline:Should find a apprenticeship by the job role
	
	Given I navigated to the Search page
	When I enter data
		| Field      | Value		 |
		| Search Box | <search term> |
	And I choose Search Button	
	Then I am on the Search Results page
	And I see Apprenticeship Results list Contains
	| Field | Rule   | Value   |
	| Title | Equals | <title> |

Examples: 
	| search term                    | title                                 |
	| railway                        | Railway engineering design technician |
	| Junior management consultant   | Junior management consultant          |
	| Relationship manager (banking) | Relationship manager (banking)        |

@Regression
#This scenario checks that an error message is displayed for and invalid postcode
Scenario:Should display error for invalid postcode
	Given I navigated to the Search page
	When I enter data
         | Field | Value |
         |    Search Box   |   Digital    |
	And I choose Search Button
	Then I am on the Search Results page
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Search Page Button
	Then I am on the Standard Provider Search page
	When I enter data
		| Field      | Value     |
		| Postcode Search Box | X123 |
	And I choose Search Button
	When I wait for the view to become active	
	Then I see
	| Field                                 | Rule     | Value                                    |
	| invalid format PostCode error message | contains | You must enter a full and valid postcode |





	#This scenario checks that an error message is displayed for and invalid postcode
Scenario:Should display error for a full valid postcode
	Given I navigated to the Search page
	When I enter data
         | Field | Value |
         |    Search Box   |   Digital    |
	And I choose Search Button
	Then I am on the Search Results page
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Search Page Button
	Then I am on the Standard Provider Search page
	When I enter data
		| Field               | Value   |
		| Postcode Search Box | XB1 1ZZ |
	And I choose Search Button
	When I wait for the view to become active	
	Then I see
	| Field                          | Rule     | Value                                    |
	| invalid postcode error message | contains | You must enter a full and valid postcode |




Scenario:Should display error for Wales postcode
	Given I navigated to the Search page
	When I enter data
         | Field | Value |
         |    Search Box   |   Digital    |
	And I choose Search Button
	Then I am on the Search Results page
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Search Page Button
	Then I am on the Standard Provider Search page
	When I enter data
		| Field               | Value    |
		| Postcode Search Box | CF11 8AZ |
	And I choose Search Button
	When I wait for the view to become active	
	Then I see
	| Field                                           | Rule     | Value                                      |
	| postcode not in england error message           | contains | The postcode entered is not in England.    |
	| wales information about apprenticeships Message | contains | Information about apprenticeships in Wales |




Scenario:Should display error for Scotland postcode
	Given I navigated to the Search page
	When I enter data
         | Field | Value |
         |    Search Box   |   Digital    |
	And I choose Search Button
	Then I am on the Search Results page
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Search Page Button
	Then I am on the Standard Provider Search page
	When I enter data
		| Field               | Value    |
		| Postcode Search Box | G46 6JJ |
	And I choose Search Button
	When I wait for the view to become active	
	Then I see
	| Field                                              | Rule     | Value                                         |
	| postcode not in england error message              | contains | The postcode entered is not in England.       |
	| Scotland Information About Apprenticeships Message | contains | Information about apprenticeships in Scotland |



		Scenario:Should display error for NorthernIreland postcode
	Given I navigated to the Search page
	When I enter data
         | Field | Value |
         |    Search Box   |   Digital    |
	And I choose Search Button
	Then I am on the Search Results page
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Search Page Button
	Then I am on the Standard Provider Search page
	When I enter data
		| Field               | Value    |
		| Postcode Search Box | BT27 5LG |
	And I choose Search Button
	When I wait for the view to become active	
	Then I see
	| Field                                                      | Rule     | Value                                         |
	| postcode not in england error message                      | contains | The postcode entered is not in England.       |
	| Northern Ireland Information About Apprenticeships Message | contains | Information about apprenticeships in Northern Ireland |