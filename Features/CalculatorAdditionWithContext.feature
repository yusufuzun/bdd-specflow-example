@UserStory(25)
Feature: Calculator Addition With Context
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given I have a new "Basic Calculator"
	Given I have a new "Advanced Calculator"
		
@business @errorHandling @TaskId(90)
Scenario: Add two numbers with basic calculator
	Given I have entered 50 into the "Basic Calculator"
	And I have entered 70 into the "Basic Calculator"
	When I press add in "Basic Calculator" and see "basic calculator result"
	Then the "basic calculator result" should be 120 on the screen
	
@business @errorHandling @TaskId(95)
Scenario: Add two numbers with advanced calculator
	Given I have entered 50 into the "Advanced Calculator"
	And I have entered 70 into the "Advanced Calculator"
	When I press add in "Advanced Calculator" and see "advanced calculator result"
	Then the "advanced calculator result" should be 120 on the screen