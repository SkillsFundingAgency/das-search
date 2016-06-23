Feature: Search_Feature
	As an employer I want to be able to use the service
	To search for different Standards
	To search for different Frameworks
	To search for different Providers in different postcodes 	
	
	
#Currently extending this section please ignore
#Test that Search Functionality still works
#@Regression
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

	
#@Regression
#Test that specific providers exist in live service
	Scenario Outline:Specific providers and frameworks in live data
	Given I have data in the config
		| Token    | Key                    |
		| JOBROLE  | data.framework.JOBROLE  |
		| Postcode | data.framework.Postcode |
	And I navigated to the Search page
	When I enter data
		| Field      | Value         |
		| Search Box | {JOBROLE} |
	And  I choose Search Button
	Then I am on the Search Results page	
	When I choose First Framework Result
	Then I am on the Framework Details page
	When I wait for the view to become active
	Then I see 	
		| Field       | Rule   | Value         |
		| Framework Heading | Contains | <Keyword > |
	When I wait to see Shortlist Link
	Then I see 
	| Field          | Rule     | Value                    |
	| Shortlist Link | Contains | Shortlist apprenticeship |
	When I choose Search Page Button
	Then I am on the Provider Search page
	When I enter data
		| Field      | Value	  |
		| Postcode Search Box | {Postcode} |
	And I choose Search Button
	Then I am on the Framework Provider Results page
	And I see Provider Results list Contains
		| Field | Rule   | Value     |
		| Title | Contains | Newcastle City Learning |

	Examples: 
		| Keyword                                                                        |
		| Children and Young People's Workforce: Children and Young People's Social Care |				  

#@Regression
#This is a shorter test to find providers but just for standards
Scenario Outline:Specific providers and standards in live data
	
	Given I navigated to the Search page
	When I enter data
		| Field      | Value         |
		| Search Box | <Keyword>	 |
	And  I choose Search Button
	Then I am on the Search Results page	
	When I choose First Standard Result
	Then I am on the Standard Details page
	Then I see
         | Field				| Rule	     | Value                         |
         |     Shortlist Link   |   Equals   |   Shortlist apprenticeship    |
	When I choose Search Page Button
	Then I am on the Provider Search page
	When I enter data
		| Field				  | Value	   |
		| Postcode Search Box | <Postcode> |
	And I choose Search Button
	Then I am on the Standard Provider Results page
	And I see Provider Results list Contains
		| Field | Rule   | Value   |
		| Title | Equals | <title> |

	Examples: 
		| Postcode | title                                  | Keyword									  |
		| M15 6BH  | MANCHESTER METROPOLITAN UNIVERSITY		| Digital & technology solutions professional |

#@Regression
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
	Then I am on the Provider Search page
	When I enter data
		| Field      | Value     |
		| Postcode Search Box | X123 |
	And I choose Search Button
	When I wait for the view to become active	
	Then I see
	| Field         | Rule     | Value            |
	| error message | contains | Invalid postcode |

