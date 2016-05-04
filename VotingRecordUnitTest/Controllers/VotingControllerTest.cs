using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System;
using System.Net;
using System.IO;

namespace VotingRecords.Controllers.Tests
{
    [TestClass()]
    public class VotingControllerTest
    {
        string baseAddress = "http://votingrecord.azurewebsites.net/";
        string allURL = "BillNo/All";
        string tdURL = "BillNo/TD/Enda/Kenny";
        string partyURL = "BillNo/Party/SF";
        string voteURL = "BillNo/Vote/Absent";
        string insertURL = "BillNo/Insert/Stephen/Murphy/Independent/Nil";
        string updateURL = "BillNo/Update/Stephen/Murphy/Ta";
        string deleteURL = "BillNo/Delete/Stephen/Murphy";

        [TestMethod()]
        public void GetbyAllTest()
        {
            HttpClient getTDclient = new HttpClient();
            string fullURL = baseAddress + allURL;
            HttpResponseMessage response = getTDclient.GetAsync(fullURL).Result;
            String A = "OK";
            String B = response.StatusCode.ToString();
            Assert.AreEqual(A, B); 
        }

        [TestMethod()]
        public void GetbyTDTest()
        {
            HttpClient getTDclient = new HttpClient();
            string fullURL = baseAddress + tdURL;
            HttpResponseMessage response = getTDclient.GetAsync(fullURL).Result;
            String A = "OK";
            String B = response.StatusCode.ToString();
            Assert.AreEqual(A, B);
        }

        [TestMethod()]
        public void GetbyPartyTest()
        {
            HttpClient getTDclient = new HttpClient();
            string fullURL = baseAddress + partyURL;
            HttpResponseMessage response = getTDclient.GetAsync(fullURL).Result;
            String A = "OK";
            String B = response.StatusCode.ToString();
            Assert.AreEqual(A, B);
        }

        [TestMethod()]
        public void GetbyVoteTest()
        {
            HttpClient getTDclient = new HttpClient();
            string fullURL = baseAddress + voteURL;
            HttpResponseMessage response = getTDclient.GetAsync(fullURL).Result;
            String A = "OK";
            String B = response.StatusCode.ToString();
            Assert.AreEqual(A, B);
        }

        [TestMethod()]
        public void InsertEntryTest()
        {
            string URI = "http://votingrecord.azurewebsites.net/BillNo/Insert/Stephen/Murphy/Independent/Nil";
            string data = "";
        
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            request.Method = "POST";
            request.ContentType = "text/plain";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.WriteLine(data);
            }

            WebResponse response = request.GetResponse();
            String A = "System.Net.HttpWebResponse";
            String B = response.ToString();
            Assert.AreEqual(A, B);
        }

        [TestMethod()]
        public void UpdateEntryTest()
        {
            string URI = "http://votingrecord.azurewebsites.net/BillNo/Update/Stephen/Murphy/Ta";
            string data = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            request.Method = "PUT";
            request.ContentType = "text/plain";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.WriteLine(data);
            }

            WebResponse response = request.GetResponse();
            String A = "System.Net.HttpWebResponse";
            String B = response.ToString();
            Assert.AreEqual(A, B);
        }

        [TestMethod()]
        public void DeleteEntryTest()
        {
            string URI = "http://votingrecord.azurewebsites.net/BillNo/Delete/Stephen/Murphy";
            string data = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            request.Method = "DELETE";
            request.ContentType = "text/plain";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.WriteLine(data);
            }

            WebResponse response = request.GetResponse();
            String A = "System.Net.HttpWebResponse";
            String B = response.ToString();
            Assert.AreEqual(A, B);
        }
    }
}