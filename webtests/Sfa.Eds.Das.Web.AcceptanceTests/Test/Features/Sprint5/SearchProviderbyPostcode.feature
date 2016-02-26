Feature: Search Provider by Postcode
	In order to choose from a provider list
	As an employer
	I want to be able to search provider in my area by entering postcode
#validate postcode search field mandatory.
Scenario: Validate post code field mandatory
Given I am on Standard '25' detail page
When I search Search for provider
Then I should see error message "This field can't be blank"

# Provider found( on radius)
Scenario Outline: Search ASPIRE ACHIEVE ADVANCE LIMITED Provider by postcode falling on Provider radius
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "aspire archive advance limited" in provider results page.
Examples:
| Postcode| id |
| LN76HN  | 25 |

## Provider not found , but other provider closer one found
Scenario Outline: Search Provider by postcode outside  Provider radius 60 miles
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should not see provider "aspire archive advance limited" in provider results page.
And I should see provider "millbrook management services limited" in provider results page.
Examples:
| Postcode | id |
| LN76HJ   | 25 |

## More than on provider found.

Scenario Outline:Search Provider by postcode falling inside Provider radius
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "aspire archive advance limited" in provider results page.
And I should see provider "millbrook management services limited" in provider results page.
Examples:
| Postcode| id |
| DN209NH | 25 |

Scenario Outline:Search Provider by postcode falling inside more than one provider radius.
Given I am on Standard '<id>' detail page
And I enter '<Postcode>' in provider search box
When I search Search for provider
Then I should see provider "skills team ltd" in provider results page.
And I should see provider "aspire achieve advance limited" in provider results page.
Examples:
| Postcode| id |
| NW66AY | 25 |

## No matched providers

Scenario: Search Provider by invalid postcode1
Given I am on Standard '25' detail page
When I enter 'test' in provider search box
And I search Search for provider
Then I should see message searchresult "There are currently no providers for the apprenticeship standard: Digital & Technology Solutions Professional in test"