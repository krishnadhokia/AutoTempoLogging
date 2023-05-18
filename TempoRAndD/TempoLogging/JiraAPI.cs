using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TempoLogging
{
    public class JiraAPI
    {
        public static async Task<JiraResponse> GetIssueIdUsingKey(string jiraIssueKey)
        {
            JiraResponse jiraResponse = new JiraResponse();
            try
            {
                string jiraApiUrl = "https://advancedcsg.atlassian.net/rest/api/"; // Jira API base URL

                // Create an HttpClient instance
                HttpClient client = new HttpClient();

                // Set the base address of the Jira API
                client.BaseAddress = new Uri(jiraApiUrl);

                // Add necessary headers, such as authentication or content type if required
                client.DefaultRequestHeaders.Add("Authorization", "Basic /* Add your Jira Token */");

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

            // Example implementation assuming the response body contains JSON data with an "id" field representing the issue ID
            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);


            jiraResponse.issueId = jsonResponse.id;
            jiraResponse.accountId = jsonResponse.fields.assignee.accountId;

            return jiraResponse;
        }
    }
}
