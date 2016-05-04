// a StockListing i.e. ticker and price

using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VotingRecords.Models
{
    // a record for a TDs voting history
    /// <summary>
    /// An Entity class to store the information relevant to voting records in NoSQL Database
    /// </summary>
    public class VotingRecordEntity : TableEntity
    {
        /// <summary>
        /// Entity property which contains required Name and Surname arguments
        /// </summary>
        /// <param name="Name">Name of TD</param>
        /// <param name="Surname">Surname of TD</param>
        public VotingRecordEntity(string Name, string Surname)
        {
            this.PartitionKey = Name;
            this.RowKey = Surname;
            this.Bill = Bill;
            this.Party = Party;
            this.Vote = Vote;
        }
        /// <summary>
        /// Additional Entity properties for voting record entity with no required properties
        /// Bill Name, Vote & Party Membership
        /// </summary>
        public VotingRecordEntity() { }

        /// <summary>
        /// Bill Name vote belongs too
        /// </summary>
        public string Bill { get; set; }
        /// <summary>
        /// Party Membership of TD
        /// </summary>
        public string Party { get; set; }
        /// <summary>
        /// Vote Cast during debate
        /// </summary>
        public string Vote { get; set; }

        internal object OrderBy(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}