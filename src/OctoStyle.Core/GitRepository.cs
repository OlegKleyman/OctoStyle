namespace OctoStyle.Core
{
    using System;

    public class GitRepository
    {
        public string Owner { get; private set; }

        public string Name { get; private set; }

        public GitRepository(string owner, string name)
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
    }
}