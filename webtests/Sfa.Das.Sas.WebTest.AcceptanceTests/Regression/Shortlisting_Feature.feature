Feature: Shortlisting_Feature
	As an employer
	I want to be able to shortlist a standard and associated provider
	So I can see the shortlisted items in my dashboard

#@Regression		@PreProd @Prod
Scenario: Shortlist a Standard
	Given I navigated to the Search page
	When I choose Search Button
	Then I am on the Search Results page
	When I choose First Standard Result
	Then I am on the Standard Details page
	When I choose Shortlist Link
