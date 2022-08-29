using FluentValidation;
using System.Net;

namespace Software.IP.Trace.Views.Find;

public class SearchCommandValidator : AbstractValidator<FindViewModel> {
    public SearchCommandValidator() {
        RuleFor(vm => vm.IPAddress).SetValidator(new LinkValidator());
        RuleFor(vm => vm.ApiKey).SetValidator(new ApiKeyValidator());
    }
}

public class LinkValidator : AbstractValidator<string> {
    private const int _urlMaxLength = 2048;

    public LinkValidator() {
        RuleFor(uri => uri)
            .NotEmpty()
            .WithMessage("IP Address is required.")
            .MaximumLength(_urlMaxLength)
            .WithMessage($"IP Address has to have up to {_urlMaxLength} characters.")
            .Must(uri => IPAddress.TryParse((string?)uri, out _))
            .WithMessage("IP Address is invalid.");
    }
}

public class ApiKeyValidator : AbstractValidator<string> {
    private const int _apiKeyLength = 32;

    public ApiKeyValidator() {
        RuleFor(uri => uri)
            .NotEmpty()
            .WithMessage("IPStack api key is required.")
            .Length(_apiKeyLength)
            .WithMessage($"IPStack api key has to have {_apiKeyLength} characters.");
    }
}