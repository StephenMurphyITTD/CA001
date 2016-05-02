// a StockListing i.e. ticker and price

using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VotingRecords.Models
{
    // a record for a TDs voting history
    public class VotingRecordEntity : TableEntity
    {
        public VotingRecordEntity(string Name, string Surname)
        {
            this.PartitionKey = Name;
            this.RowKey = Surname;
            this.Bill = Bill;
            this.Party = Party;
            this.Vote = Vote;
        }
        public VotingRecordEntity() { }

        public string Bill { get; set; }
        public string Party { get; set; }
        public string Vote { get; set; }

        internal object OrderBy(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}