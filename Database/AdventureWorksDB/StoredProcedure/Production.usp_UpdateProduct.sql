USE [AdventureWorks2019]
GO

/****** Object:  StoredProcedure [Production].[usp_UpdateProduct]    Script Date: 05-01-2025 15:02:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Production].[usp_UpdateProduct] 
(
    @ProductID INT, -- The primary key of the product to be updated
    @Name NVARCHAR(100),
    @ProductNumber NVARCHAR(50),
    @MakeFlag BIT,
    @FinishedGoodsFlag BIT,
    @Color NVARCHAR(30) NULL,
    @SafetyStockLevel SMALLINT,
    @ReorderPoint SMALLINT,
    @StandardCost MONEY,
    @ListPrice MONEY,
    @Size NVARCHAR(10) NULL,
    @SizeUnitMeasureCode NCHAR(6) NULL,
    @WeightUnitMeasureCode NCHAR(6) NULL,
    @Weight DECIMAL(8,5) NULL,
    @DaysToManufacture INT,
    @ProductLine NCHAR(4) NULL,
    @Class NCHAR(4) NULL,
    @Style NCHAR(4) NULL,
    @ProductSubcategoryID INT NULL,
    @ProductModelID INT NULL,
    @SellStartDate DATETIME,
    @SellEndDate DATETIME NULL,
    @DiscontinuedDate DATETIME NULL
)
AS
BEGIN
    -- Update the product based on the provided ProductID
    UPDATE Production.Product
    SET 
        Name = @Name,
        ProductNumber = @ProductNumber,
        MakeFlag = @MakeFlag,
        FinishedGoodsFlag = @FinishedGoodsFlag,
        Color = @Color,
        SafetyStockLevel = @SafetyStockLevel,
        ReorderPoint = @ReorderPoint,
        StandardCost = @StandardCost,
        ListPrice = @ListPrice,
        Size = @Size,
        SizeUnitMeasureCode = @SizeUnitMeasureCode,
        WeightUnitMeasureCode = @WeightUnitMeasureCode,
        [Weight] = @Weight,
        DaysToManufacture = @DaysToManufacture,
        ProductLine = @ProductLine,
        Class = @Class,
        Style = @Style,
        ProductSubcategoryID = @ProductSubcategoryID,
        ProductModelID = @ProductModelID,
        SellStartDate = @SellStartDate,
        SellEndDate = @SellEndDate,
        DiscontinuedDate = @DiscontinuedDate
    WHERE ProductID = @ProductID; -- The condition for updating the correct record
END;
GO

