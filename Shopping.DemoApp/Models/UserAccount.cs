namespace TeamWork.Models
{
    using System;
    using System.Collections.Generic;

    public class UserAccount 
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

    }
}
