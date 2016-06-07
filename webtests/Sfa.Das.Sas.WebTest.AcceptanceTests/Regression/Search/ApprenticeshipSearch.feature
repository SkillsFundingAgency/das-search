Feature: Apprenticeship Search
	As an employer
	I want to be able to search for training options
	and find a provider for a given framework

@Regression		@PreProd @Prod
Scenario Outline:Should find a apprenticeship by the job role
	
	Given I navigated to the Search page

	When I enter data
		| Field      | Value     |
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

	