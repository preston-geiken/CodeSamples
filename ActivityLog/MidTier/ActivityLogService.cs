using Bringpro.Data;
using Bringpro.Web.Domain;
using Bringpro.Web.Enums;
using Bringpro.Web.Models.Requests;
using Bringpro.Web.Models.Responses;
using Bringpro.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bringpro.Web.Services
{
    public class ActivityLogService : BaseService, IActivityLogService
    {

        public  PaginatedItemsResponse<ActivityLogAll> GetAllDetails(PaginatedRequest model)
        {
            List<ActivityLogAll> list = null;
            PaginatedItemsResponse<ActivityLogAll> response = null;

            DataProvider.ExecuteCmd(GetConnection, "ActivityLog_SelectAllForSearchFourTables"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)

              {
                  paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                  paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);
                  paramCollection.AddWithValue("@Selector", model.Selector);
              }

              , map: delegate (IDataReader reader, short set)
              {
                  if (set == 0)
                  {
                      ActivityLogAll a = new ActivityLogAll();
                      int startingIndex = 0; //startingOrdinal

                      a.ActivityLog.Id = reader.GetSafeInt32(startingIndex++);
                      a.ActivityLog.UserId = reader.GetSafeString(startingIndex++);
                      a.ActivityLog.ActivityTypeId = reader.GetSafeEnum<ActivityTypeId>(startingIndex++);
                      a.ActivityLog.JobId = reader.GetSafeInt32(startingIndex++);
                      a.ActivityLog.TargetValue = reader.GetSafeInt32(startingIndex++);
                      a.ActivityLog.IdCreated = reader.GetSafeDateTime(startingIndex++);

                      a.User.UserProfileId = reader.GetSafeInt32(startingIndex++);
                      a.User.FirstName = reader.GetSafeString(startingIndex++);
                      a.User.LastName = reader.GetSafeString(startingIndex++);
                      a.User.ExternalUserId = reader.GetSafeString(startingIndex++);
                      a.User.DateCreated = reader.GetSafeDateTime(startingIndex++);
                      a.User.DateModified = reader.GetSafeDateTime(startingIndex++);
                      a.User.MediaId = reader.GetSafeInt32(startingIndex++);
                      a.User.Email = reader.GetSafeString(startingIndex++);
                      a.User.Phone = reader.GetSafeString(startingIndex++);
                      a.User.Role.RoleId = reader.GetSafeString(startingIndex++);
                      a.User.Role.Name = reader.GetSafeString(startingIndex++);

                      a.Media.Url = reader.GetSafeString(startingIndex++);

                      a.Jobs.Id = reader.GetSafeInt32(startingIndex++);
                      a.Jobs.JobStatus = reader.GetSafeEnum<JobStatus>(startingIndex++);
                      a.Jobs.JobType = reader.GetSafeEnum<JobsType>(startingIndex++);
                      a.Jobs.Price = reader.GetSafeDecimal(startingIndex++);
                      a.Jobs.JobRequest = reader.GetSafeInt32(startingIndex++);
                      a.Jobs.ContactName = reader.GetSafeString(startingIndex++);
                      a.Jobs.Phone = reader.GetSafeString(startingIndex++);
                      a.Jobs.SpecialInstructions = reader.GetSafeString(startingIndex++);
                      a.Jobs.Created = reader.GetSafeDateTime(startingIndex++);
                      a.Jobs.Modified = reader.GetSafeDateTime(startingIndex++);;


                      if (list == null)
                      {
                          list = new List<ActivityLogAll>();
                      }
                      list.Add(a);
                  }
                  else if (set == 1)
                  {
                      response = new PaginatedItemsResponse<ActivityLogAll>();
                      response.TotalItems = reader.GetSafeInt32(0);
                  }
              }

            );

            response.Items = list;
            response.CurrentPage = model.CurrentPage;
            response.ItemsPerPage = model.ItemsPerPage;

            return response;
        }
    }
}