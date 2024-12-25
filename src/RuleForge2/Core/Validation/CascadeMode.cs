namespace RuleForge2.Core.Validation
{
    /// <summary>
    /// Specifies how validation should handle multiple rules.
    /// </summary>
    public enum CascadeMode
    {
        /// <summary>
        /// Continue validation after a rule fails.
        /// </summary>
        Continue = 0,

        /// <summary>
        /// Stop validation after a rule fails.
        /// </summary>
        StopOnFirstFailure = 1
    }
}