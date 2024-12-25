using System.Net;

namespace RuleForge.Rules.Common
{
    public class IPAddressRule : IRule<string>
    {
        private readonly bool _allowIPv4;
        private readonly bool _allowIPv6;
        public string ErrorMessage { get; }

        public IPAddressRule(bool allowIPv4 = true, bool allowIPv6 = true, string? errorMessage = null)
        {
            if (!allowIPv4 && !allowIPv6)
            {
                throw new ArgumentException("At least one IP version must be allowed");
            }

            _allowIPv4 = allowIPv4;
            _allowIPv6 = allowIPv6;
            ErrorMessage = errorMessage ?? "Invalid IP address format";
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            if (!IPAddress.TryParse(value, out IPAddress? address))
            {
                return ValidationResult.Error("IPAddress", ErrorMessage);
            }

            bool isIPv4 = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            bool isIPv6 = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;

            if ((!_allowIPv4 && isIPv4) || (!_allowIPv6 && isIPv6))
            {
                return ValidationResult.Error("IPAddress", $"IP version {(isIPv4 ? "v4" : "v6")} is not allowed");
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}