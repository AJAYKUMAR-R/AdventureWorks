USE [AdventureWorks2019]
GO

/****** Object:  UserDefinedFunction [Production].[fn_GetProductLine]    Script Date: 05-01-2025 14:59:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--NULL ACCEPTS
CREATE FUNCTION [Production].[fn_GetProductLine]()
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

