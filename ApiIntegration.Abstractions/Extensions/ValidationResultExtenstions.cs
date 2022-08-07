namespace FluentValidation
{
    using System;

    using FluentValidation.Results;

    public static class ValidationResultExtenstions
    {
        public static string GetErrorMessages(this ValidationResult validationResult)
        {
            if (validationResult is null)
            {
                throw new ArgumentNullException(nameof(validationResult));
            }

            return string.Join(Environment.NewLine, validationResult.Errors.Select(x => x.ErrorMessage));
        }
    }
}
