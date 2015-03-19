﻿@Scheduler
Feature: Scheduler
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers


Scenario: Scheduler Tab
	Given I have Scheduler tab opened
	And "New" is "Enabled"
	And "Save" is "Disabled"
	And "Delete" is "Disabled"
	And selected server is "localhost"
	And "Edit" is "Disabled"
	And "Connect" is "Disabled"
	When I create new schedule
	Then the settings are
	| Name        | Status  | Next Execution |
	| New Task 1* | Enabled | Int            |
	And "Save" is "Enabled"
	And "Delete" is "Enabled"
	And task settings are
	| Name       | Status Selected | Workflow | Edit     | Run Task as soon as | History | Edit Trigger |
	| New Task 1 | Enabled         |          | Disabled |                     |         | Enabled      |
	And username is ""
	And Password is ""


Scenario: Saving New Schedule
	Given I have Scheduler tab opened
	When I create new schedule
	Then the settings are
	| Name        | Status  | Next Execution |
	| New Task 1* | Enabled | Int            |
	And "Save" is "Enabled"
	And "Delete" is "Enabled"
	Then task settings are
	| Name      | Status Selected | Workflow              | Edit    | Run Task as soon as | History |Edit Trigger |
	| Dice Roll | Enabled         | My Category\Dice Roll | Enabled |                     |         |Enabled      |
	And username is as "IntegrationTester"
	And Password is as "I73573r0"
    When I save the Task
	Then Task "Dice Roll" is saved 
	And Save is "Disabled"


Scenario: Deleting a schedule in Scheduler
	Given I have Scheduler tab opened
	And the saved tasks are
	| Name                  | Status  | Next Execution |
	| Dice Roll             | Enabled | Int            |
	| Double Roll and Check | Enabled | Int            |
	And "Save" is "Disabled"
	And "Delete" is "Enabled"
	And task settings are
	| Name      | Status Selected | Workflow              | Edit    | Run Task as soon as | History |Edit Trigger |
	| Dice Roll | Enabled         | My Category\Dice Roll | Enabled |                     |         |Enabled      |
	And username is as "IntegrationTester"
	And Password is as "I73573r0"
    When I Delete "Dice Roll"
	Then Task "Dice Roll" is Deleted 
	And Save is "Disabled"
	And the saved tasks are
	| Name                  | Status  | Next Execution |
	| Double Roll and Check | Enabled | Int            |	


Scenario: Selected task is showing correct settings
	Given I have Scheduler tab opened
	And the saved tasks are
	| Name                  | Status  | Next Execution |
	| Dice Roll             | Enabled | Int            |
	| Double Roll and Check | Enabled | Int            |
	And settings as
	| Name      | Status Selected | Workflow              | Edit    | Run Task as soon as | History |Edit Trigger |
	| Dice Roll | Enabled         | My Category\Dice Roll | Enabled |                     |         |Enabled      |
	And "Dice Roll" task is selected 
	When I select "Double Roll and Check" task
	Then settings as
	| Name      | Status Selected | Workflow                          | Edit    | Run Task as soon as | History |Edit Trigger |
	| Dice Roll | Enabled         | My Category\Double Roll and Check | Enabled |                     |         |Enabled      |



Scenario: Editing scheduled task is prompting to save
	Given I have Scheduler tab opened
	And the saved tasks are
	| Name                  | Status  | Next Execution |
	| Dice Roll             | Enabled | Int            |
	| Double Roll and Check | Enabled | Int            |
	And "Dice Roll" task is selected 
	And settings as
	| Name      | Status Selected | Workflow              | Edit    | Run Task as soon as | History |Edit Trigger |
	| Dice Roll | Enabled         | My Category\Dice Roll | Enabled |                     |         |Enabled      |
	When I edit the settings as
	| Name      | Status Selected | Workflow              | Edit    | Run Task as soon as | History | Edit Trigger |
	| Dice Roll | Disabled        | My Category\Dice Roll | Enabled |                     |         | Enabled      |
	Then "save" is "Enabled"
	When I save the Task
	Then "Dice Roll" is saved
	And the saved tasks are
	| Name                  | Status   | Next Execution |
	| Dice Roll             | Disabled | Int            |
	| Double Roll and Check | Enabled  | Int            |






























