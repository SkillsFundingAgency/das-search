Feature: ProviderDetails
	In order to chose a provider from available providers
	As an employer 
	I want to be see full provider detail on provider page

@ignore
Scenario: Provider with more than one loocation,show only one location as choosen by employer
Given I have a provider with more than on location
When I chose a provider from result page
Then I should see only one location on provider detail page

@ignore
Scenario: Provider with 100% employer based training should not show venue  details on provider detail page
Given I have provider with full training at employer location
When I choose this provider from result page
Then I should see provider detail page with no veunue details

@ignore
Scenario: Verify provider detail page
Given I am on provider result page for a choosen standard
When I select any of the provider from the list
Then I should see  provider detail page 
And I should see "Provider name"
And I should see "Employer satisfaction"
And I should see "learner satisfaction"
And I should see "standard name"
And I should see "delivery mode"
And I should see training "venue" name
And I should see training location name
And I should see training contact number
And I should see training contact email

@ignore
Scenario: Verify training options
Given I have provider '<providername>' with all of traning modes
When I open provider detail page
Then under training modes I should see "block release"
And I should see "day release"
And I  should see "at your location"
And I should see location name.

@ginore
#manual end to end Test.
Scenario: Verify end to end provider detail page
Given I have a provider with updated info in course directory
When I open provider detail page 
Then I should see updated provider info in provider detail page.

@ignore
Scenario:Should be able to navigate back to provider result page from detail page.
Given I am on provider detail page
When I click on back link
Then I shoudl be able to return back to provider list page.

@ignore 
#manual test 
Scenario:When provider detail page is bookmarked, it should show only provider detail page when provider is still active.( FCS file to have provider inactive and test bookmarked link)












