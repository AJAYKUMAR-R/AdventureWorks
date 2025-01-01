---SEEDING THE ROLES AND CLAIMS

USE AdventureWorks2019

BEGIN TRAN

SELECT * FROM DBO.AspNetRoles


-- Seeding Roles
INSERT INTO AspNetRoles (Id,Name, NormalizedName)
VALUES 
(1,'Admin', 'ADMIN'),
(2,'Employee', 'EMPLOYEE'),
(3,'Customer', 'CUSTOMER');

-- Seeding Claims (for Employee roles)
SELECT * FROM AspNetRoleClaims

INSERT INTO AspNetRoleClaims (RoleId,ClaimType,ClaimValue) 
(SELECT 2,'Permissions',Name FROM HumanResources.Department)


----verifying the data which is being inserted
select * from AspNetRoles as ar
inner join AspNetRoleClaims as arc ON ar.Id = arc.RoleId
where ar.Id = 2

