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

        [Dependency("CreateTaskWithWaypoints")]
        public IBringgTask<Job> _CreateTaskWithWaypoints { get; set; }

        [Dependency("CreateWaypoint")]
        public IBringgTask<JobWaypoint> _CreateWaypoint { get; set; }

        [Dependency]
        public IUserAddressService _UserAddressService { get; set; }

        [Dependency]
        public IWebsiteTeamService _WebsiteTeamService { get; set; }



        public List<int> SaveJob(JobOrderUpdateRequest model)
        {
            List<int> list = new List<int>(); // create a list of WaypointIds, which are ints
            foreach (var waypoint in model.Waypoints) // goes through every Waypoint submitted in the payload
            {
                int waypointId = SaveWaypoint(waypoint); // Notice how this gets a waypointId after calling Savewaypoint()
                list.Add(waypointId); // This line gets exeuted after calling SaveWaypoint(), which calls another function SaveItem()

            }

            return list; // this line executes last in this 'upsert' function

        }

        public int SaveWaypoint(WaypointRequest model)
        {
            // we call this so we can attach an AddressId to each Waypoint in WaypointInsert() or WaypointUpdate()
            int addressId = _AddressService.SaveWaypointAddress(model);

            if (model.Id == 0) // insert Waypoint
            {
                int waypointId = _JobsWaypointService.WaypointInsert(model, addressId);

                List<int> PickupList = new List<int>(); // every Waypoint has a list of items
                foreach (var items in model.JobWaypointItemsPickup) //loops through all items in the model
                {
                    int itemId = SaveItem(items, model.JobId, waypointId); // see comment directly above SaveItem() that explains why we needs this parameters passed in
                    PickupList.Add(itemId);
                }
                List<int> DropoffList = new List<int>(); // every Waypoint has a list of items
                foreach (var items in model.JobWaypointItemsDropOff) //loops through all items in the model
                {
                    int itemId = SaveItem(items, model.JobId, waypointId); // see comment directly above SaveItem() that explains why we needs this parameters passed in
                    DropoffList.Add(itemId);
                }

                return waypointId;
            }
            else // update Waypoint
            {
                _JobsWaypointService.WaypointUpdate(model, addressId);

                int waypointId = model.Id; // we need this to be returned for the list of Waypoints

                List<int> PickupList = new List<int>(); // every Waypoint, even an updated one, needs a new list because we loop through each waypoint's items after inserting or updating a waypoint
                foreach (var items in model.JobWaypointItemsPickup)
                {
                    int itemId = SaveItem(items, model.JobId, waypointId);
                    PickupList.Add(itemId);
                }
                List<int> DropoffList = new List<int>(); // every Waypoint, even an updated one, needs a new list because we loop through each waypoint's items after inserting or updating a waypoint
                foreach (var items in model.JobWaypointItemsDropOff)
                {
                    int itemId = SaveItem(items, model.JobId, waypointId);
                    DropoffList.Add(itemId);
                }

                return waypointId;
            }
        }

        // We need JobId and waypointId in order to put them in database columns for each Item.
        // This will make it organized for us to extract the approriate items for each Waypoint during the Get. 
        public int SaveItem(WaypointItemPIckupRequest model, int? jobId, int waypointId)
        {
            int waypointItemId = 0;

            // This if statement assigns a default MediaId to an item that does not have an user image attached. The default MediaIds establish default stock images.
            if (model.MediaId == null && model.ItemTypeId > 0)
            {
                JobItemOptionsDomain JobItemOptions = _JobItemOptionsService.getIdJobItemOptions(model.ItemTypeId);
                model.MediaId = JobItemOptions.MediaId;
            }
            else
            {
                model.MediaId = model.MediaId;
            }

            if (model.Id == 0) // insert Item into database
            {
                _JobsWaypointService.WaypointItemInsert(model, jobId, waypointId); // this function returns waypointItemId
                return waypointItemId;
            }
            else  // update Item in database
            {
                int wayPointItemId = 0;
                _JobsWaypointService.WaypointItemUpdate(model, jobId, waypointId);
                wayPointItemId = model.Id; // this line is needed because SaveItem() needs to return waypointItemId, so it can be added to a list of items in each waypoint 
                return wayPointItemId;
            }
        }
        ///////// The above code -- SaveJob(), SaveWaypoint(), SaveItem() -- make up the 'upsert' that creates the massive job object



        public Job GetJobById(int Id)
        {

            Job Job = null;
            List<JobWaypoint> Waypoints = null;
            List<JobWaypointItem> Items = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Jobs_SelectJobByJobId"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@Id", Id);

              }, map: delegate (IDataReader reader, short set)
              {
                  if (set == 0)
                  {
                      int startingIndex = 0;
                      Job = new Job();
                      Job.Id = reader.GetSafeInt32(startingIndex++);
                      Job.UserId = reader.GetSafeString(startingIndex++);
                      Job.JobStatus = reader.GetSafeEnum<JobStatus>(startingIndex++);
                      Job.JobType = reader.GetSafeEnum<JobsType>(startingIndex++);
                      Job.Price = reader.GetSafeDecimal(startingIndex++);
                      Job.JobRequest = reader.GetSafeInt32(startingIndex++);
                      Job.ContactName = reader.GetSafeString(startingIndex++);
                      Job.Phone = reader.GetSafeString(startingIndex++);
                      Job.SpecialInstructions = reader.GetSafeString(startingIndex++);
                      Job.Created = reader.GetSafeDateTime(startingIndex++);
                      Job.Modified = reader.GetSafeDateTime(startingIndex++);
                      Job.WebsiteId = reader.GetSafeInt32(startingIndex++);
                      Job.ExternalJobId = reader.GetSafeInt32(startingIndex++);
                      Job.ExternalCustomerId = reader.GetSafeInt32(startingIndex++);


                      Job.User = new UserProfile();
                      Job.User.UserId = reader.GetSafeString(startingIndex++);
                      Job.User.FirstName = reader.GetSafeString(startingIndex++);
                      Job.User.LastName = reader.GetSafeString(startingIndex++);
                      Job.User.ExternalUserId = reader.GetSafeString(startingIndex++);
                      Job.User.DateCreated = reader.GetSafeDateTime(startingIndex++);
                      Job.User.DateModified = reader.GetSafeDateTime(startingIndex++);
                      Job.User.MediaId = reader.GetSafeInt32(startingIndex++);
                      Job.User.TokenHash = reader.GetSafeGuid(startingIndex++).ToString(); // Unable to cast object of type 'System.Guid' to type 'System.String

                      Job.Website = new Website();
                      Job.Website.Id = reader.GetSafeInt32(startingIndex++);
                      Job.Website.Name = reader.GetSafeString(startingIndex++);
                      Job.Website.Slug = reader.GetSafeString(startingIndex++);
                      Job.Website.Description = reader.GetSafeString(startingIndex++);
                      Job.Website.Url = reader.GetSafeString(startingIndex++);
                      Job.Website.MediaId = reader.GetSafeInt32Nullable(startingIndex++);
                      Job.Website.DateCreated = reader.GetSafeUtcDateTime(startingIndex++);
                      Job.Website.DateModified = reader.GetSafeDateTime(startingIndex++);
                      Job.Website.Phone = reader.GetSafeString(startingIndex++);
                      Job.Website.AddressId = reader.GetSafeInt32Nullable(startingIndex++);

                      Job.Website.Media = new Media();
                      Job.Website.Media.Id = reader.GetSafeInt32(startingIndex++);
                      Job.Website.Media.Url = reader.GetSafeString(startingIndex++);

                      Job.User.CreditCards = new UserCreditCards();
                      Job.User.CreditCards.Id = reader.GetSafeInt32(startingIndex++);
                      Job.User.CreditCards.UserId = reader.GetSafeString(startingIndex++);
                      Job.User.CreditCards.ExternalCardIdNonce = reader.GetSafeString(startingIndex++);
                      Job.User.CreditCards.Last4DigitsCC = reader.GetSafeString(startingIndex++);
                      Job.User.CreditCards.CardType = reader.GetSafeString(startingIndex++);

                      Job.User.Email = reader.GetSafeString(startingIndex++);
                      Job.User.Phone = reader.GetSafeString(startingIndex++);

                  }
                  else if (set == 1)
                  {
                      int startingIndex = 0;

                      JobWaypoint Waypoint = new JobWaypoint();

                      Waypoint.Id = reader.GetSafeInt32(startingIndex++);
                      Waypoint.JobId = reader.GetSafeInt32(startingIndex++);
                      Waypoint.AddressId = reader.GetSafeInt32(startingIndex++);
                      Waypoint.SuiteNo = reader.GetSafeString(startingIndex++);
                      Waypoint.ContactName = reader.GetSafeString(startingIndex++);
                      Waypoint.Phone = reader.GetSafeString(startingIndex++);
                      Waypoint.SpecialInstructions = reader.GetSafeString(startingIndex++);
                      Waypoint.ServiceNote = reader.GetSafeString(startingIndex++);
                      Waypoint.Created = reader.GetSafeDateTime(startingIndex++);
                      Waypoint.Modified = reader.GetSafeDateTime(startingIndex++);
                      Waypoint.ExternalWaypointId = reader.GetSafeInt32(startingIndex++);
                      Waypoint.ExternalCustomerId = reader.GetSafeInt32(startingIndex++);

                      Waypoint.Address = new Address();
                      Waypoint.Address.AddressId = reader.GetSafeInt32(startingIndex++);
                      Waypoint.Address.DateCreated = reader.GetSafeDateTime(startingIndex++);
                      Waypoint.Address.DateModified = reader.GetSafeDateTime(startingIndex++);
                      Waypoint.Address.UserId = reader.GetSafeString(startingIndex++);
                      Waypoint.Address.Name = reader.GetSafeString(startingIndex++);
                      Waypoint.Address.ExternalPlaceId = reader.GetSafeString(startingIndex++);
                      Waypoint.Address.Line1 = reader.GetSafeString(startingIndex++);
                      Waypoint.Address.Line2 = reader.GetSafeString(startingIndex++);
                      Waypoint.Address.City = reader.GetSafeString(startingIndex++);
                      Waypoint.Address.State = reader.GetSafeString(startingIndex++);
                      Waypoint.Address.StateId = reader.GetSafeInt32(startingIndex++);
                      Waypoint.Address.ZipCode = reader.GetSafeInt32(startingIndex++);
                      Waypoint.Address.Latitude = reader.GetSafeDecimal(startingIndex++);
                      Waypoint.Address.Longitude = reader.GetSafeDecimal(startingIndex++);


                      if (Waypoints == null)
                      {
                          Waypoints = new List<JobWaypoint>();
                      }
                      Waypoints.Add(Waypoint);

                  }
                  else if (set == 2)
                  {
                      int startingIndex = 0;
                      JobWaypointItem Item = new JobWaypointItem();

                      Item.Id = reader.GetSafeInt32(startingIndex++);
                      Item.JobId = reader.GetSafeInt32(startingIndex++);
                      Item.WaypointId = reader.GetSafeInt32(startingIndex++);
                      Item.ItemTypeId = reader.GetSafeInt32(startingIndex++);
                      Item.ItemNote = reader.GetSafeString(startingIndex++);
                      Item.Quantity = reader.GetSafeInt32(startingIndex++);
                      Item.MediaId = reader.GetSafeInt32(startingIndex++);
                      Item.Created = reader.GetSafeDateTime(startingIndex++);
                      Item.Modified = reader.GetSafeDateTime(startingIndex++);
                      Item.Operation = reader.GetSafeInt32(startingIndex++);
                      Item.ParentItemId = reader.GetSafeInt32(startingIndex++);


                      Item.JobItem = new JobItemOptionsDomain();
                      Item.JobItem.Id = reader.GetSafeInt32(startingIndex++);
                      Item.JobItem.dateCreated = reader.GetSafeDateTime(startingIndex++);
                      Item.JobItem.dateModified = reader.GetSafeDateTime(startingIndex++);
                      Item.JobItem.Name = reader.GetSafeString(startingIndex++);
                      Item.JobItem.MinTime = reader.GetSafeInt32(startingIndex++);
                      Item.JobItem.websiteId = reader.GetSafeInt32(startingIndex++);
                      Item.JobItem.MediaId = reader.GetSafeInt32(startingIndex++);
                      Item.JobItem.MaxTime = reader.GetSafeInt32(startingIndex++);

                      Item.JobItem.Media = new Media();
                      Item.JobItem.Media.Id = reader.GetSafeInt32(startingIndex++);
                      Item.JobItem.Media.Url = reader.GetSafeString(startingIndex++);
                      Item.JobItem.Media.MediaType = reader.GetSafeInt32(startingIndex++);
                      Item.JobItem.Media.UserId = reader.GetSafeString(startingIndex++);
                      Item.JobItem.Media.Title = reader.GetSafeString(startingIndex++);
                      Item.JobItem.Media.Description = reader.GetSafeString(startingIndex++);

                      if (Item.MediaId != 0 && Item.MediaId != 1)
                      {
                          Item.Media = new Media();
                          Item.Media.DateModified = reader.GetSafeDateTime(startingIndex++);
                          Item.Media.DateCreated = reader.GetSafeDateTime(startingIndex++);
                          Item.Media.Url = reader.GetSafeString(startingIndex++);
                          Item.Media.MediaType = reader.GetSafeInt32(startingIndex++);
                          Item.Media.UserId = reader.GetSafeString(startingIndex++);
                          Item.Media.Title = reader.GetSafeString(startingIndex++);
                          Item.Media.Description = reader.GetSafeString(startingIndex++);
                          Item.Media.ExternalMediaId = reader.GetSafeInt32(startingIndex++);
                          Item.Media.FileType = reader.GetSafeString(startingIndex++);
                      }

                      if (Items == null)
                      {
                          Items = new List<JobWaypointItem>();
                      }
                      Items.Add(Item);

                  }
              }
              );

            if (Waypoints != null)
            {

                foreach (var waypoint in Waypoints) // we loop through every Waypoint in our list we created in the beginning of our Get...
                {
                    foreach (var item in Items) // ... at the same time, we loop through every Item in our list we created in the beginning of our Get...
                    {
                        if (item.WaypointId == waypoint.Id) // ... and when an Item has a WaypointId that matches an Id in Waypoint...
                        {
                            if (waypoint.JobWaypointItemsPickup == null) // (null check, then instantiate a list to put Items in -- this goes in JobWaypoint Domain)
                            {
                                waypoint.JobWaypointItemsPickup = new List<JobWaypointItem>();
                            }
                            if (waypoint.JobWaypointItemsDropOff == null)
                            {
                                waypoint.JobWaypointItemsDropOff = new List<JobWaypointItem>();
                            }
                            if (item.Operation == 1) // ... if Item is for pick up, then ...
                            {
                                waypoint.JobWaypointItemsPickup.Add(item); // ... we add the particular Item to this list.
                            }
                            else // ... if Item is for drop off, then ...
                            {
                                waypoint.JobWaypointItemsDropOff.Add(item); // ... we add the particular Item to this list.
                            }
                        }
                    }
                }
                Job.JobWaypoints = Waypoints; //Job Domain already has a key value for a list of Waypoints -- we set that to our list that we created in beginning of this Get service.


                //this foreach loop determines what items belong are for pick-up vs. drop off
                foreach (var item in Items)
                {
                    bool found = false;
                    foreach (var itemAgain in Items)
                    {
                        if (item.Id != itemAgain.Id && item.Operation == 2 || item.Id == itemAgain.ParentItemId)
                        {
                            found = true;

                            break;
                        }
                    }

                    if (!found)
                    {
                        if (Job.ItemsToBeDroppedOff == null)
                        {
                            Job.ItemsToBeDroppedOff = new List<JobWaypointItem>();
                        }
                        Job.ItemsToBeDroppedOff.Add(item);
                    }

                }
            }

            //this assigns the team per zip code
            if (Waypoints != null)
            {
                if (Job.JobWaypoints[0].Address.ZipCode != 0)
                {
                    int TeamId = 0;
                    string queryZipCode = (Job.JobWaypoints[0].Address.ZipCode).ToString();
                    TeamId = _WebsiteTeamService.GetTeamIdByZipcode(queryZipCode);
                    Job.TeamId = TeamId;

                }
            }
            return Job;
        }
    }
}
/* Note to GitHub Viewer: If you have gotten this fair, congrats. Many classes have been taken out of this file,
 * so I could showcase the JobFlow process */