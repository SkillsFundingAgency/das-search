Feature: Standard User Journey
	As an employer
	I want to be able to search for training options
	and find a provider for a given standard

@HappyPath @CI @SystemTest @Demo @PreProd @Prod
Scenario:Should find a standard and provider
	Given I have data for a standard
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

	When I choose Shortlist Link
	Then I see
         | Field          | Rule     | Value                 |
         | Shortlist Link | contains | Remove from shortlist |

	When I choose Search Page Button

	Then I am on the Provider Search page

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

	When I choose Dashboard Link
	Then I am on the Dashboard Overview page
	And I see Standards Shortlist list contains at least 1 items

