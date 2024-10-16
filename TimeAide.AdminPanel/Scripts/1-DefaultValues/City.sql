-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultCity
	@ClientId int
AS
BEGIN

	DECLARE @StateId int;
	Select @StateId = StateId from [State] where ClientId = @ClientId And [StateCode] = 'PR';

	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Arecibo', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Barceloneta', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Camuy', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Dorado', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Isabela', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Manati', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Vega Alta', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Vega Baja', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Bayamon', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Catano', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Guaynabo', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Levittown', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.353' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Valencia', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Canovanas', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Carolina', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Trujillo Alto', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Florida', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Aibonito', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Arroyo', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Barranquitas', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Cayey', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Coamo', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Corozal', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Guayama', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Juana Diaz', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Aguas Buenas', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Caguas', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Culebra', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Fajardo', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Gurabo', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Humacao', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Juncos', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.357' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Rio Grande', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Vieques', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Aguada', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Aguadilla', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Anasco', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Cabo Rojo', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Hormigueros', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Mayaguez', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'San German', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'San Sebastian', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Adjuntas', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Guanica', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Ponce', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Utuado', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Yauco', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
	INSERT [dbo].[City] ([CityName], [CityDescription], [StateId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'Salinas', NULL, @StateId, @ClientId, 1, CAST(N'2020-08-26 19:16:49.360' AS DateTime), 1)
END

