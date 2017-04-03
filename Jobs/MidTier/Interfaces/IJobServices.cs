using Bringpro.Web.Domain;
using Bringpro.Web.Models.Requests;
using Bringpro.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Services.Interfaces
{
    public interface IJobsService
    {

        List<int> SaveJob(JobOrderUpdateRequest model);

        int SaveWaypoint(WaypointRequest model);

        int SaveItem(WaypointItemPIckupRequest model, int? jobId, int waypointId);

        int insertWaypointItems(JobsItemsInsertRequest model);

        bool UpdateWaypointItems(JobItemsUpdateRequest model);

        PaginatedItemsResponse<Job> GetAllJobsWithFilter(PaginatedRequest model);

        Job GetJobById(int Id);

        List<JobWaypointItem> GetAllJobsItems();

        JobWaypointItem GetJobsItemsById(int Id);

        int GetWebsiteIdByJobId(int JobId);

        string GetUserIdByJobId(int JobId);

        int InsertJob(JobInsertRequest model);

        bool UpdateJob(JobUpdateRequest model);

        bool DeleteJob(JobDeleteRequest model);

        bool DeleteJobItem(int ItemId);

        void BringgCreateTask(int JobId);

        void UpdateExternalJobId(int JobId, int ExternalJobId);
    }
}