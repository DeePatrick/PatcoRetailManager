CREATE PROCEDURE [dbo].[spUser_Insert]
	@Id nvarchar(128),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@EmailAddress nvarchar(256),
	@CreateDate datetime2

AS
BEGIN
SET NOCOUNT ON;

	INSERT INTO [dbo].[User] (Id,FirstName, LastName, EmailAddress, CreateDate) 
	VALUES (@Id, @FirstName,@LastName,@EmailAddress,@CreateDate)

	SELECT @Id = SCOPE_IDENTITY();
END
