using System.Text.Json;

namespace RuleForge.Rules.Common
{
    public class JsonRule : IRule<string>
    {
        private readonly JsonSchemaOptions _options;
        public string ErrorMessage { get; }

        public JsonRule(JsonSchemaOptions? options = null, string? errorMessage = null)
        {
            _options = options ?? new JsonSchemaOptions();
            ErrorMessage = errorMessage ?? "Invalid JSON format";
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            try
            {
                using JsonDocument document = JsonDocument.Parse(value);

                if (_options.RequireObject && document.RootElement.ValueKind != JsonValueKind.Object)
                {
                    return ValidationResult.Error("Json", "JSON must be an object");
                }

                if (_options.RequireArray && document.RootElement.ValueKind != JsonValueKind.Array)
                {
                    return ValidationResult.Error("Json", "JSON must be an array");
                }

                if (_options.MaxDepth.HasValue)
                {
                    int depth = CalculateDepth(document.RootElement);
                    if (depth > _options.MaxDepth.Value)
                    {
                        return ValidationResult.Error("Json", $"JSON depth exceeds maximum allowed depth of {_options.MaxDepth.Value}");
                    }
                }

                return ValidationResult.Success();
            }
            catch (JsonException)
            {
                return ValidationResult.Error("Json", ErrorMessage);
            }
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }

        private static int CalculateDepth(JsonElement element)
        {
            int maxDepth = 0;

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty property in element.EnumerateObject())
                    {
                        maxDepth = Math.Max(maxDepth, CalculateDepth(property.Value));
                    }
                    return maxDepth + 1;

                case JsonValueKind.Array:
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        maxDepth = Math.Max(maxDepth, CalculateDepth(item));
                    }
                    return maxDepth + 1;

                default:
                    return 0;
            }
        }
    }

    public class JsonSchemaOptions
    {
        public bool RequireObject { get; set; }
        public bool RequireArray { get; set; }
        public int? MaxDepth { get; set; }
    }
}