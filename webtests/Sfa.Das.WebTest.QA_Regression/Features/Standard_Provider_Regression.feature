Feature: Standard Details View and Provider Selection Regression Tests
	Whenever I search for the key word Mechatronics maintenance technician
	I should find the term "Mechatronics maintenance technician" as the first item on the list of search results page
	When I click the link "Mechatronics maintenance technician"
	A details page should display with details matching the meta data for Mechatronics maintenance technician
	A search for providers in "SN2 1DY" should show the appropriate providers based on postcode listed by distance 
	When I click the "Swindon College" link
	The provider details shown on the provider details page should match the API details for this provider
	

@RegressionTests
Scenario: Find Providers for "Mechatronics maintenance technician"
	Given I was on the <Search page>		
	When I enter <data>	
	| Field      | Value     |
	| Search Box | Mechatronics maintenance technician |
	And I choose <Search Button>	
	Then I am on the <Search Results page> page	
	When I choose <First Standard Result>	
	Then I am on the <Standard Details page> page	
	When I enter <data>	
	| Field      | Value     |
	| Postcode Search Box | SN2 1DY |
	And I choose <Search Button>	
	Then I am on the <Standard Provider Results page> page	
	And I see <Provider Results list> page	

	When I choose <First Provider Link>	
	Then I am on the <Provider Details page> page
	And I see the Text in table
	| Field         | Rule   | Value |	
	| Provider Name | has text | Swindon College  |