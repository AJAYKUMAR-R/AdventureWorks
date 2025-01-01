--For enabling database diagram
USE AdventureWorks2019;
ALTER AUTHORIZATION ON DATABASE::AdventureWorks2019 TO [sa];

---Difference between delete and truncate
---truncate will drop the table and recreate the schema
---Delete will only delete the table
DELETE FROM [dbo].[AspNetUsers];

select * from [dbo].[AspNetUsers]

select Distinct HD.Name from HumanResources.Employee as HE
	Inner join 
HumanResources.EmployeeDepartmentHistory as HHD
	ON HE.BusinessEntityID = HHD.BusinessEntityID
	inner join 
HumanResources.Department as HD
	On HD.DepartmentID = HHD.DepartmentID

Select * from HumanResources.Department

select * from AspNetRoles

select * from AspNetUserClaims

select * from AspNetUsers

select * from AspNetUserRoles


--Verifying the user has properly register with role and claims

select AU.UserName,AR.Name,AUC.ClaimValue from [dbo].[AspNetUsers] as AU
inner join AspNetUserRoles AS UR
	ON AU.ID = UR.UserId
inner join AspNetRoles AS AR
	ON AR.Id = UR.RoleId
inner join AspNetUserClaims AS AUC
	ON AUC.UserId = AU.Id


---WRITTING THE SPROC FOR THE PRODUCT MANAGENT

--1.
---WRITING THE SPROC FOR THE PRODUCT SEARCH DASHBOARD

--SAFE WHERE CONDTION 
--IF THE PRODUCT ID VARAIBLE IS NULL FIRST CONDTION WILL BE TRUE : @PRODUCTID IS NULL 
--SO IT WILL RETURN ALL THE DATA FORM THE TABLE SINCE WE ARE USING OR CONDITION : PP.ProductID = @PRODUCTID
--  PP.ProductID = NULL WIL LBE EVALUATED TO FALSE AT THIS CONDITION

DECLARE @PRODUCTID INT = NULL;
SELECT
    *
FROM
    Production.Product AS PP
WHERE
    (@PRODUCTID IS NULL OR PP.ProductID = @PRODUCTID)

GO


GO

CREATE PROCEDURE Production.USP_GETPRODUCTSPERPAGE (
	@ProductName NVARCHAR(500) = NULL,
    @ProductModelName NVARCHAR(300) = NULL,
    @ProductCategoryName NVARCHAR(300) = NULL,
    @ColumnDirection BIT = 1, -- 1 for ASC, 0 for DESC
    @SortColumnName NVARCHAR(200) = NULL,
    @StartPrice BIGINT = NULL,
    @EndPrice BIGINT = NULL,
    @Page INT = 1,
    @PageSize INT = 10
)
AS
BEGIN

--SELECTING THE FILTERD RESULT FOR THE PAGINATION

	SELECT 
	pp.ProductID,
	pp.Name AS ProductName,
	pp.ProductNumber,
	PPM.ProductModelID,
	PPM.Name AS ModelName,
	PPD.Description,
	PPC.ProductCategoryID,
	PPC.Name AS CategoryName,
	PLPH.ListPrice,
	ROW_NUMBER() OVER (
					ORDER BY 
						CASE 
							WHEN @SortColumnName = 'ProductName' AND @ColumnDirection = 1 THEN pp.Name 
							WHEN @SortColumnName = 'ModelName' AND @ColumnDirection = 1 THEN PPM.Name
							WHEN @SortColumnName = 'CategoryName' AND @ColumnDirection = 1 THEN PPC.Name
						END ASC,
						CASE 
							WHEN @SortColumnName = 'ProductName' AND @ColumnDirection = 0 THEN pp.Name 
							WHEN @SortColumnName = 'ModelName' AND @ColumnDirection = 0 THEN PPM.Name
							WHEN @SortColumnName = 'CategoryName' AND @ColumnDirection = 0 THEN PPC.Name
						END DESC
				) AS RowNum,
	PPH.*
	FROM
	Production.Product AS PP
		INNER JOIN 
	Production.ProductListPriceHistory AS PLPH
		ON
			PP.ProductID = PLPH.ProductID AND EndDate IS NULL
		INNER JOIN 
	Production.ProductModel AS PPM
		ON 
			PPM.ProductModelID = PP.ProductModelID
		INNER JOIN
	Production.ProductModelProductDescriptionCulture AS PMPD
		ON 
			PMPD.ProductModelID = PP.ProductModelID
		INNER JOIN
	Production.ProductDescription AS PPD
		ON
			PPD.ProductDescriptionID = PMPD.ProductDescriptionID AND PMPD.CultureID = 'EN'
		INNER JOIN 
	Production.ProductSubcategory AS PPS
		ON 
			PPS.ProductSubcategoryID = PP.ProductSubcategoryID
		INNER JOIN
	Production.ProductCategory AS PPC	
		ON
			PPC.ProductCategoryID = PPS.ProductCategoryID
		INNER JOIN 
	Production.ProductProductPhoto AS PPP
		ON 
			PPP.ProductID = PP.ProductID
		INNER JOIN 
	Production.ProductPhoto AS PPH
		ON 
			PPH.ProductPhotoID = PPP.ProductPhotoID
	WHERE
		(
			PP.Name LIKE  '%' + @ProductName + '%'
						OR
			@ProductName IS NULL
		)

		AND

		(
			PPM.Name LIKE  '%' + @ProductModelName + '%'
						OR
			@ProductModelName IS NULL
		)
		AND

		(
			PPC.Name LIKE  '%' + @ProductCategoryName + '%'
						OR
			@ProductCategoryName IS NULL
		)
		AND

		(
			(PLPH.ListPrice >= @StartPrice AND PLPH.ListPrice <= @EndPrice)
						OR
			(@StartPrice IS NULL OR @EndPrice IS NULL)
		)


