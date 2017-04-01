using Bringpro.Data;
using Bringpro.Web.Domain;
using Bringpro.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Bringpro.Web.Services.Interfaces;

namespace Bringpro.Web.Services
{
    public class AddressService : BaseService, IAddressService
    {
        public int SaveWaypointAddress (WaypointRequest model)
        {
            var addressId = 0;

            AddressAddRequest newAddress = new AddressAddRequest();

            Address CurrentAddress = GetByExternalAddressId(model.Address.ExternalPlaceId); 

            if (CurrentAddress == null)
            {

                model.Address.Name = model.ContactName;

                addressId = Insert(model.Address);                
            }
            else
            {
                AddressUpdateRequest Address = new AddressUpdateRequest();

                Address.AddressId = CurrentAddress.AddressId;
                Address.Name = model.ContactName;
                Address.ExternalPlaceId = model.Address.ExternalPlaceId;
                Address.Line1 = model.Address.Line1;
                Address.City = model.Address.City;
                Address.State = model.Address.State;
                Address.ZipCode = model.Address.ZipCode;

                if(model.Address.Latitude != null && model.Address.Latitude > 0)
                {
                    Address.Latitude = model.Address.Latitude;
                }
                if (model.Address.Longitude != null && model.Address.Longitude > 0)
                {
                    Address.Longitude = model.Address.Longitude;
                }       

                Update(Address);
                addressId = CurrentAddress.AddressId;

            }

            return addressId;
        }

        public Address GetByExternalAddressId (string ExternalPlaceId)
        {
            Address Address = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Address_SelectByExternalId"
             , inputParamMapper: delegate (SqlParameterCollection paramCollection)
             {
                 paramCollection.AddWithValue("@ExternalPlaceId", ExternalPlaceId);
             }

             , map: delegate (IDataReader reader, short set)
             {
                 int startingIndex = 0;
                 Address = new Address();

                 Address.AddressId = reader.GetSafeInt32(startingIndex++);
                 Address.DateCreated = reader.GetSafeDateTime(startingIndex++);
                 Address.DateModified = reader.GetSafeDateTime(startingIndex++);
                 Address.UserId = reader.GetSafeString(startingIndex++);
                 Address.Name = reader.GetSafeString(startingIndex++);
                 Address.ExternalPlaceId = reader.GetSafeString(startingIndex++);
                 Address.Line1 = reader.GetSafeString(startingIndex++);
                 Address.Line2 = reader.GetSafeString(startingIndex++);
                 Address.City = reader.GetSafeString(startingIndex++);
                 Address.State = reader.GetSafeString(startingIndex++);
                 Address.StateId = reader.GetSafeInt32(startingIndex++);
                 Address.ZipCode = reader.GetSafeInt32(startingIndex++);
                 Address.Latitude = reader.GetSafeDecimal(startingIndex++);
                 Address.Longitude = reader.GetSafeDecimal(startingIndex++);
                 Address.Country = reader.GetSafeString(startingIndex++);
             });

            return Address;

        }

        public int Insert(AddressAddRequest model)
        {
            int AddressId = 0;
            
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Address_Insert",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", model.UserId);
                    paramCollection.AddWithValue("@Name", model.Name);
                    paramCollection.AddWithValue("@ExternalPlaceId", model.ExternalPlaceId);
                    paramCollection.AddWithValue("@Line1", model.Line1);
                    paramCollection.AddWithValue("@Line2", model.Line2);
                    paramCollection.AddWithValue("@City", model.City);
                    paramCollection.AddWithValue("@State", model.State);
                    paramCollection.AddWithValue("@StateId", model.StateId);
                    paramCollection.AddWithValue("@ZipCode", model.ZipCode);
                    paramCollection.AddWithValue("@Country", model.Country);
                    paramCollection.AddWithValue("@Latitude", model.Latitude);
                    paramCollection.AddWithValue("@Longitude", model.Longitude);

