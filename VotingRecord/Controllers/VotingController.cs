// a RESTful services which provides stock market prices for stocks

using VotingRecords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace VotingRecords.Controllers
{
    public class RecordController : ApiController
    {
        /* 
        * GET /api/stock/            get all stock listings               GetAllListings
        * GET /api/stock/IBM         get price last trade for IBM         GetStockPrice   
        */

        // the listings on this stock market
        private List<VotingRecord> records;

        // initialise the listings collection, stateless
        public RecordController()
        {
            records = new List<VotingRecord>()
                {
                    new VotingRecord { Name = "EndaKenny", Vote = "Ta" },
                    new VotingRecord { Name = "MichaelMartin", Vote = "Nil" },
                    new VotingRecord { Name = "JoanBurton", Vote = "Ta" },
                    new VotingRecord { Name = "GerryAdams", Vote = "Nil" },
                    new VotingRecord { Name = "StephenDonnelly", Vote = "Nil" },
                    new VotingRecord { Name = "EamonnRyan", Vote = "Ta" },
                    new VotingRecord { Name = "ShaneRoss", Vote = "Ta" },
                    new VotingRecord { Name = "RichardBoydBarret", Vote = "Absent" }
                };
        }
        // todo: use repository pattern

        // GET api/stock
        public IHttpActionResult GetAllVoting()
        {
            return Ok(records.OrderBy(s => s.Name).ToList());                                                   // 200 OK, listings serialized in response body
        }

        // GET api/stock/GOOG or api/stock?ticker=GOOG
        // default route template changed to api/{controller}/{id} rather than api/{controller}/{ticker} in WebApiConfig.cs
        [Route("bill/Name/{name:alpha}")]
        public IHttpActionResult GetTDVote(String name)
        {
            // LINQ query, find matching TD (case-insensitive) or default value (null) if none matching
            VotingRecord record = records.SingleOrDefault(v => v.Name.ToUpper() == name.ToUpper());
            if (record == null)
            {
                return NotFound();
            }
            //Console.WriteLine("Hello");
            return Ok(record.Vote);
            //Console.WriteLine("World");
        }

        // GET api/stock/GOOG or api/stock?ticker=GOOG
        // default route template changed to api/{controller}/{id} rather than api/{controller}/{ticker} in WebApiConfig.cs
        [Route("bill/result/{record:alpha}")]
        public IHttpActionResult GetVoteChoice(String result)
        {
            // LINQ query, find matching TD (case-insensitive) or default value (null) if none matching
            VotingRecord record = records.SingleOrDefault(v => v.Vote.ToUpper() == result.ToUpper());
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record.Name);
        }

    }
}
