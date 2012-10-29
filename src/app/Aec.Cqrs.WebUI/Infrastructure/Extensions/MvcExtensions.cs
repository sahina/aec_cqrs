using System;
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

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> items,
            Func<T, string> text,
            Func<T, string> value = null,
            Func<T, Boolean> selected = null)
        {
            return items.Select(p => new SelectListItem
            {
                Text = text.Invoke(p),
                Value = (value == null ? text.Invoke(p) : value.Invoke(p)),
                Selected = selected != null && selected.Invoke(p)
            });
        }
    }
}