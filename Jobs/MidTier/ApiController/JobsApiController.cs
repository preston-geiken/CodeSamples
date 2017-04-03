using Microsoft.Practices.Unity;
using Bringpro.Web.Domain;
using Bringpro.Web.Enums;
using Bringpro.Web.Models.Requests;
using Bringpro.Web.Models.Responses;
using Bringpro.Web.Services;
using Bringpro.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bringpro.Web.Controllers.Api
{
    [RoutePrefix("api/jobs")]
    public class JobsApiController : ApiController

    {
        [Dependency]
        public IJobsService _JobsService { get; set; }
        [Dependency]

        public IUserProfileService _UserProfileService { get; set; }
        [Dependency]

        public IActivityLogService _ActivityLogService { get; set; }
        [Dependency]

        public IAdminJobScheduleService _ScheduleService { get; set; }

        [Dependency]
        public IBrainTreeService _BrainTreeService { get; set; }

 
        [Route("{jobId:int}"), HttpGet]
        public HttpResponseMessage GetJobById(int jobId)
        {

            ItemResponse<Job> response = new ItemResponse<Job>();

            Job Job = _JobsService.GetJobById(jobId);

            response.Item = Job;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route("waypoints"), HttpPost]
        public HttpResponseMessage JobWaypointInsert([FromBody] JobOrderUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
       
            int JobScheduleId = 0;

            JobSchedule Sched = new JobSchedule();
            Sched.JobId = model.JobId;
            Sched.ScheduleId = model.ScheduleId;
            Sched.Date = model.Date;
            if (model.Date != null && model.JobId != null)
            {
                try
                {
                    JobScheduleId = _ScheduleService.JobScheduleUpsert(Sched);
                }
                catch (System.Exception ex)
                {

                    throw (ex);
                }
            }
            JobScheduleItemsResponse<int> response = new JobScheduleItemsResponse<int>();

            response.Items = _JobsService.SaveJob(model);

            response.JobScheduleId = JobScheduleId;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        //-----------------------------------------------------------------------------
        [Route("activity/{jobId:int}"), HttpGet]
        public HttpResponseMessage GetActivityByJobId(int jobId)
        {
            ItemsResponse<ActivityLog> response = new ItemsResponse<ActivityLog>();

            string gotUserId = UserService.GetCurrentUserId();

            if (gotUserId != null)
            {
                List<ActivityLog> list = _ActivityLogService.GetByJobId(jobId);
                response.Items = list;
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("create"), HttpPost]
        public HttpResponseMessage JobInsert(JobInsertRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();
            UserProfile user = null;
            // Check to see if user is logged use userservice.isloggedin
            if (UserService.IsLoggedIn())
            {               
                user = _UserProfileService.GetUserById(UserService.GetCurrentUserId());

                model.UserId = user.UserId;
                model.Phone = user.Phone;

                if(user.ExternalUserId != null )
                {
                    int extId = 0;

                    int.TryParse(user.ExternalUserId, out extId);

                    if(extId > 0)
                    {
                        model.ExternalCustomerId = extId;
                    }
                    else
                    {
                        //  TODO: get user by phone number from bringg, grab their id, update userProfile table so we have it for this user
                        //  if they do not exist at bringg we must create them, link their acct to externalCustomerId in our db and use that customer id 
                    }
                }
            }
            //we need an email

            int tempJobId = _JobsService.InsertJob(model);

            response.Item = tempJobId;

            //Activity Log Service


            if (user != null && user.UserId != null)
            {
                ActivityLogRequest Activity = new ActivityLogRequest();

                Activity.ActivityType = ActivityTypeId.CreatedJob;

                Activity.JobId = tempJobId;
                Activity.TargetValue = (int)JobStatus.BringgCreated;

                _ActivityLogService.InsertActivityToLog(model.UserId, Activity);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage JobEdit(JobUpdateRequest model, int id)
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            model.Id = id;

            //JobsService jobService = new JobsService();
            //can you do a get and update the payment nonce in here - you will have the job ID

            bool isSuccessful = _JobsService.UpdateJob(model);

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = isSuccessful;

            //Activity Log Service - Needs to be fixed - the value for model.UserId is null
            ActivityLogRequest Activity = new ActivityLogRequest();

            Activity.ActivityType = ActivityTypeId.JobUpdated;
            Activity.JobId = id;
            Activity.TargetValue = (int)JobStatus.BringgCreated;

            _ActivityLogService.InsertActivityToLog(model.UserId, Activity);

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route("update/{id:int}"), HttpPut]
        public HttpResponseMessage PaymentNonce(JobUpdateRequest model, int id)
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            model.Id = id;
            bool isSuccessful = JobsService.UpdatePaymentNonce(model);

            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = isSuccessful;
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("guestcheckout"), HttpPost]
        public HttpResponseMessage GuestPayment(CustomerPaymentRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            string paymentNonce;

            try
            {
                paymentNonce = _BrainTreeService.GuestPayment(model);
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

  
            JobUpdateRequest jobUpdate = new JobUpdateRequest
            {
                Id = model.JobId,
                Phone = model.Phone,
                ContactName = string.Format("{0} {1}", model.FirstName, model.LastName),
                PaymentNonce = paymentNonce
            };


            ActivityLogRequest Activity = new ActivityLogRequest
            {
                ActivityType = ActivityTypeId.CreatedJob,
                JobId = model.JobId,
                TargetValue = (int)JobStatus.BringgCreated
            };

            //For guest check out we use the phone number as the userID
            _ActivityLogService.InsertActivityToLog(model.Phone, Activity);

            bool isSuccessful = _JobsService.UpdateJob(jobUpdate);


            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = isSuccessful;



            return Request.CreateResponse(HttpStatusCode.OK, response);

        }



        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage JobDelete(int Id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            JobDeleteRequest model = new JobDeleteRequest();

            model.Id = Id;

            //JobsService jobService = new JobsService();
            Job JobList = new Job();
            JobList = _JobsService.GetJobById(Id);

            bool isSuccessful = _JobsService.DeleteJob(model);

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = isSuccessful;

            //Activity Log Service

            ActivityLogRequest Activity = new ActivityLogRequest();

            Activity.ActivityType = ActivityTypeId.JobDeleted;
            Activity.JobId = Id;

            _ActivityLogService.InsertActivityToLog(JobList.UserId, Activity);

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("submit/{JobId:int}"), HttpPost]
        public HttpResponseMessage SubmitJob(int JobId)

        {
            _JobsService.BringgCreateTask(JobId);

            ItemResponse<int> response = new ItemResponse<int>();
            response.Item = JobId;
            return Request.CreateResponse(response);
        }

        [Route("items/{ItemId:int}"), HttpDelete]
        public HttpResponseMessage Delete(int ItemId)
        {
            SuccessResponse response = new SuccessResponse();
            _JobsService.DeleteJobItem(ItemId);
            return Request.CreateResponse(response);
        }

    }
}





