using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ronin.XrmToolbox
{
    public class DataverseWebResourceService
    {
        private const int WebResourceComponentType = 61;

        public List<SolutionListItem> GetUnmanagedSolutions(IOrganizationService service)
        {
            var query = new QueryExpression("solution")
            {
                ColumnSet = new ColumnSet("solutionid", "friendlyname", "uniquename", "ismanaged"),
                Criteria = new FilterExpression(LogicalOperator.And)
            };

            query.Criteria.AddCondition("ismanaged", ConditionOperator.Equal, false);
            query.Orders.Add(new OrderExpression("friendlyname", OrderType.Ascending));

            var publisherLink = query.AddLink("publisher", "publisherid", "publisherid");
            publisherLink.Columns = new ColumnSet("customizationprefix");
            publisherLink.EntityAlias = "pub";

            var result = service.RetrieveMultiple(query);
            return result.Entities.Select(e => new SolutionListItem
            {
                Id = e.Id,
                UniqueName = e.GetAttributeValue<string>("uniquename"),
                DisplayName = string.IsNullOrWhiteSpace(e.GetAttributeValue<string>("friendlyname"))
                    ? e.GetAttributeValue<string>("uniquename")
                    : string.Format("{0} ({1})", e.GetAttributeValue<string>("friendlyname"), e.GetAttributeValue<string>("uniquename")),
                PublisherPrefix = (e.GetAttributeValue<AliasedValue>("pub.customizationprefix")?.Value as string) ?? string.Empty
            }).ToList();
        }

        public List<WebResourceRow> GetWebResourcesForSolution(IOrganizationService service, Guid solutionId)
        {
            var componentQuery = new QueryExpression("solutioncomponent")
            {
                ColumnSet = new ColumnSet("objectid"),
                Criteria = new FilterExpression(LogicalOperator.And)
            };

            componentQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solutionId);
            componentQuery.Criteria.AddCondition("componenttype", ConditionOperator.Equal, WebResourceComponentType);

            var componentResults = service.RetrieveMultiple(componentQuery);
            var webResourceIds = componentResults.Entities
                .Select(e => e.GetAttributeValue<Guid>("objectid"))
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (webResourceIds.Count == 0)
            {
                return new List<WebResourceRow>();
            }

            var rows = new List<WebResourceRow>();
            foreach (var chunk in Chunk(webResourceIds, 200))
            {
                var webResourceQuery = new QueryExpression("webresource")
                {
                    ColumnSet = new ColumnSet("webresourceid", "name", "displayname", "webresourcetype", "content"),
                    Criteria = new FilterExpression(LogicalOperator.And)
                };

                webResourceQuery.Criteria.AddCondition("webresourceid", ConditionOperator.In, chunk.Cast<object>().ToArray());

                var webResources = service.RetrieveMultiple(webResourceQuery);
                rows.AddRange(webResources.Entities.Select(ToWebResourceRow));
            }

            return rows.OrderBy(r => r.Name).ToList();
        }

        public Guid CreateWebResource(IOrganizationService service, string name, string displayName, int webResourceType, string contentBase64)
        {
            var webResource = new Entity("webresource");
            webResource["name"] = name;
            webResource["displayname"] = displayName;
            webResource["webresourcetype"] = new OptionSetValue(webResourceType);
            webResource["content"] = contentBase64;
            return service.Create(webResource);
        }

        public void AddWebResourceToSolution(IOrganizationService service, Guid webResourceId, string solutionUniqueName)
        {
            var addToSolution = new AddSolutionComponentRequest
            {
                ComponentType = WebResourceComponentType,
                ComponentId = webResourceId,
                SolutionUniqueName = solutionUniqueName
            };

            service.Execute(addToSolution);
        }

        public void UpdateWebResourceContent(IOrganizationService service, Guid webResourceId, string contentBase64)
        {
            var update = new Entity("webresource", webResourceId);
            update["content"] = contentBase64;
            service.Update(update);
        }

        public void PublishWebResources(IOrganizationService service, IEnumerable<Guid> webResourceIds)
        {
            var ids = webResourceIds?.Where(id => id != Guid.Empty).Distinct().ToList() ?? new List<Guid>();
            if (ids.Count == 0)
            {
                return;
            }

            var xml = "<importexportxml><webresources>" +
                      string.Join(string.Empty, ids.Select(id => string.Format("<webresource>{0}</webresource>", id))) +
                      "</webresources></importexportxml>";

            service.Execute(new PublishXmlRequest { ParameterXml = xml });
        }

        private static WebResourceRow ToWebResourceRow(Entity webResource)
        {
            var typeCode = webResource.GetAttributeValue<OptionSetValue>("webresourcetype")?.Value ?? 0;
            return new WebResourceRow
            {
                Selected = false,
                WebResourceId = webResource.GetAttributeValue<Guid>("webresourceid"),
                Name = webResource.GetAttributeValue<string>("name"),
                DisplayName = webResource.GetAttributeValue<string>("displayname"),
                TypeCode = typeCode,
                TypeLabel = WebResourceTypeHelper.ResolveWebResourceTypeLabel(typeCode),
                ContentBase64 = webResource.GetAttributeValue<string>("content")
            };
        }

        private static IEnumerable<List<Guid>> Chunk(List<Guid> source, int chunkSize)
        {
            for (var i = 0; i < source.Count; i += chunkSize)
            {
                yield return source.Skip(i).Take(chunkSize).ToList();
            }
        }
    }
}
