Feature: Provider_Search_Feature
	As an employer I want to be able to search for different Providers in different postcodes for different frameworks

	#Currently extending this section please ignore
	#@Regression @PreProd @Prod
	Scenario Outline:Should find specific providers and frameworks in live data
	Given I navigated to the Search page
	When I enter data
		| Field      | Value         |
		| Search Box | <search term> |
	And  I choose Search Button
	Then I am on the Search Results page	
	And I see Apprenticeship Results list Contains	
		| Field       | Rule   | Value         |
		| search term | Contains | <search term> |
	When I choose First Framework Result
	Then I am on the Framework Details page
	When I choose Search Page Button
	Then I am on the Provider Search page
	When I enter data
		| Field      | Value	  |
		| Postcode Search Box | <Postcode> |
	And I choose Search Button
	Then I am on the Framework Provider Results page
	And I see Provider Results list Contains
		| Field | Rule   | Value   |
		| Title | Contains | <title> |

	Examples: 
		| Postcode | title                                  | search term                                 |
		| BS8 1EJ  | EXETER COLLEGE                         | Hospitality                                 |
		| TS17 6F  | Stockton Riverside College             | Electrotechnical: Electrical Installation   |
		| EX1 3QS  | EDUCATION + TRAINING SKILLS            | Business and Administration                 |
		

#@Regression @PreProd @Prod
Scenario Outline:Should find specific providers and standards in live data
	
	Given I navigated to the Search page
	When I enter data
		| Field      | Value         |
		| Search Box | <search term> |
	And  I choose Search Button
	Then I am on the Search Results page	
	And I see Search Results list Contains
	#Is the field name correct? 
		| Field       | Rule   | Value         |
		| search term | Equals | <search term> |
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Search Page Button
	Then I am on the Provider Search page
	When I enter data
		| Field      | Value	  |
		| Search Box | <Postcode> |
	And I choose Search Button
	Then I am on the Standard Provider Results page
	And I see Provider Results list Contains
		| Field | Rule   | Value   |
		| Title | Equals | <title> |

	Examples: 
		| Postcode | title                                  | search term                                 |
		| NE1 8ST  | UNIVERSITY OF NORTHUMBRIA AT NEWCASTLE | Digital & technology solutions professional |