﻿using System.ComponentModel.DataAnnotations;

namespace ChatZorro.Models
{
    public class UserModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
