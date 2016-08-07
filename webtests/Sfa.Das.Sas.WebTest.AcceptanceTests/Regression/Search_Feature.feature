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
	| Field         | Rule     | Value            |
	| error message | contains | You must enter a full and valid postcode |

