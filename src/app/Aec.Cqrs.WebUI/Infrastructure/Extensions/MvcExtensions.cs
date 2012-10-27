using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Aec.Cqrs.WebUI.Infrastructure.Extensions
{
    public static class MvcExtensions
    {
        public static string Errors(this ModelStateDictionary state)
        {
            var errors = new List<string>();

            foreach (var error in state.Values)
            {
                errors.AddRange(error.Errors.Select(
                    err => string.IsNullOrEmpty(err.ErrorMessage) ? err.Exception.Message : err.ErrorMessage));
            }

            return string.Join(" ", errors).Trim();
        }
    }
}