GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [RoleName]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([Id], [RoleName]) VALUES (2, N'Operator')
INSERT [dbo].[Roles] ([Id], [RoleName]) VALUES (3, N'Management')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 


INSERT [dbo].[Users] ([Id], [UserName], [Password], [Salt], [FirstName], [LastName], [Email], [Phone], [Address], [IsActive], [LastLogin]) VALUES (1, N'dhaly', N'2F68F5CDD5966E1D3C5997CE7A409FBFEF201419', N'E5CuZac=', NULL, NULL, NULL, NULL, N'Tejgaon', 1, CAST(N'2017-11-20 09:45:58.687' AS DateTime))
INSERT [dbo].[Users] ([Id], [UserName], [Password], [Salt], [FirstName], [LastName], [Email], [Phone], [Address], [IsActive], [LastLogin]) VALUES (2, N'ccl', N'D4E8ED7B8A4AA306FC55B18DB8E9E214570EA01E', N'A3jElZU=', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2017-11-16 13:01:16.947' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
SET IDENTITY_INSERT [dbo].[UsersRoles] ON 


INSERT [dbo].[UsersRoles] ([Id], [UserId], [RoleId]) VALUES (1, 1, 1)
INSERT [dbo].[UsersRoles] ([Id], [UserId], [RoleId]) VALUES (2, 1, 3)
INSERT [dbo].[UsersRoles] ([Id], [UserId], [RoleId]) VALUES (3, 2, 2)
SET IDENTITY_INSERT [dbo].[UsersRoles] OFF