using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Ronin.XrmToolbox.UserTimezoneManager.Models;

namespace Ronin.XrmToolbox.UserTimezoneManager.Services
{
    /// <summary>
    /// Retrieves all available timezone definitions from the Dataverse <c>timezonedefinition</c> entity.
    /// The results are cached after the first retrieval.
    /// </summary>
    public class TimezoneService
    {
        private List<TimezoneOption> _cache;

        /// <summary>
        /// Returns cached timezone options, querying Dataverse only on the first call.
        /// </summary>
        public List<TimezoneOption> GetTimezones(IOrganizationService service)
        {
            if (_cache != null)
                return _cache;

            var query = new QueryExpression("timezonedefinition")
            {
                ColumnSet = new ColumnSet("timezonecode", "userinterfacename", "standardname"),
                Orders =
                {
                    new OrderExpression("userinterfacename", OrderType.Ascending)
                }
            };

            var result = service.RetrieveMultiple(query);

            _cache = result.Entities
                .Select(e => new TimezoneOption
                {
                    TimezoneCode = e.GetAttributeValue<int>("timezonecode"),
                    DisplayName = e.GetAttributeValue<string>("userinterfacename"),
                    StandardName = e.GetAttributeValue<string>("standardname")
                })
                .ToList();

            return _cache;
        }

        /// <summary>
        /// Looks up the display name for a given timezone code from the cache.
        /// Returns the code as a string if not found.
        /// </summary>
        public string GetDisplayName(int timezoneCode)
        {
            if (_cache == null)
                return timezoneCode.ToString();

            var option = _cache.FirstOrDefault(t => t.TimezoneCode == timezoneCode);
            return option?.DisplayName ?? timezoneCode.ToString();
        }

        /// <summary>Clears the cached timezone data, forcing a fresh load on next request.</summary>
        public void InvalidateCache() => _cache = null;
    }
}
