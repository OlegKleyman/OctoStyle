namespace OctoStyle.Core
{
    using System;

    public class GitHubStyleViolation
    {
        public string RuleId { get; private set; }

        public string Message { get; private set; }

        public int Position { get; private set; }

        public GitHubStyleViolation(string ruleId, string message, int position)
        {
            if (ruleId == null)
            {
                throw new ArgumentNullException("ruleId");
            }
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.RuleId = ruleId;
            this.Message = message;
            this.Position = position;
        }
    }
}