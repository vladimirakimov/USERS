using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using StringToExpression.LanguageDefinitions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ITG.Brix.Users.Infrastructure.Providers.Impl
{
    public class OdataProvider : IOdataProvider
    {
        public string FilterTransform(string filter, IDictionary<string, string> replacements)
        {
            string result = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = filter;
                foreach (var replacement in replacements)
                {
                    result = result.Replace(replacement.Key, replacement.Value);
                }
            }
            return result;
        }

        public Expression<Func<User, bool>> GetFilterPredicate(string filter)
        {
            Expression<Func<User, bool>> result = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var language = new ODataFilterLanguage();
                try
                {
                    result = language.Parse<User>(filter);
                }
                catch (Exception exception)
                {
                    throw new FilterODataException(exception);
                }
            }

            return result;
        }
    }
}
