using Bringpro.Data;
using Bringpro.Web.Domain;
using Bringpro.Web.Models.Requests;
using Bringpro.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Services
{
    public class CustomerRatingService : BaseService, ICustomerRatingService 
    {
        public int Insert(CustomerRatingAddRequest model)
        {
            int Id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CustomerRating_Insert",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", model.UserId);
                    paramCollection.AddWithValue("@Rating", model.Rating);
                    paramCollection.AddWithValue("@Note", model.Note);
                    paramCollection.AddWithValue("@JobId", model.JobId);


                    SqlParameter p = new SqlParameter("@Id", SqlDbType.Int);
                    p.Direction = System.Data.ParameterDirection.Output;
                    paramCollection.Add(p);
                },

                returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@Id"].Value.ToString(), out Id);
                });

            return Id;
        }
        public List<CustomerRating> GetAll()
        {
            List<CustomerRating> list = new List<CustomerRating>();

            DataProvider.ExecuteCmd(GetConnection, "dbo.CustomerRating_SelectAll"
              , inputParamMapper: null
              , map: delegate (IDataReader reader, short set)
              {
                  CustomerRating c = new CustomerRating();
                  int startingIndex = 0; //startingOrdinal

                  c.Id = reader.GetSafeInt32(startingIndex++);
                  c.UserId = reader.GetSafeString(startingIndex++);
                  c.DateCreated = reader.GetSafeDateTime(startingIndex++);
                  c.DateModified = reader.GetSafeDateTime(startingIndex++);
                  c.Rating = reader.GetSafeDecimal(startingIndex++);
                  c.Note = reader.GetSafeString(startingIndex++);
                  c.JobId = reader.GetSafeInt32(startingIndex++);
                  c.FirstName = reader.GetSafeString(startingIndex++);
                  c.LastName = reader.GetSafeString(startingIndex++);
                  c.ExternalUserId = reader.GetSafeString(startingIndex++);


                  if (list == null)
                  {
                      list = new List<CustomerRating>();
                  }
                  list.Add(c);
              });

            return list;
        }
    }
}