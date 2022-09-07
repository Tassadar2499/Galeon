Feature: Authorization
	Check authorization

@mytag
Scenario: User registration
	Given request registration 'Requests/AuthorizationTests/Check_registration.json'
	When registration requested
	And confirm email requested
	And login requested
	Then registration response status code should be 200
	And registration response succeed should be true
	And user should be added to database
	And email to user email should be sent
	And confirm email response status code should be 200
	And confirm email response succeed should be true
	And login response status code should be 200
	And login response token should not be empty
	And login response UserName should equals to user email