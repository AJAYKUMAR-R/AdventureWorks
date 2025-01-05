USE [AdventureWorks2019]
GO

/****** Object:  UserDefinedFunction [Production].[fn_GetUnitofMeasure]    Script Date: 05-01-2025 14:52:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--To fetch the details of the UnitofWeightMesaure dropdown for the UI
CREATE FUNCTION [Production].[fn_GetUnitofMeasure]()
RETURNS TABLE
AS
RETURN
(
	SELECT UnitMeasureCode FROM Production.UnitMeasure
)
GO

