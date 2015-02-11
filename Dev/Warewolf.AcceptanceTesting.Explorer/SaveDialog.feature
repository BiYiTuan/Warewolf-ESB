﻿Feature: SaveDialog
	In order to save resources in save dialog
	As a Warewolf user
	I want to save resources 

@SaveDialog
Scenario: Creating Folder from Save Dialog under localhost
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I create "New Folder" in "localhost"
	And I should see "6" folders in "localhost" save dialog


Scenario: Creating Folder from Save Dialog under folder
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I create "New Folder" in "Folder 1"
	Then I should see "9" children for "Folder 1"


Scenario: Saving a Workflow in localhost
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I save "Newworkflow" in "localhost"
	Then "NewWorkflow" is visible in "localhost"
	

Scenario: Saving a Workflow in localhost folder
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I save "Newworkflow" in "Folder 1"
	Then "NewWorkflow" is visible in "Folder 1"	
	

Scenario: Save button is Enabled when I enter new name for resource
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I enter name "Savewf"
	Then save button is "Enabled"
	And validation message is ""
	Then I should see "9" children for "Folder 1"


Scenario: Save button is disabled when name is empty
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I enter name ""
	Then save button is "Disabled"
	And validation message is "'Name' cannot be empty"

Scenario: Save with duplicate name and expect validation
    Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I save "Savewf" in "Folder 1"
	And validation message is ""
	Then I should see "9" children for "Folder 1"
	When I enter name "Savewf"
	Then save button is "Disabled"
	And validation message is "Name already exists"


Scenario: Save resource names with special character expect validation
    Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I enter name "Save@#$"
	Then save button is "Disabled"
	And validation message is "'Name' contains invalid characters."

Scenario: Renaming Folders in Save Dialog
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I rename "Folder 1" to "renamed" in "localhost" save dialog
	Then I should see "8" children for "renamed"
	And I should not see "Folder 1" in "localhost"

Scenario: Renaming Folders with duplicate names
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I open "Folder 1" in "localhost" save dialog 
	Then I should see "8" children for "Folder 1"
	When I rename "Folder 1" to "renamed" in "localhost" save dialog
	Then I should see "8" children for "renamed"
	And I should not see "Folder 1" in "localhost"
	When I rename "Folder 2" to "renamed" in "localhost" save dialog
	Then validation message is thrown "True"


Scenario: Filtering Resources in save dialog
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders in "localhost" save dialog
	When I search for "deleteresouce" in save dialog
    Then I should see "deleteresouce" in "Follder 1"

Scenario: Refresh resources in save dialog
    Given the Save Dialog is opened
    And the "localhost" server is visible in save dialog
    When I refresh resources in savedialog
	Then save dilog localhost is refreshed "True"

Scenario: Opening saved workflow and saving
    Given I have an "unsaved" workflow open
	When I press "Ctrl+s"
	Then the "unsaved" workflow is saved "True"


Scenario: Opening New workflow and saving
    Given I have an New workflow open
	When I press "Ctrl+s"
	Given the Save Dialog is opened
	Then save button is "Disabled"
	And cancel button is "Enabled"
	When I enter name "New"
	Then save button is "Enabled"
	When I save "New" in "localhost"
	Then the "New" workflow is saved "True"


Scenario: Opening Save dialog and canceling
    Given I have an New workflow open
	When I press "Ctrl+s"
	Given the Save Dialog is opened
	Then save button is "Disabled"
	And cancel button is "Enabled"
	When I enter name "New"
	Then save button is "Enabled"
	When I cancel the save dialog
	Then the save dialog is closed
	Then the "New" workflow is saved "False"







