Feature: Search Provider by Postcode on FrameworkDetailPage
	In order to choose from a provider list
	As an employer
	I want to be able to search provider in my area by entering postcode


@ignore
#validate postcode search field mandatory.
Scenario: Validate post code field mandatory on framework detail page
Given I am on Framework '40322' detail page
When I search Search for provider
Then I should see error message "This field can't be blank"

@ignore
# Provider found( on radius)
Scenario Outline: Search Aardvark Training LIMITED Provider by postcode falling on Provider radius
Given I am on Framework '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "Aardvark Training" in provider results page.
Examples:
| Postcode | id |
| NG24 4SU | 40322 |

@ignore
## Provider not found , but other provider closer one found
Scenario Outline: Search Provider by postcode outside  Provider radius 60 miles
Given I am on Framework '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should not see provider "Aardvark Training" in provider results page.
And  I should see provider "MILLBROOK MANAGEMENT SERVICES LIMITED" in provider results page.
Examples:
| Postcode | id |
| HP18 9QU   | 40322 |

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

