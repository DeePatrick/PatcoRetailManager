CREATE PROCEDURE [dbo].[spSale_SaleReport]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT [s].[SaleDate], [s].[SubTotal], [s].[Tax], [s].[Total], [u].[FirstName], [u].[LastName], [u].[EmailAddress]
	FROM [Sale] s
	INNER JOIN [User] u on s.CashierId = u.Id;
END
