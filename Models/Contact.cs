using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Column(TypeName = "ntext")]
        public string Context { get; set; }
        public DateTime ClaimTime { get; set; }
    }
}
