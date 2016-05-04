// a RESTful services which provides voting records of TDs on a bill
using VotingRecords.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

/// <summary>
/// Namespace in which the controller is running
/// </summary>
namespace VotingRecords.Controllers
{
    /// <summary>
    /// A publicly available controller class allowing a user access to read and write to a database
    /// </summary>
    public class RecordController : ApiController
    {
        /// <summary>
        /// A method used for creating a table client for reading and writing to a Microsoft NoSQL Database
        /// </summary>
        /// <returns>Cloud Table connection Object for use by other methods</returns>
        public static CloudTable Table()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Vote");
            return table;
        }

        /// <summary>
        /// Route: billNo/All
        /// Issuing a GET request against this URL returns a record of every member who voted or abstained from voting on this particular bill
        /// e.g. http://localhost:port/BillNo/ALL will return everybodies vote against this bill
        /// </summary>
        /// <returns>Returns a list of all records for all TDs who took part in voting process</returns>
        //Get Records for All Dail Members
        [Route("billNo/All")]
        public IEnumerable<VotingRecordEntity> GetbyAll() //CloudTable table
        {
            try
            {
                CloudTable table = Table();
                TableQuery<VotingRecordEntity> query = new TableQuery<VotingRecordEntity>();
                var record = table.ExecuteQuery(query);
                return record;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Route: BillNo/TD/{Name}/{Surname}
        /// Issuing a GET request against this URL returns a record of how a particular TD voted on this particular bill
        /// e.g. http://localhost:port/BillNo/TD/Enda/Kenny will return Enda Kennys vote against this bill
        /// </summary>
        /// <param name="Name">Name of TD</param>
        /// <param name="Surname">Surname of TD</param>
        /// <returns>Outputs voting records from the requested TD</returns>
        // Get Records for a specific TD
        [Route("billNo/TD/{Name}/{Surname}")]
        public IEnumerable<VotingRecordEntity> GetbyTD(String Name, String Surname) 
        {
            try
            {
                CloudTable table = Table();
                string nameFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Name);
                string surnameFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Surname);
                TableQuery<VotingRecordEntity> query = new TableQuery<VotingRecordEntity>().Where(TableQuery.CombineFilters(nameFilter, TableOperators.And, surnameFilter));
                var record = table.ExecuteQuery(query);
                return record;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Route: BillNo/Party/{Party}
        /// Issuing a GET request against this URL returns a record of how all TDs from a single party voted
        /// </summary>
        /// <param name="Party">Party Name</param>
        /// <returns>A list of how all party members voted on this bill</returns>
        // Get Records for who voted what way in a party
        [Route("billNo/Party/{Party:alpha}")]
        public IEnumerable<VotingRecordEntity> GetbyParty(String Party)
        {
            try
            {
                CloudTable table = Table();
                TableQuery<VotingRecordEntity> query = new TableQuery<VotingRecordEntity>().Where(TableQuery.GenerateFilterCondition("Party", QueryComparisons.Equal, Party));
                var record = table.ExecuteQuery(query);
                return record;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Route: BillNo/Vote/{Vote}
        /// Issuing a GET request against this URL returns a record of all TDs who voted similarly
        /// </summary>
        /// <param name="Vote">Ta, Nil, Absent</param>
        /// <returns>A 200 Response Confirming Action has completed and a full list of TDs who voted a particular way on a bill</returns>
        // Get Records to see who voted a particular way (e.g. Ta, Nil or Absent)
        [Route("billNo/Vote/{Vote:alpha}")]
        public IEnumerable<VotingRecordEntity> GetbyVote(String Vote) 
        {
            try
            {
                CloudTable table = Table();
                TableQuery<VotingRecordEntity> query = new TableQuery<VotingRecordEntity>().Where(TableQuery.GenerateFilterCondition("Vote", QueryComparisons.Equal, Vote));
                var record = table.ExecuteQuery(query);
                return record;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Route: BillNo/Insert/{Name}/{Surname}/{Party}/{Vote}
        /// Issuing a POST request against this URL creates a database record with a TDs name, party alliance and vote
        /// </summary>
        /// <param name="Name">Name of TD</param>
        /// <param name="Surname">Surname of TD</param>
        /// <param name="Party">Party TD belongs too</param>
        /// <param name="Vote">Ta, Nil, Absent</param>
        /// <returns>A 200 Response Confirming Action has completed</returns>
        //Post Request
        [Route("billNo/Insert/{Name}/{Surname}/{Party}/{Vote}")]
        [HttpPost]
        public IHttpActionResult InsertEntry(String Name, String Surname, String Party, String Vote) 
        {
            try
            {
                CloudTable table = Table();
                VotingRecordEntity insertEntity = new VotingRecordEntity(Name, Surname);
                insertEntity.Bill = "Technological Universities Bill 2015 - Report Stage. Amendment 18";
                insertEntity.Vote = Vote;
                insertEntity.Party = Party;
                TableOperation insertOperation = TableOperation.Insert(insertEntity);
                table.Execute(insertOperation);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Route: BillNo/Update/{Name}/{Surname}/{Vote}
        /// Issuing a PUT request against this URL will retrieve a voting record based on Name and Surname. After this it will modify the entity to change the vote and commit the change to the database
        /// </summary>
        /// <param name="Name">Name of TD</param>
        /// <param name="Surname">Surname of TD</param>
        /// <param name="Vote">Ta, Nil, Absent</param>
        /// <returns>A 200 Response Confirming Action has completed</returns>
        [Route("billNo/Update/{Name}/{Surname}/{Vote}")]
        [HttpPut]
        public IHttpActionResult UpdateEntry(String Name, String Surname, String Vote) 
        {
            try
            {
                CloudTable table = Table();
                TableOperation retrieveOperation = TableOperation.Retrieve<VotingRecordEntity>(Name, Surname);
                TableResult retrievedResult = table.Execute(retrieveOperation);
                VotingRecordEntity updateEntity = (VotingRecordEntity)retrievedResult.Result;
                updateEntity.Vote = Vote;
                TableOperation updateOperation = TableOperation.Replace(updateEntity);
                table.Execute(updateOperation);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Route: BillNo/Delete/{Name}/{Surname}}
        /// Issuing a Delete request against this URL will retrieve a voting record based on Name and Surname. After this it will delete the record entirely from the database
        /// e.g. http://localhost:port/BillNo/Stephen/Murphy/Ta will update the votes database recording this newly appended vote type
        /// </summary>
        /// <param name="Name">Name of TD</param>
        /// <param name="Surname">Surname of TD</param>
        /// <returns>A 200 Response Confirming Action has completed</returns>
        //Delete Request
        [Route("billNo/Delete/{Name}/{Surname}")]
        [HttpDelete]
        public IHttpActionResult DeleteEntry(String Name, String Surname) 
        {
            try
            {
                CloudTable table = Table();
                TableOperation retrieveOperation = TableOperation.Retrieve<VotingRecordEntity>(Name, Surname);
                TableResult retrievedResult = table.Execute(retrieveOperation);
                VotingRecordEntity deleteEntity = (VotingRecordEntity)retrievedResult.Result;
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                table.Execute(deleteOperation);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}