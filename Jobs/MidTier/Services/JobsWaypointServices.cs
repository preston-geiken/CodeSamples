﻿using Bringpro.Data;
using Bringpro.Web.Domain;
using Bringpro.Web.Models.Requests;
using Bringpro.Web.Models.Responses;
using Bringpro.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Services
{
    public class JobsWaypointService : BaseService, IJobsWaypointService
    {
        public int WaypointInsert(WaypointRequest model, int addressId)
        {
            int id = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Jobs_WaypointInsert"
                            , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                            {
                                paramCollection.AddWithValue("@JobId", model.JobId);
                                paramCollection.AddWithValue("@AddressId", addressId);
                                paramCollection.AddWithValue("@SuiteNo", model.SuiteNo);
                                paramCollection.AddWithValue("@ContactName", model.ContactName);
                                paramCollection.AddWithValue("@Phone", model.Phone);
                                paramCollection.AddWithValue("@SpecialInstructions", model.SpecialInstructions);
                                paramCollection.AddWithValue("@ServiceNote", model.ServiceNote);
                                paramCollection.AddWithValue("@ExternalCustomerId", model.ExternalCustomerId);


                                SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                                p.Direction = System.Data.ParameterDirection.Output;

                                paramCollection.Add(p);

                            }, returnParameters: delegate (SqlParameterCollection param)
                            {
                                int.TryParse(param["@Id"].Value.ToString(), out id);
                            });
            return id;
        }

        public void WaypointUpdate(WaypointRequest model, int addressId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Jobs_WaypointsUpdate"
                   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {
                       paramCollection.AddWithValue("@Id", model.Id);
                       paramCollection.AddWithValue("@JobId", model.JobId);
                       paramCollection.AddWithValue("@AddressId", addressId);
                       paramCollection.AddWithValue("@SuiteNo", model.SuiteNo);
                       paramCollection.AddWithValue("@ContactName", model.ContactName);
                       paramCollection.AddWithValue("@Phone", model.Phone);
                       paramCollection.AddWithValue("@SpecialInstructions", model.SpecialInstructions);
                       paramCollection.AddWithValue("@ServiceNote", model.ServiceNote);
                   });

        }

        public int WaypointItemInsert(WaypointItemPickupRequest model, int? jobId, int waypointId)
        {
            int waypointItemId = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Jobs_WaypointItemsInsert"
          , inputParamMapper: delegate (SqlParameterCollection paramCollection)
          {
              paramCollection.AddWithValue("@JobId", jobId);
              paramCollection.AddWithValue("@WaypointId", waypointId);
              paramCollection.AddWithValue("@ItemTypeId", model.ItemTypeId);
              paramCollection.AddWithValue("@ItemNote", model.ItemNote);
              paramCollection.AddWithValue("@Quantity", model.Quantity);
              paramCollection.AddWithValue("@MediaId", model.MediaId);
              paramCollection.AddWithValue("@Operation", model.Operation);
              paramCollection.AddWithValue("@ParentItemId", model.ParentItemId);


              SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
              p.Direction = System.Data.ParameterDirection.Output;

              paramCollection.Add(p);

          }, returnParameters: delegate (SqlParameterCollection param)
          {
              int.TryParse(param["@Id"].Value.ToString(), out waypointItemId);
          });

            return waypointItemId;
        }

        public void WaypointItemUpdate(WaypointItemPickupRequest model, int? jobId, int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Jobs_WaypointItemsUpdate"
                        , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@Id", model.Id);
                            paramCollection.AddWithValue("@JobId", jobId);
                            paramCollection.AddWithValue("@WaypointId", id);
                            paramCollection.AddWithValue("@ItemTypeId", model.ItemTypeId);
                            paramCollection.AddWithValue("@ItemNote", model.ItemNote);
                            paramCollection.AddWithValue("@Quantity", model.Quantity);
                            paramCollection.AddWithValue("@MediaId", model.MediaId);
                            paramCollection.AddWithValue("@Operation", model.Operation);
                        });
        }
    }
}