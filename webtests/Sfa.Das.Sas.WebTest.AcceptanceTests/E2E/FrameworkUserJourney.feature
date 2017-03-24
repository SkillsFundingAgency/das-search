Feature: Framework User Journey
	As an employer
	I want to be able to search for training options
	and find a provider for a given framework

@E2E	@Prod
Scenario:Should find a framework and provider
Given I navigated to the Start page
	Given I have data in the config
		| Token    | Key                     |
		| JOBROLE  | data.framework.JOBROLE  |
		| Postcode | data.framework.Postcode |

	And I navigated to the Start page
	When I choose Start Button
	Then I am on the Search page

	When I enter data
		| Field      | Value     |
		| Search Box | {JOBROLE} |
	And I choose Search Button
	Then I am on the Search Results page

	When I choose First Framework Result
	Then I am on the Framework Details page
	When I choose Search Page Button
	Then I am on the Framework Provider Search page

	When I enter data
		| Field               | Value      |
		| Postcode Search Box | {Postcode} |
	When I choose Search Button
	Then I am on the Framework Provider Results page
	And I see Provider Results list contains at least 1 items

	When I choose First Provider Link
	Then I am on the Provider Details page
	And I see
		| Field         | Rule   | Value |
		| Provider Name | Exists | true  |

@E2E	@CI @Demo @SystemTest @PreProd 
Scenario:Should find a framework and provider for a levy paying employer
	Given I navigated to the Start page
	Given I have data in the config
		| Token    | Key                       |
		| JOBROLE  | levy.framework.searchterm |
		| Postcode | levy.framework.Postcode   |

	And I navigated to the Start page
	When I choose Start Button
	Then I am on the Search page

	When I enter data
		| Field      | Value     |
		| Search Box | {JOBROLE} |
	And I choose Search Button
	Then I am on the Search Results page

	When I choose First Framework Result
	Then I am on the Framework Details page
		And I see
		| Field        | Rule   | Value                                    |
		| Summary Text | Equals | Summary of this apprenticeship framework |
	When I choose Search Page Button
	Then I am on the Framework Provider Search page

	When I enter data
		| Field               | Value      |
		| Postcode Search Box | {Postcode} |
	When I choose Yes Im levy paying employer	
	When I choose Search Button
	Then I am on the Framework Provider Results page
	And I see Provider Results list contains at least 1 items

	When I choose First Provider Link
	Then I am on the Provider Details page
	And I see
		| Field         | Rule   | Value |
		| Provider Name | Exists | true  |

@E2E		@CI @Demo @SystemTest @PreProd 
Scenario:Should find a framework and provider for a non levy paying employer
	Given I navigated to the Start page
	Given I have data in the config
		| Token    | Key                           |
		| JOBROLE  | non-levy.framework.searchterm |
		| Postcode | non-levy.framework.Postcode   |

	And I navigated to the Start page
	When I choose Start Button
	Then I am on the Search page

	When I enter data
		| Field      | Value     |
		| Search Box | {JOBROLE} |
	And I choose Search Button
	Then I am on the Search Results page

	When I choose First Framework Result
	Then I am on the Framework Details page
	When I choose Search Page Button
	Then I am on the Framework Provider Search page

	When I enter data
		| Field               | Value      |
		| Postcode Search Box | {Postcode} |
	When I choose No Im not Levy Paying Employer	
	When I choose Search Button
	Then I am on the Framework Provider Results page
	And I see Provider Results list contains at least 1 items

	When I choose First Provider Link
	Then I am on the Provider Details page
	And I see
		| Field         | Rule   | Value |
		| Provider Name | Exists | true  |

@ignore
Scenario Outline: Find a Provider and Framework Apprenticeship For an Employer - PreProd
Given I navigated to the Start page
When I want to find Provider for Business and Professional Administration framework near S60 1PQ
And I am <LevyOrNonLevy>
Then I can see Provider details 

Examples: 
| LevyOrNonLevy                  |
| Yes Im levy paying employer    |
| No Im not Levy Paying Employer |