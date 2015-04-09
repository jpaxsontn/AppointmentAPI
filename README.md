Coding Challenge

Choose 1 of the following

Using the your language of choice language, have the function LetterCount(str) take the str parameter being passed and return the first word with the greatest number of repeated letters. For example: "Today, is the greatest day ever!" should return greatest because it has 2 e's (and 2 t's) and it comes before ever which also has 2 e's. If there are no words with repeating letters return -1. Words will be separated by spaces.
Using the your language of choice, have the function PalindromeTwo(str) take the str parameter being passed and return the string true if the parameter is a palindrome, (the string is the same forward as it is backward) otherwise return the string false. The parameter entered may have punctuation and symbols but they should not affect whether the string is in fact a palindrome. For example: "Anne, I vote more cars race Rome-to-Vienna" should return true. 
function PalindromeTwo(str) { 
  // code goes here  
  return str;        
}  
// keep this function call here 
// to see how to enter arguments in JavaScript scroll down
print(PalindromeTwo(readline()));
Test 2 
Author a RESTful API using the Nancy FX framework that exposes functionality around the use cases below. Please use the domain model below as a starting point to understand concepts but feel free to adjust/add/remove elements as long as you are able to conversationally justify your decision. The goal here is to focus on creating a fully-fledged API for scheduling appointments. 
Domain 
Appointment
Attributes: Id, Patient, Provider, Service, Reason for Visit, Planned Duration
Notes: All fields are required. Appointments may only be scheduled for service with providers who have availability and have a certification level equal to or exceeding what is required by the service. Additionally the patient should be old enough to participate in said service.
Patient
Attributes: Id, Name, Age
Provider
Attributes: Id, Name, Certification Level (int)
Notes: 
Service
Attributes: Id, Name, Required Certification Level (int), Duration, Required Age
Requirements
•	When booking an appointment the provider should be available meaning not having had their time booked.
•	Providers should have five minutes in between appointments. 
•	Appointments can only be scheduled between the hours of 9 am and 4pm.


