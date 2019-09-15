USE [Rockstar]
GO

/****** Object:  Table [dbo].[Tweets]    Script Date: 9/15/2019 7:25:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tweets](
	[Id] [uniqueidentifier] NOT NULL,
	[Text] [varchar](280) NOT NULL,
	[location] [geography] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/*** creating spatial index to ease geo search ***/
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO

/****** Object:  Index [si_TweetsLocation]    Script Date: 9/15/2019 7:26:17 PM ******/
CREATE SPATIAL INDEX [si_TweetsLocation] ON [dbo].[Tweets]
(
	[location]
)USING  GEOGRAPHY_AUTO_GRID 
WITH (
CELLS_PER_OBJECT = 12, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

GO

/****** Object:  StoredProcedure [dbo].[GetProximateTweets]    Script Date: 9/15/2019 7:27:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetProximateTweets]
	@long float = 0,
	@lat  float = 0,
	@radius int = 0
AS
BEGIN
	DECLARE @centerPoint geography = geography::Point(@lat, @long, 4326);

	SELECT TEXT, LOCATION.STAsText()
	FROM Tweets
	WHERE @centerPoint.STDistance(LOCATION) <= @radius;
END
GO


GO
/****** Object:  StoredProcedure [dbo].[InsertTweet]    Script Date: 9/15/2019 7:28:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[InsertTweet] 
	@long float = 0,
	@lat float = 0,
	@content text = ''

AS
BEGIN TRANSACTION
INSERT INTO [dbo].[Tweets]
           ([Id]
           ,[Text]
           ,[location])
     VALUES
           (NEWID()
           ,@content
           ,geography::Point(@lat, @long, 4326));
COMMIT
