using Microsoft.Practices.Unity;
using Bringpro.Data;
using Bringpro.Web.Classes.Tasks.Bringg.Interfaces;
using Bringpro.Web.Domain;
using Bringpro.Web.Enums;
using Bringpro.Web.Models.Requests;
using Bringpro.Web.Models.Responses;
using Bringpro.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//Note to GitHub Viewers: Many lines were removed from this file in order to showcase Bringg intergration
namespace Bringpro.Web.Services
{

    public class JobsService : BaseService, IJobsService

    {
        [Dependency]
        public IAddressService _AddressService { get; set; }
        [Dependency]
        public IJobsWaypointService _JobsWaypointService { get; set; }
        [Dependency]
        public IJobItemOptionsService _JobItemOptionsService { get; set; }
        [Dependency("CreateTask")]
        public IBringgTask<Job> _CreateTask { get; set; }
        [Dependency("CreateWaypoint")]
        public IBringgTask<JobWaypoint> _CreateWaypoint { get; set; }
        [Dependency]
        public IUserAddressService _UserAddressService { get; set; }
        [Dependency]
        public IWebsiteTeamService _WebsiteTeamService { get; set; }

  
        public void BringgCreateTask(int JobId)
        {
            // the next three lines of code submits a task (aka job) to Bringg
            Job Job = GetJobById(JobId);
            //CreateTask<Job> CreateTask = new CreateTask<Job>();
            _CreateTask.Execute(Job);

            // after creating a task in Bringg and getting back an ExternalJobId, we call GetJobById because it has the updated ExternalJobId
            Job = GetJobById(JobId); // job.userid != null call my function

            if (Job.UserId != null)
            {
                // pass in job to UserAddressService process job address function 
                _UserAddressService.ProcessJobAddresses(Job, Job.UserId);
            }
            /* now that the taks/job has been created in Bringg (along with one Waypoint), we want add all additional Waypoints to Bringg
               we do not get the first waypoint because it was already sent to bring in CreateTask.Execute(Job) */
            try
            {
                int counter = 0;
                foreach (var waypoint in Job.JobWaypoints)
                {
                    if (counter > 0)
                    {
                        waypoint.ExternalCustomerId = Job.ExternalCustomerId;
                        waypoint.ExternalJobId = Job.ExternalJobId;
                        _CreateWaypoint.Execute(waypoint);
                    }
                    counter++;
                }
                UpdateJobStatus(JobStatus.BringgCreated, Job.ExternalJobId);
            }
            catch(Exception)
            {
                UpdateJobStatus(JobStatus.BringgError, Job.ExternalJobId);
            }
        }
    }
}