using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Ronin.XrmToolbox.UserTimezoneManager.Models;

namespace Ronin.XrmToolbox.UserTimezoneManager.Services
{
    /// <summary>
    /// Handles loading system users and reading/updating their timezone settings via
    /// the Dataverse <c>systemuser</c> and <c>usersettings</c> entities.
    /// </summary>
    public class UserService
    {
        private readonly TimezoneService _timezoneService;

        public UserService(TimezoneService timezoneService)
        {
            _timezoneService = timezoneService ?? throw new ArgumentNullException(nameof(timezoneService));
        }

        /// <summary>
        /// Retrieves enabled system users and resolves each user's current timezone
        /// from <c>usersettings</c>.
        /// </summary>
        public List<UserTimezoneModel> GetUsers(IOrganizationService service)
        {
            // 1. Load enabled system users
            var userQuery = new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("systemuserid", "fullname", "internalemailaddress"),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("isdisabled", ConditionOperator.Equal, false),
                        new ConditionExpression("isintegration", ConditionOperator.Equal, false),
                        new ConditionExpression("accessmode", ConditionOperator.NotEqual, 3) // exclude non-interactive/support
                    }
                },
                Orders = { new OrderExpression("fullname", OrderType.Ascending) }
            };

            var users = service.RetrieveMultiple(userQuery).Entities;

            if (users.Count == 0)
                return new List<UserTimezoneModel>();

            // 2. Load usersettings for all retrieved users in a single query
            var userIds = users.Select(u => u.Id).ToArray();
            var settingsQuery = new QueryExpression("usersettings")
            {
                ColumnSet = new ColumnSet("systemuserid", "timezonecode")
            };
            settingsQuery.Criteria.AddCondition("systemuserid", ConditionOperator.In, userIds.Cast<object>().ToArray());

            var settings = service.RetrieveMultiple(settingsQuery).Entities
                .ToDictionary(s => s.GetAttributeValue<Guid>("systemuserid"), s => s.GetAttributeValue<int>("timezonecode"));

            // 3. Combine into models
            var models = new List<UserTimezoneModel>(users.Count);
            foreach (var user in users)
            {
                var userId = user.Id;
                settings.TryGetValue(userId, out int timezoneCode);

                var model = new UserTimezoneModel
                {
                    UserId = userId,
                    FullName = user.GetAttributeValue<string>("fullname") ?? string.Empty,
                    EmailAddress = user.GetAttributeValue<string>("internalemailaddress") ?? string.Empty,
                    CurrentTimezoneCode = timezoneCode,
                    CurrentTimezoneName = _timezoneService.GetDisplayName(timezoneCode),
                    NewTimezoneCode = timezoneCode
                };
                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// Executes a SavedQuery FetchXML against Dataverse and returns matching users with timezone data.
        /// </summary>
        public List<UserTimezoneModel> GetUsersByFetchXml(IOrganizationService service, string fetchXml)
        {
            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            var users = result.Entities;

            if (users.Count == 0)
                return new List<UserTimezoneModel>();

            var userIds = users.Select(u => u.Id).ToArray();
            var settingsQuery = new QueryExpression("usersettings")
            {
                ColumnSet = new ColumnSet("systemuserid", "timezonecode")
            };
            settingsQuery.Criteria.AddCondition("systemuserid", ConditionOperator.In, userIds.Cast<object>().ToArray());

            var settings = service.RetrieveMultiple(settingsQuery).Entities
                .ToDictionary(s => s.GetAttributeValue<Guid>("systemuserid"), s => s.GetAttributeValue<int>("timezonecode"));

            var models = new List<UserTimezoneModel>(users.Count);
            foreach (var user in users)
            {
                var userId = user.Id;
                settings.TryGetValue(userId, out int timezoneCode);

                var model = new UserTimezoneModel
                {
                    UserId = userId,
                    FullName = user.GetAttributeValue<string>("fullname") ?? string.Empty,
                    EmailAddress = user.GetAttributeValue<string>("internalemailaddress") ?? string.Empty,
                    CurrentTimezoneCode = timezoneCode,
                    CurrentTimezoneName = _timezoneService.GetDisplayName(timezoneCode),
                    NewTimezoneCode = timezoneCode
                };
                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// Updates the timezone for a single user by writing to their <c>usersettings</c> record.
        /// </summary>
        /// <returns>The old timezone display name (for logging).</returns>
        public string UpdateUserTimezone(IOrganizationService service, Guid userId, int newTimezoneCode)
        {
            // usersettings uses the systemuserid as its primary key
            var oldName = _timezoneService.GetDisplayName(GetCurrentTimezoneCode(service, userId));

            var settings = new Entity("usersettings")
            {
                Id = userId
            };
            settings["timezonecode"] = newTimezoneCode;

            service.Update(settings);

            return oldName;
        }

        /// <summary>Reads the current timezone code directly from Dataverse for a given user.</summary>
        public int GetCurrentTimezoneCode(IOrganizationService service, Guid userId)
        {
            var result = service.Retrieve("usersettings", userId, new ColumnSet("timezonecode"));
            return result.GetAttributeValue<int>("timezonecode");
        }
    }
}