                    SqlParameter p = new SqlParameter("@AddressId", SqlDbType.Int);
                    p.Direction = System.Data.ParameterDirection.Output;
                    paramCollection.Add(p);
                },

                returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@AddressId"].Value.ToString(), out AddressId);
                });

            return AddressId;
        }

        public void Update(AddressUpdateRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Address_Update",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", model.UserId);
                    paramCollection.AddWithValue("@Name", model.Name);
                    paramCollection.AddWithValue("@ExternalPlaceId", model.ExternalPlaceId);
                    paramCollection.AddWithValue("@Line1", model.Line1);
                    paramCollection.AddWithValue("@Line2", model.Line2);
                    paramCollection.AddWithValue("@City", model.City);
                    paramCollection.AddWithValue("@StateId", model.StateId);
                    paramCollection.AddWithValue("@ZipCode", model.ZipCode);
                    paramCollection.AddWithValue("@Country", model.Country);
                    paramCollection.AddWithValue("@AddressId", model.AddressId);
                    paramCollection.AddWithValue("@Latitude", model.Latitude);
                    paramCollection.AddWithValue("@Longitude", model.Longitude);
                });
        }

        public Address GetById(int BlogId)
        {
            Address a = new Address();

            DataProvider.ExecuteCmd(GetConnection, "dbo.Address_SelectById"
             , inputParamMapper: delegate (SqlParameterCollection paramCollection)
             {
                 paramCollection.AddWithValue("@AddressId", BlogId);
             }

             , map: delegate (IDataReader reader, short set)
             {
                 int startingIndex = 0;
                 a.AddressId = reader.GetSafeInt32(startingIndex++);
                 a.DateCreated = reader.GetSafeDateTime(startingIndex++);
                 a.DateModified = reader.GetSafeDateTime(startingIndex++);
                 a.UserId = reader.GetSafeString(startingIndex++);
                 a.Name = reader.GetSafeString(startingIndex++);
                 a.ExternalPlaceId = reader.GetSafeString(startingIndex++);
                 a.Line1 = reader.GetSafeString(startingIndex++);
                 a.Line2 = reader.GetSafeString(startingIndex++);
                 a.City = reader.GetSafeString(startingIndex++);
                 a.StateId = reader.GetSafeInt32(startingIndex++);
                 a.ZipCode = reader.GetSafeInt32(startingIndex++);
                 a.Country = reader.GetSafeString(startingIndex++);
                 a.Latitude = reader.GetSafeDecimal(startingIndex++);
                 a.Longitude = reader.GetSafeDecimal(startingIndex++);
             });

            return a;
        }

        public List<Address> GetAll()
        {
            List<Address> list = new List<Address>();

            DataProvider.ExecuteCmd(GetConnection, "dbo.Address_SelectAll"
              , inputParamMapper: null
              , map: delegate (IDataReader reader, short set)
              {
                  Address a = new Address();
                  int startingIndex = 0; //startingOrdinal

                  a.AddressId = reader.GetSafeInt32(startingIndex++);
                  a.DateCreated = reader.GetSafeDateTime(startingIndex++);
                  a.DateModified = reader.GetSafeDateTime(startingIndex++);
                  a.UserId = reader.GetSafeString(startingIndex++);
                  a.Name = reader.GetSafeString(startingIndex++);
                  a.ExternalPlaceId = reader.GetSafeString(startingIndex++);
                  a.Line1 = reader.GetSafeString(startingIndex++);
                  a.Line2 = reader.GetSafeString(startingIndex++);
                  a.City = reader.GetSafeString(startingIndex++);
                  a.StateId = reader.GetSafeInt32(startingIndex++);
                  a.ZipCode = reader.GetSafeInt32(startingIndex++);
                  a.Latitude = reader.GetSafeDecimal(startingIndex++);
                  a.Longitude = reader.GetSafeDecimal(startingIndex++);

                  if (list == null)
                  {
                      list = new List<Address>();
                  }
                  list.Add(a);
              });

            return list;
        }

        public void DeleteById(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Address_DeleteById"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@AddressId", id);
                });
        }

        public void DeleteByExternalAddressId(string externalId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Address_DeleteByExternalId"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@ExternalId", externalId);
                });
        }
    }
}