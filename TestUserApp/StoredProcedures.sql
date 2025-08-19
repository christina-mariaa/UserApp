CREATE PROCEDURE Get_All
AS
BEGIN
	SELECT id, name, surname, age, birth_date from [dbo].[Users] ORDER BY id
END
GO

CREATE PROCEDURE User_Get_By_Id
	@Id INT
AS
BEGIN
	SELECT id, name, surname, age, birth_date from [dbo].[Users] u WHERE u.id = @Id
END
GO

CREATE PROCEDURE Insert_User
	@Name nvarchar(50),
	@Surname nvarchar(50) = NULL,
	@Age int,
	@Birth_date date = NULL,
	@New_id int output

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Users] (name, surname, age, birth_date) VALUES (@Name, @Surname, @Age, @Birth_date);
	SET @New_id = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE Update_User
	@Id int,
	@Name nvarchar(50),
	@Surname nvarchar(50) = NULL,
	@Age int,
	@Birth_date date = NULL

AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].[Users] SET name = @Name, surname = @Surname, age = @Age, birth_date = @Birth_date
	WHERE id = @Id

	SELECT @@ROWCOUNT AS Affected;
END
GO

CREATE PROCEDURE Delete_User
	@Id int
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM [dbo].[Users] WHERE id = @Id;
	SELECT @@ROWCOUNT AS Affected;
END
GO



