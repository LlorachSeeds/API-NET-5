using System;

namespace Domain.Users
{
    public class UserSettings
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public CommunicationPreference CommunicationPreference { get; set; }

        public User User { get; set; }

        public bool IsDeleted { get; set; }

        public UserSettings(string value)
        {
            Value = value;
        }

        public UserSettings()
        {
        }
    }
}