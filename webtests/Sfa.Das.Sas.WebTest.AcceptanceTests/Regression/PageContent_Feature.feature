Feature: Page Content
	As a page in the Search and Shortlist Service
	I want to display structure and content that is consistent
	So that user experience can be uniform across the service

#@Regression
Scenario: Search Page Core Content
	Given I navigated to the Search page
	Then I see
         | Field         | Rule   | Value |
         | Search Box    | Exists | True  |
         | Search Button | Exists | True  |
	     | Footer Element| Exists | True  |

#@Regression
Scenario: Search Results Core Content
	Given I navigated to the Search Results page
	And I waited for the view to become active
	Then I see
	| Field              | Rule   | Value |
	| Filter Block       | Exists | True  |
	| Sorting Dropdown   | Exists | True  |
	| Navigation Element | Exists | True  |
	| Shortlist link     | Exists | True  |

#@Regression
Scenario: Standard Details Page	
	Given I navigated to the Search page
	When I enter data
		| Field      | Value         |
		| Search Box | Digital & technology solutions professional	 |
	And  I choose Search Button
	Then I am on the Search Results page	
	When I choose First Standard Result
	Then I am on the Standard Details page
	And I see
	| Field          | Rule   | Value |
	| Shortlist link | Exists | True  |

#@Regression
#Scenario:Framework Details Page



#@Regression
Scenario: Provider Details Page	
	Given I navigated to the Search page
	When I enter data
		| Field      | Value         |
		| Search Box | Digital & technology solutions professional	 |
	And  I choose Search Button
	Then I am on the Search Results page	
	When I choose First Standard Result
	Then I am on the Standard Details page	
	When I choose Search Page Button
	Then I am on the Provider Search page
	When I enter data
		| Field               | Value   |
		| Postcode Search Box | M15 6BH |
	And I choose Search Button
	Then I am on the Standard Provider Results page	
	When I choose First Provider Link
	Then I am on the Provider Details page
	And I see 
		| Field          | Rule   | Value |
		| Shortlist Link | Exists | True  |

#@Regression
#Scenario: Dashboard Page