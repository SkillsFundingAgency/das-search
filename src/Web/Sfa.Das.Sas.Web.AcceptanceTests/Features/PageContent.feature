Feature: Page Content
	As a page in the Search and Shortlist Service
	I want to display structure and content that is consistent
	So that user experience can be uniform across the service

Scenario: Search Page Core Content
	Given I navigated to the Search page
	Then I see
         | Field         | Rule   | Value |
         | Search Box    | Exists | True  |
         | Search Button | Exists | True  |
	     | Footer Element| Exists | True  |

@Regression
Scenario: Search Results Core Content
	Given I navigated to the Search Results page
	And I waited for the view to become active
	Then I see
	| Field              | Rule   | Value |
	| Filter Block       | Exists | True  |
	| Sorting Dropdown   | Exists | True  |
	| Navigation Element | Exists | True  |
	#| Shortlist link     | Exists | True  |

@Regression
Scenario: Standard Details Page	
	Given I navigated to the Search page
	When I enter data
		| Field      | Value         |
		| Search Box | Digital & technology solutions professional	 |
	And  I choose Search Button
	Then I am on the Search Results page	
	When I choose First Standard Result
	Then I am on the Standard Details page 