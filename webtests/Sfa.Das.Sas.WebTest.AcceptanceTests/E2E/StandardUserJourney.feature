Feature: Standard User Journey
	As an employer
	I want to be able to search for training options
	and find a provider for a given standard



@E2E	 @Prod
Scenario:Should find a standard and provider
	Given I have data in the config
		| Token    | Key                    |
		| JOBROLE  | data.standard.JOBROLE  |
		| Postcode | data.standard.Postcode |

	And I navigated to the Start page
	When I choose Start Button
	Then I am on the Search page

	When I enter data
		| Field      | Value     |
		| Search Box | {JOBROLE} |
	And I choose Search Button
	Then I am on the Search Results page

	When I choose First Standard Result
	Then I am on the Standard Details page	
	When I choose Search Page Button
	Then I am on the Standard Provider Search page
	When I enter data
		| Field               | Value      |
		| Postcode Search Box | {Postcode} |
	And I choose Search Button
	Then I am on the Standard Provider Results page
	And I see Provider Results list contains at least 1 items
	When I choose First Provider Link
	Then I am on the Provider Details page
	And I see
		| Field         | Rule   | Value |
		| Provider Name | Exists | true  |

@E2E	 @CI @Demo @SystemTest @PreProd 
Scenario:Should find a standard and provider for a levy paying employer
	Given I have data in the config
		| Token    | Key                      |
		| JOBROLE  | levy.standard.searchterm |
		| Postcode | levy.standard.Postcode   |

	And I navigated to the Start page
	When I choose Start Button
	Then I am on the Search page

	When I enter data
		| Field      | Value     |
		| Search Box | {JOBROLE} |
	And I choose Search Button
	Then I am on the Search Results page

	When I choose First Standard Result
	Then I am on the Standard Details page
	And I see
		| Field        | Rule   | Value                                   |
		| Summary Text | Equals | Summary of this apprenticeship standard |
	When I choose Search Page Button
	Then I am on the Standard Provider Search page
	When I enter data
		| Field               | Value      |
		| Postcode Search Box | {Postcode} |
	When I choose Yes Im levy paying employer	
	And I choose Search Button
	Then I am on the Standard Provider Results page
	And I see Provider Results list contains at least 1 items
	When I choose First Provider Link
	Then I am on the Provider Details page
	And I see
		| Field         | Rule   | Value |
		| Provider Name | Exists | true  |

@E2E	 @CI @Demo @SystemTest @PreProd 
Scenario:Should find a standard and provider for a non levy paying employer
	Given I have data in the config
		| Token    | Key                          |
		| JOBROLE  | non-levy.standard.searchterm |
		| Postcode | non-levy.standard.Postcode   |

	And I navigated to the Start page
	When I choose Start Button
	Then I am on the Search page

	When I enter data
		| Field      | Value     |
		| Search Box | {JOBROLE} |
	And I choose Search Button
	Then I am on the Search Results page

	When I choose First Standard Result
	Then I am on the Standard Details page	
	When I choose Search Page Button
	Then I am on the Standard Provider Search page
	When I choose No Im not Levy Paying Employer
	When I enter data
		| Field               | Value      |
		| Postcode Search Box | {Postcode} |	
	And I choose Search Button
	Then I am on the Standard Provider Results page
	And I see Provider Results list contains at least 1 items
	When I choose First Provider Link
	Then I am on the Provider Details page
	And I see
		| Field         | Rule   | Value |
		| Provider Name | Exists | true  |

@ignore
Scenario Outline: Find a Provider and Standard Apprenticeship For an Employer - PreProd
Given I navigated to the Start page
When I want to find Provider for Business and Professional Administration framework near S60 1PQ
And I am <LevyOrNonLevy>
Then I can see Provider details 

Examples: 
| LevyOrNonLevy                  |
| Yes Im levy paying employer    |
| No Im not Levy Paying Employer |