Feature: Framework User Journey
	As an employer
	I want to be able to search for training options
	and find a provider for a given framework

@E2E		@CI @Demo @SystemTest @PreProd @Prod
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