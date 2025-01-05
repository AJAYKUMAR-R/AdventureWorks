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

CREATE PROCEDURE Production.uppGetProductListPerPage (
	@ProductName NVARCHAR(500) = NULL,
    @ProductModelName NVARCHAR(300) = NULL,
    @ProductCategoryName NVARCHAR(300) = NULL,
    @ColumnDirection BIT = 1, -- 1 for ASC, 0 for DESC
    @SortColumnName NVARCHAR(200) = NULL,
    @StartPrice BIGINT = NULL,
    @EndPrice BIGINT = NULL,
    @Page INT = 1,
    @PageSize INT = 10,
	@TotalRecord INT OUTPUT
)
AS
BEGIN




IF @Page = 0 OR @PageSize = 0
BEGIN
	 RAISERROR ('Page or Page size can"t be zero', 16, 1);
END

IF @StartPrice > @EndPrice
	RAISERROR('Start price should be smaller than the end price',16,1);

IF OBJECT_ID('tempdb..#RESULTTEMP') IS NOT NULL
	DROP TABLE #RESULTTEMP


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


SELECT @TotalRecord = COUNT(1) FROM #RESULTTEMP



SET @Page = (@Page - 1) * @PageSize;

SELECT 
*
FROM
#RESULTTEMP
ORDER BY 
		CASE WHEN @SortColumnName = 'ProductName'  AND   @ColumnDirection = 1 THEN ProductName END ASC,
		CASE WHEN @SortColumnName = 'ProductName'  AND   @ColumnDirection = 0 THEN ProductName END DESC,
		CASE WHEN @SortColumnName = 'ModelName'	   AND   @ColumnDirection = 1 THEN ModelName END ASC,
		CASE WHEN @SortColumnName = 'ModelName'    AND   @ColumnDirection = 0 THEN ModelName END DESC,
		CASE WHEN @SortColumnName = 'CategoryName' AND   @ColumnDirection = 1 THEN CategoryName END ASC,
		CASE WHEN @SortColumnName = 'CategoryName' AND   @ColumnDirection = 0 THEN CategoryName END DESC
OFFSET @PAGE ROWS
FETCH NEXT @PAGESIZE ROWS ONLY


END

GO

DECLARE @TotalRecord INT;

EXEC Production.USP_GETPRODUCTSPERPAGE 
    @ProductName = 'Tour',
    @ProductModelName = 'Touring-2000',
    @ProductCategoryName = '',
    @ColumnDirection = 0, -- Ascending order
    @SortColumnName = 'ProductName',
    @StartPrice = 1000,
    @EndPrice = 2000,
    @Page = 1,
    @PageSize = 5,
    @TotalRecord = @TotalRecord OUTPUT;

-- To check the total record count returned
SELECT @TotalRecord AS TotalRecords;


GO

--FUNCTIONLITY : ADDING RECORDS

GO

--Table valued Function
--To fetch the subcategory details for the UI
CREATE FUNCTION Production.fn_GetProductSubCategories()
RETURNS TABLE
AS
RETURN
(
    SELECT 
        ProductSubCategoryName = Name,
		ProductSubcategoryID
    FROM
        Production.ProductSubcategory
);

GO

CREATE FUNCTION Production.fn_GetProducModelName()
RETURNS TABLE
AS
RETURN
(
    SELECT 
        ProductModelName = Name,
		ProductModelID
    FROM
        Production.ProductModel
);

GO

GO
--Table valued Function
--To fetch the subcategory details for the UI
--NULL ACCEPTS
CREATE FUNCTION Production.fn_GetClass()
RETURNS TABLE
AS
RETURN
(
   SELECT 1 AS ID,'H' AS [TYPE]
   UNION ALL
   SELECT 2,'M'
   UNION ALL
   SELECT 3,'L'
);


GO
--NULL ACCEPTS
CREATE FUNCTION Production.fn_GetStyle()
RETURNS TABLE
AS
RETURN
(
   SELECT 1 AS ID,'U' AS [TYPE]
   UNION ALL
   SELECT 2,'M'
   UNION ALL
   SELECT 3,'W'
);

