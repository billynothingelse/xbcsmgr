using System;
using Newtonsoft.Json;

namespace XboxCsMgr.XboxLive.Account
{
    public class AccountDetails
    {
        public DateTime DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Gamertag { get; set; }

        public ulong OwnerXuid { get; set; }

        public bool IsAdult { get; set; }

        public string Locale { get; set; }

        public ulong OwnerPuid { get; set; }

        public ulong Puid { get; set; }
    }
}
