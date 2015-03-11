﻿@NewServerSource
Feature: NewServerSource
	In order to connect to other Warewolf servers
	As a Warewolf user
	I want to be able to manager connections to Warewolf servers


Scenario: Opening New Server Source
	Given I have New Server Source opened
	And "Textbox" is focussed 
	And prtocall selected as "http" 
	And server port as "3142" 
	And Address textbox is "Visible"
	And Authentication type as
	| Windows  | User | Public |
	| Selected |      |        |
	And the test is "Enabled"
	And the save is "Disabled"
	And the message as "The server connection must be tested with a valid adderess before you can save"


Scenario: Creating New Source as windows
   Given I have New Server Source opened
   And I entered "SANDBOX-1" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows  | User | Public |
    | Selected |      |        |
   When I Test Connection
   Then Test Connecton is "Successful"
   And Save is "Enabled"
   When I save the source
   Then the save dialog is opened

Scenario: Windows Test connection is unsuccessfull
   Given I have New Server Source opened
   And I entered "ABSCD" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows  | User | Public |
    | Selected |      |        |
   When I Test Connection
   Then Test Connecton is "UnSuccessful"
   And the validation message as "Connection Error: An error occured while sending the request."
   And Save is "Disabled"
   And the message as "Test Connection"


Scenario: Test connection is unsuccessfull 
   Given I have New Server Source opened
   And I entered "rsaklf@#$" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows  | User | Public |
    | Selected |      |        |
   When I Test Connection
   Then Test Connecton is "UnSuccessful"
   And the validation message as "Connection Error: An error occured while sending the request."
   And Save is "Disabled"
   And the message as "Test Connection"

Scenario: Creating New Source as User
   Given I have New Server Source opened
   And I entered "SANDBOX-1" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows | User     | Public |
    |         | Selected |        |
   Then Username field is "Visible"
   And Password field is "Visible"
   And the test is "Enabled"
   And Save is "Disabled"
   And the message as "The server connection must be tested with a valid adderess before you can save" 
   When I type Username as "IntegrationTester"
   And I type Password as "I73573r0"
   When I Test Connection
   Then Test Connecton is "Successful"
   And Save is "Enabled"
   When I save the source
   Then the save dialog is opened

Scenario: User Test Connection is Unssuccesful
   Given I have New Server Source opened
   And I entered "RSAKLF" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows | User     | Public |
    |         | Selected |        |
   Then Username field is "Visible"
   And Password field is "Visible"
   And the test is "Enabled"
   And Save is "Disabled"
   And the message as "The server connection must be tested with a valid adderess before you can save" 
   When I type Username as "IntegrationTester"
   And I type Password as "I73573r0"
   When I Test Connection
   Then Test Connecton is "UnSuccessful"
   And the validation message as "Connection Error: An error occured while sending the request."
   And Save is "Disabled"
   And the message as "Test Connection"

Scenario: Creating server source Authentication error
   Given I have New Server Source opened
   And I entered "RSAKLF" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows | User     | Public |
    |         | Selected |        |
   Then Username field is "Visible"
   And Password field is "Visible"
   And the test is "Enabled"
   And Save is "Disabled"
   And the message as "The server connection must be tested with a valid adderess before you can save" 
   When I type Username as "#$##$"
   And I type Password as "I73573r0"
   When I Test Connection
   Then Test Connecton is "UnSuccessful"
   And the validation message as "Connection Error: Unauthorized"
   And Save is "Disabled"
   And the message as "Test Connection"

Scenario: Creating New Source as Public
   Given I have New Server Source opened
   And I entered "SANDBOX-1" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows | User | Public   |
    |         |      | Selected |
   Then Username field is "InVisible"
   And Password field is "InVisible"
   And the test is "Enabled"
   And Save is "Disabled"
   And the message as "The server connection must be tested with a valid adderess before you can save" 
   When I Test Connection
   Then Test Connecton is "Successful"
   And Save is "Enabled"
   When I save the source
   Then the save dialog is opened


Scenario: Connecting to server which has no permissions
   Given I have New Server Source opened
   And I entered "SANDBOX-1" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows | User | Public   |
    |         |      | Selected |
   Then Username field is "InVisible"
   And Password field is "InVisible"
   And the test is "Enabled"
   And Save is "Disabled"
   And the message as "The server connection must be tested with a valid adderess before you can save" 
   When I Test Connection
   Then Test Connecton is "UnSuccessful"
   And the validation message as "Connection Error: Unauthorized"
   And Save is "Disabled"
   And the message as "Test Connection"



   
Scenario: Editing server source 
   Given I have New Server Source opened
   And I entered "SANDBOX-1" as remote server name
   And I selcted prtocall as "http" from dropdown
   And I server port as "3142" 
   And Save is "Disabled"
   And I Select Authentication Type as
    | Windows  | User | Public |
    | Selected |      |        |
   When I Test Connection
   Then Test Connecton is "Successful"
   And Save is "Enabled"
   When I save the source
   Then the save dialog is opened




Scenario: Editing Server Source Twice
   Given I Open server source "ServerSource"
   And tab is opened with name as "Edit - ServerSource"
   And remote server name as "SANDBOX-1" is visible
   And I selcted prtocall as "http" from dropdown is visible
   And I server port as "3142" 
   And Authentication Type selected as
    | Windows | User     | Public |
    |         | Selected |        |
   Then Username field is "Visible"
   And Password field is "Visible"
   And Username as "Integrationtester"
   And Password as is "I73573r0"
   And the test is "Enabled"
   And Test Connecton is "Successful"
   And Save is "Disabled"
   When I edit Authentication Type as
   | Windows | User | Public   |
   |         |      | Selected |
   Then Username field is "InVisible"
   And Password field is "InVisible"
   And the test is "Enabled"
   And Test Connecton is ""
   When I click Test Connection
   Then Test Connecton is "Successfull"
   Then tab is opened with name as "Edit - ServerSource" with star
   And Save is "Enabled"
   When I save the source
   Then the save dialog is "Not opened"
   And server source "ServerSource" is saved


