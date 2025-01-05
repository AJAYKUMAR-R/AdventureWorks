USE [AdventureWorks2019]
GO

/****** Object:  UserDefinedFunction [Production].[fn_GetStyle]    Script Date: 05-01-2025 14:53:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--NULL ACCEPTS
CREATE FUNCTION [Production].[fn_GetStyle]()
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

