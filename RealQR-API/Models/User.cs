﻿using System.ComponentModel.DataAnnotations;

namespace RealQR_API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool IsUserAdmin { get; set; }
        public ICollection<Enquiry> Enquiries { get; set; }
    }
}
