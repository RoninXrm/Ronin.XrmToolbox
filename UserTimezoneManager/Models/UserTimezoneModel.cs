using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ronin.XrmToolbox.UserTimezoneManager.Models
{
    /// <summary>
    /// Represents a Dataverse system user with timezone information, used for grid data binding.
    /// </summary>
    public class UserTimezoneModel : INotifyPropertyChanged
    {
        private string _currentTimezoneName;
        private int _currentTimezoneCode;
        private int _newTimezoneCode;
        private bool _isSelected;

        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        /// <summary>Whether this user is selected via the checkbox column.</summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>The numeric Dataverse timezone code currently saved for this user.</summary>
        public int CurrentTimezoneCode
        {
            get => _currentTimezoneCode;
            set
            {
                _currentTimezoneCode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>The display name of the current timezone (resolved from the timezone definition lookup).</summary>
        public string CurrentTimezoneName
        {
            get => _currentTimezoneName;
            set
            {
                _currentTimezoneName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The timezone code selected in the "New Timezone" combo box column.
        /// Initialised to <see cref="CurrentTimezoneCode"/> so the combo starts on the user's current value.
        /// </summary>
        public int NewTimezoneCode
        {
            get => _newTimezoneCode;
            set
            {
                _newTimezoneCode = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
