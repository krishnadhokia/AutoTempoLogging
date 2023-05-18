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
        public async Task<bool> Log(TempoLogDetails tempoLogDetails)
        {
            bool isLogged = false;

            // Tempo worklog endpoint
            string tempoEndpoint = "https://api.tempo.io";

            // Create the HttpClient and set base URL
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(tempoEndpoint);

            // Set Jira credentials
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer mlYyTDFIwHgxanwM7ZRBmPO85yFTe3");

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
