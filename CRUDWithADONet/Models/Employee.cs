using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDWithADONet.Models
{
	public class Employee
	{
		[Key]
		public Int64 id { get; set; }

        [DisplayName("First Name")]
        [Required]

        public string? first_name{ get; set; }

        [DisplayName("Last Name")]
        [Required]

        public string? last_name { get; set; }

        [DisplayName("Date Of Birth")]
        [Required]

        public DateTime date_of_birth { get; set; }

        [DisplayName("E-mail")]
        [Required]

        public string? email { get; set; }
        [Required]

        public double salary { get; set; }

        [NotMapped]
        public string full_name
        {
            get { return first_name + " " + last_name; }
        }
    }
}

