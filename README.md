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
Unit-tests and integration-tests are located in the /Tests/ folder. COCOA uses MS-Test as a test-runner. Integration test classes derive from IntegrationTest and are setup to use an in-memory database replicating a deployed environment. Tests can be run in visual studio using the Test Explorer or from the project folder using command-line:
- ```dotnet test```