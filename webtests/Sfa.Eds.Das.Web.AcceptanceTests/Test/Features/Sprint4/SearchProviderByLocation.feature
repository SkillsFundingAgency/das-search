Feature:SearchProvider by location
	In order to find training providers
	As an employer
	I want to search providers by entering geo lcoation
	

Scenario Outline: Entered Lat long to search provider is within provider given radius
Given I am on Standard '<id>' detail page
And I enter '<LatLong>' in provider search box
When I click on search button
Then I should list of providers on provider search result page.
Examples:
| LatLong               | id |
| 52.4113623,-1.5256923 | 25 |



Scenario Outline:Provider INTEC BUSINESS COLLEGES searchable when entered latlong with in Provider radius
Given I am on Standard '<id>' detail page
And I enter '<LatLong>' in provider search box
When I search Search for provider
Then I should see provider "intec business colleges" in provider results page.
Examples:
| LatLong               | id |
| 52.4113623,-1.5256923 | 25 |


Scenario Outline: Search ASPIRE ACHIEVE ADVANCE LIMITED Provider by latlong falling on Provider radius
Given I am on Standard '<id>' detail page
And I enter '<LatLong>' in provider search box
When I search Search for provider
Then I should see provider "aspire archive advance limited" in provider results page.
Examples:
| LatLong              | id |
| 53.4679071,-0.3699098 | 25 |


Scenario Outline:Search Provider by latlong falling inside Provider radius
Given I am on Standard '<id>' detail page
And I enter '<LatLong>' in provider search box
When I search Search for provider
Then I should see provider "aspire archive advance limited" in provider results page.
And I should see provider "millbrook management services limited" in provider results page.
Examples:
| LatLong               | id |
| 53.5152058,-0.5601394 | 25 |


#search for by location(latlong or postcode) which falls outside 60 miles provider radius
Scenario Outline: Search Provider by latlong outside  Provider radius 60 miles
Given I am on Standard '<id>' detail page
And I enter '<LatLong>' in provider search box
When I search Search for provider
Then I should not see provider "aspire archive advance limited" in provider results page.
And I should see provider "millbrook management services limited" in provider results page.
Examples:
| LatLong               | id |
| 53.5117482,-0.3669604 | 25 |

@ignore
Scenario Outline: Search Provider by latlong falling inside two provider radius.
Given I am on Standard '<id>' detail page
And I enter '<LatLong>' in provider search box
When I search Search for provider
Then I should see provider "aspire archive advance limited" in provider results page.
And I should see provider "millbrook management services limited" in provider results page.
And I should see provider "millbrook management services limited" listed in top as nearest training provider.
Examples:
| LatLong               | id |
| 53.5152058,-0.5601394 | 25 |

@ignore
#Provider "INTEC BUSINESS COLLEGES" in CV21 2BB with radius of 35 miles
#Provider "COVENTRY & WARWICKSHIRE CHAMBER TRAINING (CWT)" in CV32 4JE with radius of 40 miles
Scenario Outline: Search results More than on provider returned in search result page.
Given I am on Standard '<id>' detail page
And I enter '<LatLong>' in provider search box
When I search Search for provider
Then I should see total of "5" providers found.
And I should see provider "INTEC BUSINESS COLLEGES" in provider results page.
And I should see provider "COVENTRY & WARWICKSHIRE CHAMBER TRAINING (CWT)" in provider results page.
And I should see provider "Aspire archive advance limited" in provider result page.
And I should see provider "MILLBROOK MANAGEMENT SERVICES LIMITED" in provider results page.
And I should see provider "Training services 2000 ltd" in provider results page.
Examples:
| LatLong               |
| 52.7365701,-1.0496998 |


















