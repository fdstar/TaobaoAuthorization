# ASP.NET Core & EntityFramework Core Based Startup Template

This template is a simple startup project to start with ABP
using ASP.NET Core and EntityFramework Core.

## Prerequirements

* Visual Studio 2017
* .NET Core SDK
* SQL Server

## How To Migration
* Add-Migration -Name "TaobaoAuth_Init" -Context "TaobaoAuthorization.EntityFrameworkCore.TaobaoAuthorizationDbContext" -Project "TaobaoAuthorization.EntityFrameworkCore" -StartupProject "TaobaoAuthorization.Web"
* Remove-Migration -Context "TaobaoAuthorization.EntityFrameworkCore.TaobaoAuthorizationDbContext" -Project "TaobaoAuthorization.EntityFrameworkCore" -StartupProject "TaobaoAuthorization.Web"
* Update-Database -Context "TaobaoAuthorization.EntityFrameworkCore.TaobaoAuthorizationDbContext" -Project "TaobaoAuthorization.EntityFrameworkCore" -StartupProject "TaobaoAuthorization.Web"
* **get-help**

## Seed Sql
*Sql Server*
``` Sql
INSERT INTO [dbo].[Partners]
           ([CreationTime]
           ,[PartnerKey]
           ,[SecretKey]
           ,[PartnerName]
           ,[IsDisabled])
     VALUES
           (GETDATE()
           ,'Partner001'
           ,'1234567890'
           ,'测试合作号'
           ,0)
GO
```

## How To Run

* Open solution in Visual Studio 2017
* Set .Web project as Startup Project and build the project.
* Run the application.
