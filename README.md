# COCOA - Course Companion

## Installation
- Load project in Visual Studio.
- Restore nuget packages by right clicking the project in the solution explorer.
- Restore npm packages using a command-line with ```npm i``` from the ```wwwroot``` folder.

## Database
Our relational MS-SQL database stores all the informations for Cocoa. It uses ASP.NET Identity for authentication and authorization. Microsoft Entitiy Framework Core has been used for code-first database migration, something that allows querying the database using a DatabaseContext (injected to the controllers) and LINQ syntax. 

## Database relations
![alt tag](https://i.gyazo.com/e41511c2040314d98166b7364a0af712.png)

## Testing
Unit-tests and integration-tests are located in the proj -> /Tests/ folder. COCOA uses MS-Test as a test-runner and is set up to use an in-memory database replicating a deployed environment. Authentication in the test-environment is done using JWT-tokens (enabled on test server only).
### Writing tests
Integration test classes derive from IntegrationTest with protected variables _client and _server. For doing integration tests requiring an authenticated test user, IntegrationTests also derive a RegisterSignIn(email) method for easily creating a new test user and signing in. Test-user email format should be: 
- Teacher: test.[TestClass].[TestMethod]@ntnu.no
- Student: test.[TestClass].[TestMethod]@stud.ntnu.no
### Running tests
Tests can be run in visual studio using the Test Explorer or from the project folder using command-line:
- ```dotnet test```