SELECT @TOTALRECORD = COUNT(1) FROM #RESULTTEMP



IF @PAGE = 0 OR @PAGESIZE = 0
BEGIN
	 RAISERROR ('Page or Page size can"t be zero', 16, 1);
END

SET @PAGE = (@PAGE - 1) * @PAGESIZE;

SELECT 
*
FROM
#RESULTTEMP
ORDER BY 
		CASE WHEN @COLUMNNAME = 'ProductName'  AND   @COULMNDIRECTION = 1 THEN ProductName END ASC,
		CASE WHEN @COLUMNNAME = 'ProductName'  AND   @COULMNDIRECTION = 0 THEN ProductName END DESC,
		CASE WHEN @COLUMNNAME = 'ModelName'	   AND   @COULMNDIRECTION = 1 THEN ModelName END ASC,
		CASE WHEN @COLUMNNAME = 'ModelName'    AND   @COULMNDIRECTION = 0 THEN ModelName END DESC,
		CASE WHEN @COLUMNNAME = 'CategoryName' AND   @COULMNDIRECTION = 1 THEN CategoryName END ASC,
		CASE WHEN @COLUMNNAME = 'CategoryName' AND   @COULMNDIRECTION = 0 THEN CategoryName END DESC
OFFSET @PAGE ROWS
FETCH NEXT @PAGESIZE ROWS ONLY


END

CREATE PROCEDURE Production.USP_GETPRODUCTSPERPAGE (
	@PRODUCTNAME VARCHAR(500),
	@PRODUCTMODELNAME VARCHAR(300),
	@PRODUCTCATEGORIENAME VARCHAR(300),
	@COULMNDIRECTION BIT,
	@COLUMNNAME VARCHAR(200),
	@STARTPRCIE BIGINT,
	@ENDPRICE BIGINT,
	@TOTALRECORD INT,
	@PAGE INT,
	@PAGESIZE INT
)
AS
BEGIN


IF OBJECT_ID('tempdb..#RESULTTEMP') IS NOT NULL
    DROP TABLE #RESULTTEMP;


SELECT 
pp.ProductID,
pp.Name AS ProductName,
pp.ProductNumber,
PPM.ProductModelID,
PPM.Name AS ModelName,
PPD.Description,
PPC.ProductCategoryID,
PPC.Name AS CategoryName,
PLPH.ListPrice,
PPH.*
INTO #RESULTTEMP
FROM
Production.Product AS PP
	INNER JOIN 
Production.ProductListPriceHistory AS PLPH
	ON
		PP.ProductID = PLPH.ProductID AND EndDate IS NULL
	INNER JOIN 
Production.ProductModel AS PPM
	ON 
		PPM.ProductModelID = PP.ProductModelID
	INNER JOIN
Production.ProductModelProductDescriptionCulture AS PMPD
	ON 
		PMPD.ProductModelID = PP.ProductModelID
	INNER JOIN
Production.ProductDescription AS PPD
	ON
		PPD.ProductDescriptionID = PMPD.ProductDescriptionID AND PMPD.CultureID = 'EN'
	INNER JOIN 
Production.ProductSubcategory AS PPS
	ON 
		PPS.ProductSubcategoryID = PP.ProductSubcategoryID
	INNER JOIN
Production.ProductCategory AS PPC	
	ON
		PPC.ProductCategoryID = PPS.ProductCategoryID
	INNER JOIN 
Production.ProductProductPhoto AS PPP
	ON 
		PPP.ProductID = PP.ProductID
	INNER JOIN 
Production.ProductPhoto AS PPH
	ON 
		PPH.ProductPhotoID = PPP.ProductPhotoID
WHERE
	(
		PP.Name LIKE  '%' + @PRODUCTNAME + '%'
					OR
		@PRODUCTNAME IS NULL
	)

	AND

	(
		PPM.Name LIKE  '%' + @PRODUCTMODELNAME + '%'
					OR
		@PRODUCTMODELNAME IS NULL
	)
	AND

	(
		PPM.Name LIKE  '%' + @PRODUCTCATEGORIENAME + '%'
					OR
		@PRODUCTCATEGORIENAME IS NULL
	)
	AND

	(
		(PLPH.ListPrice >= @STARTPRCIE AND PLPH.ListPrice <= @ENDPRICE)
					OR
		(@STARTPRCIE IS NULL OR @ENDPRICE IS NULL)
	)



END