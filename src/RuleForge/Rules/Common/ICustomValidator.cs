namespace RuleForge.Rules.Common
{
    public interface ICustomValidator<T>
    {
        ValidationResult Validate(ValidationContext<T> context);
        Task<ValidationResult> ValidateAsync(ValidationContext<T> context);
    }

    public class ValidationContext<T>
    {
        public T Instance { get; }
        public string PropertyName { get; }
        public object PropertyValue { get; }
        public object RootInstance { get; }
        public ValidationContext<T> Parent { get; }

        public ValidationContext(T instance, string propertyName = null, object propertyValue = null, object rootInstance = null, ValidationContext<T> parent = null)
        {
            Instance = instance;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            RootInstance = rootInstance ?? instance;
            Parent = parent;
        }
    }
}