Feature: ViewFrameworkDetails
	In order to chose a Fraemwork
	As an employer
	I want to be able to open Framework details page.


@regression
Scenario Outline: Search framework by title and pathway together
Given I am on Search landing page
And I enter framework '<TitlePathway>' in search box
When I click on search button
And I choose  framework '<TitlePathway>' from search result page 
Then I should see Framework '<FrameworkTitle>' on framework detail page
And I should see Framework pathway '<PathwayTitle>' on framework detail page
Examples:
| TitlePathway                                       | FrameworkTitle | PathwayTitle                       |
| Food and Drink: Meat and Poultry Industry Skills   | Food and Drink | Meat and Poultry Industry Skills   |
| Food and Drink: Baking Industry Skills             | Food and Drink | Baking Industry Skills             |
| Food and Drink: Fish and Shellfish Industry Skills | Food and Drink | Fish and Shellfish Industry Skills |