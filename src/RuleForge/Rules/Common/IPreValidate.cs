namespace RuleForge.Rules.Common
{
    public interface IPreValidate<T>
    {
        bool ShouldValidate(ValidationContext<T> context);
    }

    public class PreValidateContext<T>
    {
        public T Instance { get; }
        public string PropertyName { get; }
        public object PropertyValue { get; }

        public PreValidateContext(T instance, string propertyName = null, object propertyValue = null)
        {
            Instance = instance;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}