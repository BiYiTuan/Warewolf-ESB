﻿Feature: Random
	In order to generate random values
	As a Warewolf user
	I want a tool that can generate, numbers, guids and letters


Scenario: Generate Letters
	Given I have a type as "Letters"
	And I have a length as "10"
	When the random tool is executed 
	Then the result from the random tool should be of type "System.String" with a length of "10"
	And the execution has "NO" error

Scenario: Generate Letters and Numbers
	Given I have a type as "LetterAndNumbers"
	And I have a length as "10"
	When the random tool is executed 
	Then the result from the random tool should be of type "System.String" with a length of "10"
	And the execution has "NO" error
	
Scenario: Generate Numbers one digit
	Given I have a type as "Numbers"
	And I have a range from "0" to "9" 
	When the random tool is executed 
	Then the result from the random tool should be of type "System.Int32" with a length of "1"
	And the execution has "NO" error

Scenario: Generate Numbers two digits
	Given I have a type as "Numbers"
	And I have a range from "10" to "99" 
	When the random tool is executed 
	Then the result from the random tool should be of type "System.Int32" with a length of "2"
	And the execution has "NO" error

Scenario: Generate Guid
	Given I have a type as "Guid"
	When the random tool is executed 
	Then the result from the random tool should be of type "System.Guid" with a length of "36"
	And the execution has "NO" error

Scenario: Generate Numbers with blank range
	Given I have a type as "Numbers"
	And I have a range from "" to "" 
	When the random tool is executed 
	Then the result from the random tool should be of type "System.String" with a length of "0"
	And the execution has "AN" error

Scenario: Generate Numbers with one blank range
	Given I have a type as "Numbers"
	And I have a range from "1" to "" 
	When the random tool is executed 
	Then the result from the random tool should be of type "System.String" with a length of "0"
	And the execution has "AN" error

Scenario: Generate Numbers with a negative range
	Given I have a type as "Numbers"
	And I have a range from "-1" to "-9" 
	When the random tool is executed 
	Then the result from the random tool should be of type "System.Int32" with a length of "2"
	And the execution has "NO" error

Scenario: Generate Letters with blank length
	Given I have a type as "Numbers"
	And I have a range from "" to ""  
	When the random tool is executed 
	Then the execution has "AN" error

Scenario: Generate Letters with a negative length
	Given I have a type as "Letters"
	And I have a length as "-1"
	When the random tool is executed 
	Then the result from the random tool should be of type "System.String" with a length of "0"
	And the execution has "AN" error

Scenario: Generate Letters and Numbers with blank length
	Given I have a type as "LetterAndNumbers"
	And I have a length as ""
	When the random tool is executed 
	Then the result from the random tool should be of type "System.String" with a length of "0"
	And the execution has "AN" error

Scenario: Generate Letters and Numbers with a negative length
	Given I have a type as "LetterAndNumbers"
	And I have a length as ""
	When the random tool is executed 
	Then the result from the random tool should be of type "System.String" with a length of "0"
	And the execution has "AN" error

Scenario: Generate a Number between 5 and 5
	Given I have a type as "Numbers"
	And I have a range from "5" to "5" 
	When the random tool is executed 
	Then the result from the random tool should be of type "System.Int32" with a length of "1"
	And the random value will be "5"
	And the execution has "NO" error

Scenario: Generate a Number between a negative index in a recordset and 5
	Given I have a type as "Numbers"
	And I have a range from "[[rec(-1).set]]" to "5" 
	When the random tool is executed 
	Then the execution has "AN" error

Scenario: Generate a Number between 5 and a negative index in a recordset
	Given I have a type as "Numbers"
	And I have a range from "5" to "[[rec(-1).set]]" 
	When the random tool is executed 
	Then the execution has "AN" error

Scenario: Generate Letters with a negative recordset index for length
	Given I have a type as "Letters"
	And I have a length as "[[rec(-1).set]]"
	When the random tool is executed 
	Then the execution has "AN" error

Scenario: Generate Letters and Numbers with a negative recordset index for length
	Given I have a type as "LetterAndNumbers"
	And I have a length as "[[rec(-1).set]]"
	When the random tool is executed 
	Then the execution has "AN" error

