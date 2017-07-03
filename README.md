# Bank Account System
Application is hosted in [http://projectx-randal.azurewebsites.net](http://projectx-randal.azurewebsites.net) if you want to try it out.

## Overview
1. A web application  using ASP.NET Core and Entity Framework
2. A user can register for a new bank account
3. A user can login using the Account Number (please don't use the account name) and password
4. A user can deposit, widthraw, and transfer funds to other users.
5. Basic validations like negative value, insufficient funds, account already existing, and transferring to own account were implemented
6. A page that shows all previous transactions
7. Concurrency was addressed by adding a TimeStamp column and having EF Core designate that column as a concurrency token.
8. Database updates were done using EF Core migrations. These can be found under the Migrations folder of the web project.
9. A separate Unit Test project was created to test all the logic

## Building and Running the Web Application
Restore dependencies
```
dotnet restore
```
Move to the web application directory
```
cd ProjectX
```
Run the web application
```
dotnet run
```
Access the application using this URL:
```
 http://localhost:5000
```
To run the test go to `Project.Tests` folder and run
```
dotnet test
```
