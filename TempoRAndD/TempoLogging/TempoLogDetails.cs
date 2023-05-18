namespace TempoLogging
{
    public class TempoLogDetails
    {
        public string authorAccountId { get; set; }
        public string description { get; set; }
        public int issueId { get; set; }
        public string startDate { get; set; }
        public string startTime { get; set; }
        public int timeSpentSeconds { get; set; }
    }
    public class JiraResponse
    {
        public string issueId { get; set; }
        public string accountId { get; set; }
    }
}
