using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Ronin.XrmToolbox.UserTimezoneManager.Services
{
    /// <summary>
    /// Retrieves system views (<c>savedquery</c> records) that return <c>systemuser</c> records.
    /// </summary>
    public class ViewService
    {
        /// <summary>
        /// Loads all public and user-defined views for the <c>systemuser</c> entity.
        /// </summary>
        public List<SystemView> GetSystemUserViews(IOrganizationService service)
        {
            // querytype 0 = public view, 1 = advanced find view
            var query = new QueryExpression("savedquery")
            {
                ColumnSet = new ColumnSet("savedqueryid", "name", "fetchxml", "querytype"),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("returnedtypecode", ConditionOperator.Equal, "systemuser"),
                        new ConditionExpression("statecode", ConditionOperator.Equal, 0)
                    }
                },
                Orders = { new OrderExpression("name", OrderType.Ascending) }
            };

            var result = service.RetrieveMultiple(query);

            return result.Entities
                .Select(e => new SystemView
                {
                    Id = e.Id,
                    Name = e.GetAttributeValue<string>("name"),
                    FetchXml = e.GetAttributeValue<string>("fetchxml"),
                    QueryType = e.GetAttributeValue<int>("querytype")
                })
                .ToList();
        }
    }

    /// <summary>Represents a saved system view.</summary>
    public class SystemView
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string FetchXml { get; set; }
        public int QueryType { get; set; }

        public override string ToString() => Name;
    }
}
