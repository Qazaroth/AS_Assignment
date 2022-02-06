CREATE TABLE [dbo].[Users] (
    [id]                 INT             IDENTITY (1, 1) NOT NULL,
    [email]              VARCHAR (320)   NOT NULL,
    [firstName]          VARCHAR (26)    NOT NULL,
    [lastName]           VARCHAR (26)    NOT NULL,
    [creditCard]         VARCHAR (MAX)   NOT NULL,
    [cvc]                VARCHAR (MAX)   NOT NULL,
    [NRIC]               VARCHAR (MAX)   NOT NULL,
    [password]           VARCHAR (MAX)   NOT NULL,
    [salt]               VARCHAR (MAX)   NOT NULL,
    [birthDate]          DATETIME        NOT NULL,
    [dateTimeRegistered] DATETIME        NULL,
    [isAdmin]            INT             DEFAULT ((0)) NOT NULL,
    [roles]              NVARCHAR (MAX)  NULL,
    [deleted]            INT             DEFAULT ((0)) NOT NULL,
    [avatar]             VARBINARY (MAX) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([id] ASC)
);

