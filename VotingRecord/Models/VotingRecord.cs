// a StockListing i.e. ticker and price

using System;
using System.ComponentModel.DataAnnotations;

namespace VotingRecords.Models
{
    // a record for a TDs voting history
    public class VotingRecord
    {
        // TD Name e.g. Enda Kenny, Gerry Adams
        [Required]
        public string Name
        {
            get;
            set;
        }

        // Party a TD belongs too e.g. Fine Gael, Sinn Fein
      /**  [Required]
        public string Party
        {
            get;
            set;
        } **/

        // Voting Record - Tá, Níl or Absent
        //[Required]
        public string Vote
        {
            get;
            set;
        }
    }
}