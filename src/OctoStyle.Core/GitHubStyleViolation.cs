namespace OctoStyle.Core
{
    using System;

    /// <summary>
    /// Represents a GitHub pull request file style violation.
    /// </summary>
    public class GitHubStyleViolation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubStyleViolation"/> class.
        /// </summary>
        /// <param name="ruleId">The ID of the rule which was broken.</param>
        /// <param name="message">The violation message.</param>
        /// <param name="lineNumber">The line number of the violation.</param>
        public GitHubStyleViolation(string ruleId, string message, int lineNumber)
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
            this.LineNumber = lineNumber;
        }

        /// <summary>
        /// Gets the <see cref="RuleId"/>.
        /// </summary>
        /// <value>The ID of the rule which was broken.</value>
        public string RuleId { get; private set; }

        /// <summary>
        /// Gets the <see cref="Message"/>.
        /// </summary>
        /// <value>The violation message.</value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the <see cref="LineNumber"/>.
        /// </summary>
        /// <value>The line number of the violation.</value>
        public int LineNumber { get; private set; }
    }
}