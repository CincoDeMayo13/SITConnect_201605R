CREATE TABLE [dbo].[user_info](
[userID] [int] IDENTITY(1,1) NOT NULL,
[fname] [nvarchar](200) NULL,
[lname] [nvarchar](200) NULL,
[dob] [DATE] NULL,
[cardnumber] [int] NULL,
[cardexpiry] [nvarchar](5) NULL,
[cvv] [int] NULL,
[email] [nvarchar](200) NULL,
[password] [nvarchar](50) NULL,
[photo] [nvarchar](200) NULL,
CONSTRAINT [PK_user_info] PRIMARY KEY CLUSTERED
(
	[userID] ASC
)WITH (PAD_INDEX = off, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

