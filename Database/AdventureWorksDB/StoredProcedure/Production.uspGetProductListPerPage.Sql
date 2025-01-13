USE [AdventureWorks2019]
GO

/****** Object:  StoredProcedure [Production].[uspGetProductListPerPage]    Script Date: 13-01-2025 21:26:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [Production].[uspGetProductListPerPage] (
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

DECLARE @Offset INT;


SET @Offset = (@Page - 1) * @PageSize;

IF @PageSize = 0
BEGIN
	SET @PageSize = IIF(@TotalRecord = 0,1,@TotalRecord)
	SET @Offset =0
END

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


