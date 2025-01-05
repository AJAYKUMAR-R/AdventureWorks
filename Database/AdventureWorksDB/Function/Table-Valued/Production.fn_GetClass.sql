USE [AdventureWorks2019]
GO

/****** Object:  UserDefinedFunction [Production].[fn_GetClass]    Script Date: 05-01-2025 15:00:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--Table valued Function
--To fetch the subcategory details for the UI
--NULL ACCEPTS
CREATE FUNCTION [Production].[fn_GetClass]()
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

