namespace OctoStyle.Core
{
    using System;

    /// <summary>
    /// Represents a GitHub repository.
    /// </summary>
    public class GitHubRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubRepository"/> class.
        /// </summary>
        /// <param name="owner">The repository owner.</param>
        /// <param name="name">The name of the repository.</param>
        public GitHubRepository(string owner, string name)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            const string cannotBeEmptyMessage = "Cannot be empty.";

            if (owner.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "owner");
            }

            if (name.Length == 0)
            {
                throw new ArgumentException(cannotBeEmptyMessage, "name");
            }

            this.Owner = owner;
            this.Name = name;
        }

        /// <summary>
        /// Gets the <see cref="Owner"/>.
        /// </summary>
        /// <value>The owner of the repository.</value>
        public string Owner { get; private set; }

        /// <summary>
        /// Gets the <see cref="Name"/>
        /// </summary>
        /// <value>The name of the repository.</value>
        public string Name { get; private set; }
    }
}