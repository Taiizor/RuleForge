using System.Collections.Generic;
using RuleForge.Abstractions;

namespace RuleForge.Core.Validation
{
    /// <summary>
    /// Represents a named set of validation rules.
    /// </summary>
    public class RuleSet
    {
        /// <summary>
        /// Gets the name of the rule set.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the rules in this set.
        /// </summary>
        public IList<IRule> Rules { get; }

        /// <summary>
        /// Initializes a new instance of the RuleSet class.
        /// </summary>
        /// <param name="name">The name of the rule set.</param>
        public RuleSet(string name)
        {
            Name = name;
            Rules = new List<IRule>();
        }
    }

    /// <summary>
    /// Represents a named set of validation rules for a specific type.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    public class RuleSet<T>
    {
        /// <summary>
        /// Gets the name of the rule set.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the rules in this set.
        /// </summary>
        public IList<IRule<T>> Rules { get; }

        /// <summary>
        /// Initializes a new instance of the RuleSet class.
        /// </summary>
        /// <param name="name">The name of the rule set.</param>
        public RuleSet(string name)
        {
            Name = name;
            Rules = new List<IRule<T>>();
        }
    }
}