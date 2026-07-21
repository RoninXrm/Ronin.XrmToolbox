namespace Ronin.XrmToolbox.UserTimezoneManager.Models
{
    /// <summary>
    /// Represents a single timezone entry retrieved from the Dataverse <c>timezonedefinition</c> entity.
    /// </summary>
    public class TimezoneOption
    {
        /// <summary>The numeric code stored on <c>usersettings.timezonecode</c>.</summary>
        public int TimezoneCode { get; set; }

        /// <summary>The human-readable name shown in the UI (from <c>userinterfacename</c>).</summary>
        public string DisplayName { get; set; }

        /// <summary>The Windows/standard timezone name (from <c>standardname</c>).</summary>
        public string StandardName { get; set; }

        public override string ToString() => DisplayName;
    }
}
