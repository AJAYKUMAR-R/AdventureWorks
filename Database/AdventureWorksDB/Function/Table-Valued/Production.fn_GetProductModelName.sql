USE [AdventureWorks2019]
GO

/****** Object:  UserDefinedFunction [Production].[fn_GetProducModelName]    Script Date: 05-01-2025 15:00:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [Production].[fn_GetProducModelName]()
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

