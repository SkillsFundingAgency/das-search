Feature: Providerlistdetails
	In order to chose a provider from list
	As an employer 
	I should be able to see list of providers for a standard training
#@ignore
#Scenario: Verify delivery mode 100 % employer based training is listed on top
#Given I have chosen a Standard
#When I search for provider by postcode
#Then I should see provider who provide employer based training is listed on top
#And I see Distance showing text message "Training takes place at employer's location"
#@ignore
#Scenario: Verify providers listed by nearest first 
#Given I have chosen a Standard
#When I search for  provider by postcode
#Then I should see matched providers list with nearest provider first in the list
#@ignore
#Scenario: Verify provider has no location specified
#Given I have chosen a Standard
#When I search for provider by postcode
#Then I should see provider who has no location details 
#And I see distance showing message "Training take place at employer's location"
#
#@ignore
#Scenario: Verify provider has more than one location
#Given I have chosen a Standard
#When I search for provider by postcode
#And I have provider with  more than one location
#Then I  see in search results same provider listed multiple times showing nearest location first in the list.
#
#@ignore
#Scenario: Verify no providers found.
#Given I have chosen a Standard
#When I search for provider by postcode
#And I have no providers operating in given postcode
#Then I should see nothing in result page with message "There are currently no training providers found for Standard <> in postcode <>"
#
#@ignore
#Scenario: Verify provider location name same as provider name
#Given I have found provider name and location name
#When I search for provider by postcode
#Then I should see provider name in result page
#And I should not see location name field


Scenario Outline: Verify Provider with additional information on the result page
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should all providers in result page
And   under each provider I should see provider "website"
And I should see provider "location name"
And I should see provider "location address"
And I should see provider "Employer satisfaction"
And I should see provider "Learner satisfaction"
Examples:
| id | Postcode |
| 25 | CV7 8ED  |
| 12 | cv1 2wt  |

#@ignore
#Scenario: Verify provider with no employer or learner satisfaction data
#Given I provider with no employer satisfaction data
#When I search for a provider which doesn't have employer satisfaction data
#And I see matched provider list
#Then I see should provider with employer satisfaction field empty.
#
#@ignore 
## manual test
#Scenario: Verify provider list page to show only active providers for a standard
#Given I am on provider  list page page
#And I have bookmarked
#When I open the link 
#Then I should see only active providers in provider list page who currently provides training.