Feature: Search Provider by Postcode on FrameworkDetailPage
	In order to choose from a provider list
	As an employer
	I want to be able to search provider in my area by entering postcode

	### verify providers are found in search results when employer search by postcode within  provider radius
	##to test this feature,for testing purpose at test provider "DUDLEY METROPOLITAN BOROUGH COUNCIL" is searched from different
	##different locations to validate if this is show in search result page. 


#validate postcode search field mandatory.
Scenario: Validate post code field mandatory on framework detail page
Given I am on Framework '40322' detail page
When I search Search for provider
Then I should see error message "This field can't be blank"


# Provider found( on radius)
Scenario Outline: Search DUDLEY METROPOLITAN BOROUGH COUNCIL Provider by postcode falling on Provider radius
Given I am on Framework '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "DAS Test Provider - Nasir Khan" in provider results page.
Examples:
Examples:
| id    | Postcode |
| 40327 | SO14 1PB |
| 40325 | SO14 1PB |


## Provider not found , but other provider closer one found
Scenario Outline: Search Provider by postcode outside  Provider radius 80 miles
Given I am on Framework '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should not see provider "DUDLEY METROPOLITAN BOROUGH COUNCIL" in provider results page.
Examples:
| Postcode | id |
| LS21 3JS   | 40335 |

## More than on provider found.




@ignore
Scenario Outline:Search Provider by postcode falling inside Provider radius
Given I am on Framework '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "ASPIRE ACHIEVE ADVANCE LIMITED" in provider results page.
And I should see provider "MILLBROOK MANAGEMENT SERVICES LIMITED" in provider results page.
Examples:
| Postcode| id |
| DE73 8EN | 40322 |

