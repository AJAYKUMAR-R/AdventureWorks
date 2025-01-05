USE [AdventureWorks2019]
GO

/****** Object:  StoredProcedure [Production].[usp_InsertProduct]    Script Date: 05-01-2025 15:02:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Production].[usp_InsertProduct] 
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

