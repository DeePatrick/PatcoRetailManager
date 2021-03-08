CREATE PROCEDURE [dbo].[spSale_Lookup]
	@CashierId nvarchar(128),
	@SaleDate datetime2
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[Sale].[ID]
	FROM Sale 
	Where CashierId=@CashierId AND SaleDate=@SaleDate
END
