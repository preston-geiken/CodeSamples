using Newtonsoft.Json.Linq;
using Bringpro.Web.Classes.Tasks.Bringg.Interfaces;
using Bringpro.Web.Domain;
using Bringpro.Web.Services;
using System.Collections.Generic;

namespace Bringpro.Web.Classes.Tasks.Bringg
{
    public class CreateTask<T> : BaseBringgTask<T>, IBringgTask<T>
        where T : Job
    {
        
        public override string GetRequestUrl(T request)
        {
            //end point according to developers.bringg.com
            return "tasks";
        }

        public override string GetRequestType()
        {
            //must state the method
            return "POST";
        }

        protected override Dictionary<string, object> MakeRequest(T request)
        {

            Dictionary<string, object> payload = new Dictionary<string, object>();
            

            if (request.JobWaypoints != null)
            {
                JobWaypoint wp = request.JobWaypoints[0];

                payload.Add("customer_id", request.ExternalCustomerId);
                payload.Add("address", wp.Address.Line1 + " " + wp.Address.City + ", " + wp.Address.State + " " + wp.Address.ZipCode);
                payload.Add("lat", wp.Address.Latitude.ToString());
                payload.Add("lng", wp.Address.Longitude.ToString());
            }
            return payload;
        }

        protected override void ProcessResponse(T request, Dictionary<string, object> response)
        {

            int JobId = request.Id;
            int WaypointId = request.JobWaypoints[0].Id;

            IDictionary<string, JToken> task = null;
            int ExternalJobId = 0;
            int ExternalWaypointId = 0;

            // we go into the object that is returned by Bringg, and we extract the data we want for our database
            if (response.ContainsKey("task"))
            {
                task = (JObject)response["task"];
                if (task.ContainsKey("id"))
                {
                    ExternalJobId = (int)task["id"];
                    ExternalWaypointId = (int)task["active_way_point_id"];
                }
            }

            //this changes ExternalJobId from null to this value
            JobsService.UpdateExternalJobId(JobId, ExternalJobId);
            //this is the first waypoint in the job. Other waypoints can be added with CreateWaypoint
            JobsService.UpdateExternalWaypointId(WaypointId, ExternalWaypointId); 
        }

    }
}