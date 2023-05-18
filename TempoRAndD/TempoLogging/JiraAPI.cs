using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TempoLogging
{
    public class JiraAPI
    {
        public static async Task<string> GetIssueIdUsingKey(string jiraIssueKey)
        {
            string issueId = "";
            try
            {
                string jiraApiUrl = "https://advancedcsg.atlassian.net/rest/api/"; // Jira API base URL

                // Create an HttpClient instance
                HttpClient client = new HttpClient();

                // Set the base address of the Jira API
                client.BaseAddress = new Uri(jiraApiUrl);

                // Add necessary headers, such as authentication or content type if required
                client.DefaultRequestHeaders.Add("Authorization", "Basic a3Jpc2huYS5kaG9raXlhQG9uZWFkdmFuY2VkLmNvbTpBVEFUVDN4RmZHRjBwN3pJaDdkTk1GZmFEeW9DX0Z0SVJVbnFxa1VZR0lFdnNPU0FGeXZTa09na05uQ1AxN0hfZXl3OXQ2ekdqTzNuQU5fdk5MdHdTMWtfMXV4MDYyODZEV0lPX2JZNEtVRE1oTTE0WmdxUkZaU2dnUFluVVlTSGtjeFFZTFdHQlpobjF3OFdCTHFvZGFKV0J6aHJhRE93eERxMHRwSWlDVk5OaXVRUVN4SkpXSWM9QkUyN0M1REI=");

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
                    issueId = ParseJiraIssueId(responseBody);

                    Console.WriteLine($"Issue ID for Jira Key {jiraIssueKey}: {issueId}");
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
            return issueId;
        }
        static string ParseJiraIssueId(string responseBody)
        {
            // Parse the response JSON to extract the issue ID
            // Implement your own logic here based on the structure of the response
            // Return the issue ID

            // Example implementation assuming the response body contains JSON data with an "id" field representing the issue ID
            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
            string issueId = jsonResponse.id;

            return issueId;
        }
    }
}