GO

--NULL ACCEPTS
CREATE FUNCTION Production.fn_GetProductLine()
RETURNS TABLE
AS
RETURN
(
   SELECT 1 AS ID,'R' AS [TYPE]
   UNION ALL
   SELECT 2,'M'
   UNION ALL
   SELECT 3,'T'
   UNION ALL
   SELECT 3,'S'
);

GO
--To fetch the details of the UnitofWeightMesaure dropdown for the UI
CREATE FUNCTION Production.fn_GetUnitofMeasure()
RETURNS TABLE
AS
RETURN
(
	SELECT UnitMeasureCode FROM Production.UnitMeasure
)



GO
CREATE PROCEDURE Production.usp_InsertProduct 
(
    @Name NVARCHAR(100) ,
    @ProductNumber NVARCHAR(50) ,
    @MakeFlag BIT ,
    @FinishedGoodsFlag BIT ,
    @Color NVARCHAR(30) NULL,
    @SafetyStockLevel SMALLINT ,
    @ReorderPoint SMALLINT ,
    @StandardCost MONEY ,
    @ListPrice MONEY ,
    @Size NVARCHAR(10) NULL,
    @SizeUnitMeasureCode NCHAR(6) NULL,
    @WeightUnitMeasureCode NCHAR(6) NULL,
    @Weight DECIMAL(8,5) NULL,
    @DaysToManufacture INT ,
    @ProductLine NCHAR(4) NULL,
    @Class NCHAR(4) NULL,
    @Style NCHAR(4) NULL,
    @ProductSubcategoryID INT NULL,
    @ProductModelID INT NULL,
    @SellStartDate DATETIME ,
    @SellEndDate DATETIME NULL,
    @DiscontinuedDate DATETIME NULL
)
AS
BEGIN
	
    INSERT INTO Production.Product
    (
        Name,
        ProductNumber,
        MakeFlag,
        FinishedGoodsFlag,
        Color,
        SafetyStockLevel,
        ReorderPoint,
        StandardCost,
        ListPrice,
        Size,
        SizeUnitMeasureCode,
        WeightUnitMeasureCode,
        [Weight],
        DaysToManufacture,
        ProductLine,
        Class,
        Style,
        ProductSubcategoryID,
        ProductModelID,
        SellStartDate,
        SellEndDate,
        DiscontinuedDate
    )
    VALUES
    (
        @Name,
        @ProductNumber,
        @MakeFlag,
        @FinishedGoodsFlag,
        @Color,
        @SafetyStockLevel,
        @ReorderPoint,
        @StandardCost,
        @ListPrice,
        @Size,
        @SizeUnitMeasureCode,
        @WeightUnitMeasureCode,
        @Weight,
        @DaysToManufacture,
        @ProductLine,
        @Class,
        @Style,
        @ProductSubcategoryID,
        @ProductModelID,
        @SellStartDate,
        @SellEndDate,
        @DiscontinuedDate
    );
END;



GO

EXEC Production.usp_InsertProduct 
    @Name = 'Product A',
    @ProductNumber = 'P12345',
    @MakeFlag = 1,  -- Assuming 1 means 'True' (for MakeFlag)
    @FinishedGoodsFlag = 1,  -- Assuming 1 means 'True' (for FinishedGoodsFlag)
    @Color = 'Red',
    @SafetyStockLevel = 100,
    @ReorderPoint = 50,
    @StandardCost = 20.00,
    @ListPrice = 40.00,
    @Size = 'Medium',
    @SizeUnitMeasureCode = 'KG',
    @WeightUnitMeasureCode = 'KG',
    @Weight = 15.5,
    @DaysToManufacture = 10,
    @ProductLine = 'A1',
    @Class = 'H',
    @Style = 'S1',
    @ProductSubcategoryID = 1,
    @ProductModelID = 1,
    @SellStartDate = '2025-01-01',
    @SellEndDate = '2026-01-01',
    @DiscontinuedDate = NULL; -- Optional, leave NULL if no discontinued date

