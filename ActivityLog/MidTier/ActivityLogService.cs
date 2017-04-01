using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Enums;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Web.Services
{
    public class ActivityLogService : BaseService, IActivityLogService
    {

//        [Dependency]
//        public IAdminHub _AdminHub { get; set; }  //----------------Someone needs to come and fix this... temporary bandaid fix -anna

        public  void InsertActivityToLog(ActivityLogRequest model)
        {
//This is not being used

            DataProvider.ExecuteNonQuery(GetConnection, "ActivityLog_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@ActivityTypeId", model.ActivityType);
                    paramCollection.AddWithValue("@JobId", model.JobId);
                    paramCollection.AddWithValue("@TargetValue", model.TargetValue);

//                    AdminHub admin = new AdminHub();
//                    admin.PushNotification(model.ActivityType);




                }
                );
        }

        //CODE BELOW was added to test SignalR in REST CLIENT
        public  int Insert(string userId,ActivityLogAddRequest model)
        {

            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.ActivityLog_Insert_v4"
                , inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    SqlParameter p = new SqlParameter("@Id", SqlDbType.Int);
                    p.Direction = ParameterDirection.Output;

//                    AdminHub admin = new AdminHub();

                    parameterCollection.Add(p);

                    parameterCollection.AddWithValue("@ActivityTypeId", model.ActivityType);
                    parameterCollection.AddWithValue("@JobId", model.JobId);
                    parameterCollection.AddWithValue("@TargetValue", model.TargetValue);
                    parameterCollection.AddWithValue("@RawResponse", model.RawResponse);
                    parameterCollection.AddWithValue("@UserId", userId);

                    //admin.PushNotification(model.ActivityType);



                },
                returnParameters: delegate (SqlParameterCollection param)
                {

                    int.TryParse(param["@Id"].Value.ToString(), out id);

                });


            return id;



        }


        public void InsertActivityToLog(string userId, ActivityLogRequest model)
        {

           
            DataProvider.ExecuteNonQuery(GetConnection, "ActivityLog_InsertWithId"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", userId);
                    paramCollection.AddWithValue("@ActivityTypeId", model.ActivityType);
                    paramCollection.AddWithValue("@JobId", model.JobId);
                    paramCollection.AddWithValue("@TargetValue", model.TargetValue);
//
//                    AdminHub admin = new AdminHub();
//                    admin.PushNotification(model.ActivityType, userId);

                }
                );
        }

        public  PaginatedItemsResponse<ActivityLog> GetAll(PaginatedRequest model)
        {
            List<ActivityLog> list = null;
            PaginatedItemsResponse<ActivityLog> response = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.ActivityLog_SelectAllForSearch"
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
                      ActivityLog a = new ActivityLog();
                      int startingIndex = 0; //startingOrdinal

                      a.Id = reader.GetSafeInt32(startingIndex++);
                      a.UserId = reader.GetSafeString(startingIndex++);
                      a.ActivityTypeId = reader.GetSafeEnum<ActivityTypeId>(startingIndex++);
                      a.JobId = reader.GetSafeInt32(startingIndex++);
                      a.TargetValue = reader.GetSafeInt32(startingIndex++);
                      a.IdCreated = reader.GetSafeDateTime(startingIndex++);

                      if (list == null)
                      {
                          list = new List<ActivityLog>();
                      }
                      list.Add(a);
                  }
                  else if (set == 1)
                  {
                      response = new PaginatedItemsResponse<ActivityLog>();
                      response.TotalItems = reader.GetSafeInt32(0);
                  }
              }

            );

            response.Items = list;
            response.CurrentPage = model.CurrentPage;
            response.ItemsPerPage = model.ItemsPerPage;

            return response;
        }


        public  PaginatedItemsResponse<ActivityLogAll> GetAllDetails(PaginatedRequest model)
        {
            List<ActivityLogAll> list = null;
            PaginatedItemsResponse<ActivityLogAll> response = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.ActivityLog_SelectAllDetails"
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

        //Get all By JobID

        public List<ActivityLog> GetByJobId(int id)
        {
            List<ActivityLog> list = new List<ActivityLog>();
            ActivityLog a = null;

            DataProvider.ExecuteCmd(GetConnection, "ActivityLog_SelectAllByJobId",
                inputParamMapper: delegate(SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@JobId", id);
                }, map: delegate(IDataReader reader, short set)
                {
                    a = new ActivityLog();

                    int startingIndex = 0;


                    a.Id = reader.GetInt32(startingIndex++);
                    a.UserId = reader.GetSafeString(startingIndex++);
                    a.ActivityTypeId = reader.GetSafeEnum<ActivityTypeId>(startingIndex++);
                    a.JobId = reader.GetInt32(startingIndex++);
                    a.TargetValue = reader.GetSafeInt32(startingIndex++);
                    a.IdCreated = reader.GetDateTime(startingIndex++);
                    a.RawResponse = reader.GetSafeString(startingIndex++);

                    list.Add(a);


                });

            return list;



        }




    }
}