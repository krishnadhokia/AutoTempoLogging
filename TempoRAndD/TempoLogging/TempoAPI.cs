using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TempoLogging
{
    public class TempoAPI
    {
        public async Task<bool> Log(string issueId)
        {
            bool isLogged = false;
            TempoLogDetails tempoLogDetails = new TempoLogDetails()
            {
                authorAccountId = "6226d71159c0740069dabd57",
                description = "Testing Tempo Outlook Integration",
                issueId = Convert.ToInt32(issueId),
                startDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                startTime = "20:06:00",
                timeSpentSeconds = 3600
            };
            // Tempo worklog endpoint
            string tempoEndpoint = "https://api.tempo.io";

            // Create the HttpClient and set base URL
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(tempoEndpoint);

            // Set Jira credentials
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer /*Add your tempo token*/");

            string tempoUrl = "/4/worklogs";
            // serialize tempo details class to pass payload

            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(tempoLogDetails);
            try
            {
                // Send POST request to create the worklog
                HttpResponseMessage response = await httpClient.PostAsync(tempoUrl, new StringContent(payload, Encoding.UTF8, "application/json"));

                // Check response status
                isLogged = response.IsSuccessStatusCode;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
            }
            return isLogged;
        }
    }
}
