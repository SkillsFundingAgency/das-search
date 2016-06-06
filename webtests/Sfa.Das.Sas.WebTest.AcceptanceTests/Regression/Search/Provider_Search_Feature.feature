Feature: Provider_Search_Feature
	As an employer I want to be able to search for different Providers in different postcodes for different frameworks

	#Currently extending this section please ignore
	#@Regression		@PreProd @Prod
	Scenario Outline:Should find a list of providers for any postcode existing in the CD API for specific Frameworks
	
	Given I navigated to the Search page
	When I enter data
		| Field      | Value         |
		| Search Box | <search term> |
	And I choose Search Button
	Then I am on the Search Results page
	When I choose First Framework Result
	Then I am on the Framework Details page
	When I choose {linkName}
	Then I am on the Provider Search page
	When I enter data
		| Field      | Value	  |
		| Search Box | <Postcode> |
	And I choose Search Button
	Then I am on the Framework Provider Results page
	And I see Provider Results list Contains
		| Field | Rule   | Value   |
		| Title | Equals | <title> |

	Examples: 
		| Postcode | title                       | search term                               |
		| BS8 1EJ  | EXETER COLLEGE              | Hospitality                               |
		| TS17 6F  | Stockton Riverside College  | Electrotechnical: Electrical Installation |
		| EX1 3QS  | EDUCATION + TRAINING SKILLS | Business and Administration               |