ALTER proc [dbo].[Jobs_SelectJobByJobId]
		@Id int
as

BEGIN

		SELECT j.[Id]
			  ,j.[UserId]
			  ,j.[Status]
			  ,j.[JobType]
			  ,j.[Price]
			  ,j.[JobRequest]
			  ,j.[ContactName]
			  ,j.[Phone]
			  ,j.[SpecialInstructions]
			  ,j.[Created]
			  ,j.[Modified]
			  ,j.[WebsiteId]
			  ,j.[ExternalJobId]
			  ,j.[ExternalCustomerId]

			  ,u.[UserId]
			  ,u.[FirstName]
			  ,u.[LastName]
			  ,u.[ExternalUserId]
			  ,u.[DateAdded]
			  ,u.[DateModified]
			  ,u.[MediaId]
			  ,u.[TokenHash]

			  ,[wb].[Id]
			  ,[wb].[Name]
			  ,[wb].[Slug]
			  ,[wb].[Description]
			  ,[wb].[Url]
			  ,[wb].[MediaId]
			  ,[wb].[DateCreated]
			  ,[wb].[DateModified]
			  ,[wb].[Phone]
			  ,[wb].[AddressId]

			  ,[m].[Id]
			  ,[m].[Url]

			  ,[cc].[Id]
			  ,[cc].[UserId]
			  ,[cc].[ExternalCardIdNonce]
			  ,[cc].[Last4DigitsCC]
			  ,[cc].[CardType]

			  ,asp.[Email]
			  ,asp.[PhoneNumber]


		FROM [dbo].[Jobs] as j
		left join [dbo].[UserProfiles] as u
		on j.UserId = u.UserId

		left join [dbo].[Website] as wb
		on wb.Id = j.WebsiteId

		left join [dbo].[Media] as m
		on wb.MediaId = m.Id

		left join [dbo].[UserCreditCards] as cc
		on u.UserId = cc.UserId

		left join [dbo].[AspNetUsers] as asp
		on asp.Id = u.UserId

		WHERE j.[Id] = @Id

		SELECT
			   w.[Id]
			  ,w.[JobId]
			  ,w.[AddressId]
			  ,w.[SuiteNo]
			  ,w.[ContactName]
			  ,w.[Phone]
			  ,w.[SpecialInstructions]
			  ,w.[ServiceNote]
			  ,w.[Created]
			  ,w.[Modified]
			  ,w.[ExternalWaypointId]
			  ,w.[ExternalCustomerId]

			  ,a.[AddressId]
			  ,a.[DateCreated]
			  ,a.[DateModified]
			  ,a.[UserId]
			  ,a.[Name]
			  ,a.[ExternalPlaceId]
			  ,a.[Line1]
			  ,a.[Line2]
			  ,a.[City]
			  ,a.[State]
			  ,a.[StateId]
			  ,a.[ZipCode]
			  ,a.[Latitude]
			  ,a.[Longitude]

		FROM [dbo].[JobWaypoints] as w
		left join [dbo].[Address] as a
		on w.AddressId = a.AddressId
		WHERE JobId = @Id
		
		SELECT
			   i.[Id]
			  ,i.[JobId]
			  ,i.[WaypointId]
			  ,i.[ItemTypeId]
			  ,i.[ItemNote]
			  ,i.[Quantity]
			  ,i.[MediaId]
			  ,i.[Created]
			  ,i.[Modified]
			  ,i.[Operation]
			  ,i.[ParentItemId]


			  ,[jio].[Id]
			  ,[jio].[DateCreated]
			  ,[jio].[DateModified]
			  ,[jio].[Name]
			  ,[jio].[Weight]
			  ,[jio].[WebsiteId]
			  ,[jio].[MediaId]
			  ,[jio].[WeightMax]

			  ,[jim].[Id]
			  ,[jim].[Url]
			  ,[jim].[MediaType]
			  ,[jim].[UserId]
			  ,[jim].[Title]
			  ,[jim].[Description]

			  ,m.[Created]
			  ,m.[Modified]
			  ,m.[Url]
			  ,m.[MediaType]
			  ,m.[UserId]
			  ,m.[Title]
			  ,m.[Description]
			  ,m.[ExternalMediaId]
			  ,m.[FileType]

		FROM
		[dbo].[JobWaypointsItems] as i
		left join [dbo].[JobItemOptions] as jio
		on jio.Id = i.ItemTypeId 

		left join [dbo].[Media] as jim
		on jim.Id = jio.MediaId

		left join [dbo].[Media] as m
		on i.MediaId = m.Id

		WHERE i.JobId = @Id

END

/* 		Test Code	
		Declare @Id int = 1677
		exec [dbo].[Jobs_SelectJobByJobId] @Id

*/