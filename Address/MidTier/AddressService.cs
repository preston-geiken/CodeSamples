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
                AddressUpdateRequest updateAddress = new AddressUpdateRequest();

                updateAddress.AddressId = CurrentAddress.AddressId;
                updateAddress.Name = model.ContactName;
                updateAddress.ExternalPlaceId = model.Address.ExternalPlaceId;
                updateAddress.Line1 = model.Address.Line1;
                updateAddress.City = model.Address.City;
                updateAddress.State = model.Address.State;
                updateAddress.ZipCode = model.Address.ZipCode;

                if(model.Address.Latitude != null && model.Address.Latitude > 0)
                {
                    updateAddress.Latitude = model.Address.Latitude;
                }
                if (model.Address.Longitude != null && model.Address.Longitude > 0)
                {
                    updateAddress.Longitude = model.Address.Longitude;
                }       

                Update(updateAddress);
                addressId = CurrentAddress.AddressId;

            }

            return addressId;
        }

        public int SaveWebsiteAddress(WebsiteAddRequest model)
        {
            var addressId = model.AddressId;
            AddressAddRequest newAddress = new AddressAddRequest();
            AddressUpdateRequest updateAddress = new AddressUpdateRequest();
            
            if (addressId > 0)
            {
                updateAddress.Name = model.Name;
                updateAddress.AddressId = model.AddressId;
                updateAddress.ExternalPlaceId = model.ExternalAddressId;
                updateAddress.Line1 = model.Street;
                updateAddress.City = model.City;
                updateAddress.State = model.State;
                updateAddress.ZipCode = model.ZipCode;
                updateAddress.Country = model.Country;

                Update(updateAddress);
             
            }
            else
            {
                newAddress.Name = model.Name;
                newAddress.ExternalPlaceId = model.ExternalAddressId;
                newAddress.Line1 = model.Street;
                newAddress.City = model.City;
                newAddress.State = model.State;
                newAddress.ZipCode = model.ZipCode;
                newAddress.Country = model.Country;

                addressId = Insert(newAddress);
            }

            return addressId;
        }

        public Address GetByExternalAddressId (string ExternalPlaceId)
        {
            Address SingleAddress = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Address_SelectByExternalId"
             , inputParamMapper: delegate (SqlParameterCollection paramCollection)
             {
                 paramCollection.AddWithValue("@ExternalPlaceId", ExternalPlaceId);
             }

             , map: delegate (IDataReader reader, short set)
             {
                 int startingIndex = 0;
                 SingleAddress = new Address();

                 SingleAddress.AddressId = reader.GetSafeInt32(startingIndex++);
                 SingleAddress.DateCreated = reader.GetSafeDateTime(startingIndex++);
                 SingleAddress.DateModified = reader.GetSafeDateTime(startingIndex++);
                 SingleAddress.UserId = reader.GetSafeString(startingIndex++);
                 SingleAddress.Name = reader.GetSafeString(startingIndex++);
                 SingleAddress.ExternalPlaceId = reader.GetSafeString(startingIndex++);
                 SingleAddress.Line1 = reader.GetSafeString(startingIndex++);
                 SingleAddress.Line2 = reader.GetSafeString(startingIndex++);
                 SingleAddress.City = reader.GetSafeString(startingIndex++);
                 SingleAddress.State = reader.GetSafeString(startingIndex++);
                 SingleAddress.StateId = reader.GetSafeInt32(startingIndex++);
                 SingleAddress.ZipCode = reader.GetSafeInt32(startingIndex++);
                 SingleAddress.Latitude = reader.GetSafeDecimal(startingIndex++);
                 SingleAddress.Longitude = reader.GetSafeDecimal(startingIndex++);
                 SingleAddress.Country = reader.GetSafeString(startingIndex++);
             });

            return SingleAddress;

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