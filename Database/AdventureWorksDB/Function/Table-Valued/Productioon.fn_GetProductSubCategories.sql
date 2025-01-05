USE [AdventureWorks2019]
GO

/****** Object:  UserDefinedFunction [Production].[fn_GetProductSubCategories]    Script Date: 05-01-2025 14:54:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--Table valued Function
--To fetch the subcategory details for the UI
CREATE FUNCTION [Production].[fn_GetProductSubCategories]()
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

