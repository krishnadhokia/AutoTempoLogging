using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TempoLogging
{
    public class JiraAPI
    {
        public static async Task<JiraResponse> GetIssueIdUsingKey(string jiraIssueKey)
        {
            JiraResponse jiraResponse = new JiraResponse();
            List<string> responseResult = new List<string>();
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                string jiraApiUrl = "https://advancedcsg.atlassian.net/rest/api/"; // Jira API base URL

                // Create an HttpClient instance
                HttpClient client = new HttpClient();

                // Set the base address of the Jira API
                client.BaseAddress = new Uri(jiraApiUrl);

                // Add necessary headers, such as authentication or content type if required
                client.DefaultRequestHeaders.Add("Authorization", "Basic a2h1c2hib28uamFqb29Ab25lYWR2YW5jZWQuY29tOkFUQVRUM3hGZkdGMGFtLVhuY0ZtNXctVnpIUGdINFNOaFd3eWNtaHZjNjBHVmVrd0Q2NWN0NmYwWHFWdHhHRUUtN0ZHX19QRlUwazd6Z3M1UHVSQVM4UkgtRXVqbGgyNWI4UzgzTUhtWnVKN3lWTEViUzhNTjl1V3M2SzZRT0hKVy0wRnlTWjQtdjUzdnpLSWotcXN0WjRXNlp5WmRLZUg5ZU1BMXNNZk5vcnh3U3M3VkxVeWlqND04QUE1NzJGNA==");

                // Build the request URL
                string requestUrl = $"3/issue/{jiraIssueKey}";


                // Send GET request to retrieve issue details
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the response JSON to extract the issue ID
                    jiraResponse = ParseJiraIssueId(responseBody);

                    
                }
                else
                {
                    // If the request was not successful, handle the error
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jiraResponse;
        }
        static JiraResponse ParseJiraIssueId(string responseBody)
        {
            JiraResponse jiraResponse = new JiraResponse();
            // Parse the response JSON to extract the issue ID
            // Implement your own logic here based on the structure of the response
            // Return the issue ID
            List<string> response = new List<string>();
            // Example implementation assuming the response body contains JSON data with an "id" field representing the issue ID
            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
            jiraResponse.issueId = jsonResponse.id;
            jiraResponse.accountId = "557058:ed17f950-4877-450b-a7a5-734b21fe3d25";//jsonResponse.fields.assignee.accountId;

            return jiraResponse;
        }
    }
}
