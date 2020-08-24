CREATE PROCEDURE [dbo].[spUserLookup]
	@pId nvarchar(128)
AS
BEGIN
SET NOCOUNT ON;

	SELECT FirstName, LastName, EmailAddress, CreateDate
	FROM [dbo].[User]
	WHERE Id = @pId
END
