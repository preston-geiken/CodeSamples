using Newtonsoft.Json.Linq;
using Bringpro.Web.Classes.Tasks.Bringg.Interfaces;
using Bringpro.Web.Domain;
using Bringpro.Web.Services;
using System.Collections.Generic;

namespace Bringpro.Web.Classes.Tasks.Bringg
{
    public class CreateWaypoint<T> : BaseBringgTask<T>, IBringgTask<T>
        where T : JobWaypoint
    {
        public override string GetRequestUrl(T request)
        {
            //end point according to developers.bringg.com
            return "tasks/" + request.ExternalJobId + "/way_points";
        }

        public override string GetRequestType()
        {
            //must state the method
            return "POST";
        }

        protected override Dictionary<string, object> MakeRequest(T request)
        {
            // Create the payload object
            Dictionary<string, object> payload = new Dictionary<string, object>();

            if (request.ExternalJobId != null)
            {
                //payload.Add("task_id", request.ExternalJobId);
                payload.Add("customer_id", request.ExternalCustomerId);
                payload.Add("scheduled_at", "2017-03-27T22:47:58.000Z"); //TODO: Remove hardcoded date later on
                payload.Add("address", request.Address.Line1 + " " + request.Address.City + ", " + request.Address.State + " " + request.Address.ZipCode);
                payload.Add("lat", request.Address.Latitude.ToString());
                payload.Add("lng", request.Address.Longitude.ToString());
            }

            return payload;

        }
        protected override void ProcessResponse(T request, Dictionary<string, object> response)
        {
            int WaypointId = request.Id;

            IDictionary<string, JToken> Waypoint = null;
            int ExternalWaypointId = 0;

            // we go into the object that is returned by Bringg, and we extract the data we want for our database
            if (response.ContainsKey("way_point"))
            {
                Waypoint = (JObject)response["way_point"];
                if (Waypoint.ContainsKey("id"))
                {
                    ExternalWaypointId = (int)Waypoint["id"]; //we want the ExternalWaypointId because it will be added to dbo.JobWaypoints
                }
            }

            JobsService.UpdateExternalWaypointId(WaypointId, ExternalWaypointId);
        }
    }
}