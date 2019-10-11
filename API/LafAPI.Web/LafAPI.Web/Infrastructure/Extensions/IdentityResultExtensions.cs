namespace LafAPI.Web.Infrastructure.Extensions
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public static class IdentityResultExtensions
    {
        public static string GetFirstError(this IdentityResult identityResult)
        {
            if (identityResult == null)
            {
                throw new ArgumentNullException(nameof(identityResult));
            }

            return identityResult.Errors.Select(e => e.Description).FirstOrDefault();
        }

        public static ModelStateDictionary GetAllErrors(this IdentityResult identityResult, ModelStateDictionary modelState)
        {
            if (identityResult == null)
            {
                throw new ArgumentNullException(nameof(identityResult));
            }

            foreach (var error in identityResult.Errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }

            return modelState;
        }
    }
}