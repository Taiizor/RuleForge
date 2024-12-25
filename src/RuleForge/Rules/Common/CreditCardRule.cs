namespace RuleForge.Rules.Common
{
    public class CreditCardRule : IRule<string>
    {
        public string ErrorMessage { get; }

        public CreditCardRule(string? errorMessage = null)
        {
            ErrorMessage = errorMessage ?? "Invalid credit card number";
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            value = value.Replace(" ", "").Replace("-", "");

            if (!value.All(char.IsDigit))
            {
                return ValidationResult.Error("CreditCard", ErrorMessage);
            }

            int sum = 0;
            bool isEven = false;

            // Luhn Algorithm
            for (int i = value.Length - 1; i >= 0; i--)
            {
                int digit = value[i] - '0';

                if (isEven)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                isEven = !isEven;
            }

            return sum % 10 == 0 
                ? ValidationResult.Success() 
                : ValidationResult.Error("CreditCard", ErrorMessage);
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}