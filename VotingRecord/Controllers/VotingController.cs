// a RESTful services which provides voting records of TDs on a bill

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
        * GET /api/record/                  get all TDs & There Votes       GetAllVoting
        * GET /bill/name/StephenDonnelly    get vote from particular TD     GetTDVote   
        */

        // the vote record on this bill 
        private List<VotingRecord> records;

        // initialise the listings collection, stateless
        public RecordController()
        {
            records = new List<VotingRecord>()
                {
                    new VotingRecord { Name = "EndaKenny", Vote = "Ta", Party = "FineGael" },
                    new VotingRecord { Name = "MichaelMartin", Vote = "Nil", Party = "FiannaFail" },
                    new VotingRecord { Name = "JoanBurton", Vote = "Ta", Party = "Labour" },
                    new VotingRecord { Name = "GerryAdams", Vote = "Nil", Party = "SinnFein" },
                    new VotingRecord { Name = "StephenDonnelly", Vote = "Nil", Party = "SocDems" },
                    new VotingRecord { Name = "EamonnRyan", Vote = "Ta", Party = "Greens" },
                    new VotingRecord { Name = "ShaneRoss", Vote = "Ta", Party = "IndependentAlliance" },
                    new VotingRecord { Name = "RichardBoydBarret", Vote = "Absent", Party = "AAA/PBP" },
                    new VotingRecord { Name = "Michael Noonan", Vote = "Ta", Party = "FineGael" },
                    new VotingRecord { Name = "MaryLouMcDonald", Vote = "Ta", Party = "SinnFein" }
                };
        }
        // todo: use repository pattern

        // GET api/record
        public IHttpActionResult GetAllVoting()
        {
            return Ok(records.OrderBy(s => s.Name).ToList());                                                   // 200 OK, listings serialized in response body
        }

        // GET bill/name/EndaKenny
        // default route template changed to api/{controller}/{id} rather than api/{controller}/{ticker} in WebApiConfig.cs
        [Route("bill/Name/{name:alpha}")]
        public IHttpActionResult GetTDVote(String name)
        {
            // LINQ query, find matching TD (case-insensitive) or default value (null) if none matching
            VotingRecord record = records.FirstOrDefault(v => v.Name.ToUpper() == name.ToUpper());
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record.Vote);
        }

        // Get bill/result/Ta
        // default route template changed to api/{controller}/{id} rather than api/{controller}/{ticker} in WebApiConfig.cs
        [Route("bill/record/{result:alpha}")]
        public IHttpActionResult GetVoteChoice(String result)
        {
            // LINQ query, find matching vote (case-insensitive) or default value (null) if none matching
            var record = records.Where(v => v.Vote.ToUpper() == result.ToUpper());
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // GET bill/party/SinnFein
        [Route("bill/party/{groups:alpha}")]
        public IHttpActionResult GetPartyChoice(String groups)
        {
            // LINQ query, find voting record of a TDs based on party affiliation (case-insensitive) or default value (null) if none matching
            var record = records.Where(p => p.Party.ToUpper() == groups.ToUpper());

            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

    }
}
