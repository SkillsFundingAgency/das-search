Feature: FrameworkSearch
	In to find suitable framework
	As an employer
	I want to be able to search frameworks by Title/Pathway



@regression
Scenario Outline: Search Framework by keyword
	Given I am on Search landing page
	And I enter framework '<Title>' in search box
	When I click on search button
	Then I should see matching '<Expected>' frameworks on result page
Examples:
| Title                  | Expected                               |
| Agriculture            | Agriculture                            |
| Animal Technology      | Animal Technology                      |
| Signmaker/Technician   | Signmaking: Signmaker/Technician       |
| Signmaker/Installer    | Signmaking: Signmaker/Installer        |
| Beauty Therapy General | Beauty Therapy: Beauty Therapy General |


@regression
Scenario Outline: Search framework by title and pathway together
Given I am on Search landing page
And I enter framework '<TitlePathway>' in search box
When I click on search button
Then I should see matching '<TitlePathway>' frameworks on result page
Examples:
| TitlePathway                                       |
| Food and Drink: Meat and Poultry Industry Skills   |
| Food and Drink: Baking Industry Skills             |
| Food and Drink: Fish and Shellfish Industry Skills |
