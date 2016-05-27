Feature: Apprenticeship Search Smoke Test
	As an employer
	I want to be able to search for training options
	and find a provider for a given framework

@Smoke		@CI @SystemTest @Demo @PreProd @Prod
Scenario:Should find an apprenticeship
Given I navigated to the Start page
	Given I have data for a framework
	And I navigated to the Search page

	When I enter data
		| Field      | Value     |
		| Search Box | {JOBROLE} |
	And I choose Search Button
	Then I am on the Search Results page