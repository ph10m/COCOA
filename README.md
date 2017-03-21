# COCOA - Course Companion

## Installation
- Load project in Visual Studio.
- Restore nuget packages by right clicking the project in the solution explorer.
- Restore npm packages using a command line with ```npm i``` from the ```wwwroot``` folder.

## Database
Our database stores all the informations for Cocoa. It uses ASP.NET Identity for authentication and authorization. Microsoft Entitiy Framework Core has been used for code-first database migration, something that allows querying the database using a DatabaseContext (injected to the controllers) and LINQ syntax. 
![alt tag](https://i.gyazo.com/ccbeebd9a6ea56e743b05fdb615be2dd.png)
