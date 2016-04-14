Feature: ProviderDetailsforFrameworks
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


Scenario Outline: Verify provider detail page
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
And  I select any of the provider from the list
Then I should see "Provider name"
And I should see "Employer satisfaction"
And I should see "Learner satisfaction"
And I should see "Website course page"
And I should see "Website contact page"
And I should see "Standard name"
And I should see "Training structure"
And I should see "Training location"
And I should see "phone"
And I should see "email"
Examples:
| id | Postcode |
| 25 | B46 3DJ  |
| 12 | CV7 8ED  |
| 17 | cv1 2WT  |


@ignore
Scenario: Verify training options
Given I have provider '<providername>' with all of traning modes
When I open provider detail page
Then under training modes I should see "block release"
And I should see "day release"
And I  should see "at your location"
And I should see location name.

@ignore
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


