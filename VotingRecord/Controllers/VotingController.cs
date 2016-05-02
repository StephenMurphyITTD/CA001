// a RESTful services which provides voting records of TDs on a bill

using VotingRecords.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using System.Net.Http;
using System.Net;

namespace VotingRecords.Controllers
{
    public class RecordController : ApiController
    {
        public static CloudTable Table(string tableName)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            return table;
        }

        // Get Records for All Dail Members
        [Route("bill/All")]
        public IEnumerable<VotingRecordEntity> GetbyAll() //CloudTable table
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            TableQuery<VotingRecordEntity> query = new TableQuery<VotingRecordEntity>();
            var record = table.ExecuteQuery(query);
            return record;
        }

        // Get Records for a specific TD
        [Route("bill/Ta/{Name}/{Surname}")]
        public IEnumerable<VotingRecordEntity> GetbyTD(String Name, String Surname) 
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            string nameFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Name);
            string surnameFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Surname);
            TableQuery<VotingRecordEntity> query = new TableQuery<VotingRecordEntity>().Where(TableQuery.CombineFilters(nameFilter, TableOperators.And, surnameFilter));
            var record = table.ExecuteQuery(query);
            return record;
        }

        // Get Records for who voted what way in a party
        [Route("bill/Party/{Party:alpha}")]
        public IEnumerable<VotingRecordEntity> GetbyParty(String Party)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            TableQuery<VotingRecordEntity> query = new TableQuery<VotingRecordEntity>().Where(TableQuery.GenerateFilterCondition("Party", QueryComparisons.Equal, Party));
            var record = table.ExecuteQuery(query);
           
            return record;
        }

        // Get Records to see who voted a particular way (e.g. Ta, Nil or Absent)
        [Route("bill/Vote/{Vote:alpha}")]
        public IEnumerable<VotingRecordEntity> GetbyVote(String Vote) 
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            TableQuery<VotingRecordEntity> query = new TableQuery<VotingRecordEntity>().Where(TableQuery.GenerateFilterCondition("Vote", QueryComparisons.Equal, Vote));
            var record = table.ExecuteQuery(query);

            return record;
        }

        //Post Request
        [Route("bill/Insert")]
        [HttpPost]
        public HttpResponseMessage InsertEntry() 
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            VotingRecordEntity insertObject = new VotingRecordEntity("Jennifer", "Greene");
            insertObject.Bill = "Technological Universities Bill 2015 - Report Stage. Amendment 18";
            insertObject.Vote = "Nil";
            insertObject.Party = "Labour";
            TableOperation insertOperation = TableOperation.Insert(insertObject);
            table.Execute(insertOperation);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            return response;
        }

        //Put Request
        [Route("bill/Update")]
        [HttpPut]
        public HttpResponseMessage UpdateEntry() 
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            TableOperation retrieveOperation = TableOperation.Retrieve<VotingRecordEntity>("Jennifer", "Greene");
            TableResult retrievedResult = table.Execute(retrieveOperation);
            VotingRecordEntity updateEntity = (VotingRecordEntity)retrievedResult.Result;
            updateEntity.Vote = "Ta";
            TableOperation updateOperation = TableOperation.Replace(updateEntity);
            table.Execute(updateOperation); 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            return response;
        }

        //Delete Request
        [Route("bill/Delete")]
        [HttpDelete]
        public IHttpActionResult DeleteEntry() 
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            TableOperation retrieveOperation = TableOperation.Retrieve<VotingRecordEntity>("Jennifer", "Greene");
            TableResult retrievedResult = table.Execute(retrieveOperation);
            VotingRecordEntity deleteEntity = (VotingRecordEntity)retrievedResult.Result;
            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
            table.Execute(deleteOperation);
            return Ok();
        }
    }
}
