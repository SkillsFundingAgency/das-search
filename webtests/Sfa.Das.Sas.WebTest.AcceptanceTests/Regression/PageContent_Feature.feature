Feature: Page Content
	As a page in the Search and Shortlist Service
	I want to display structure and content that is consistent
	So that user experience can be uniform across the service

#@Regression @Pre-prod @Prod
Scenario: Search Page Core Content
	Given I navigated to the Search page
	Then I see
         | Field         | Rule   | Value |
         | Search Box    | Exists | True  |
         | Search Button | Exists | True  |
	     | Footer Element| Exists | True  |

#@Regression @Pre-prod @Prod
Scenario: Search Results Core Content
	Given I navigated to the Search Results page
	Then I wait to see Filter Block
	Then I wait to see Sorting Dropdown
	Then I wait to see Navigation Element

