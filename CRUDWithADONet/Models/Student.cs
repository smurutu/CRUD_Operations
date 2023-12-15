using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDWithADONet.Models
{
	public class Student
	{
        [Key]
        public Int64 id { get; set; }

        [DisplayName("First Name")]
        [Required]

        public string? first_name { get; set; }

        [DisplayName("Middle Name")]
        [Required]

        public string? middle_name { get; set; }

        [DisplayName("Last Name")]
        [Required]

        public string? last_name { get; set; }

        [DisplayName("Date Of Birth")]
        [Required]

        public DateTime date_of_birth { get; set; }

        [DisplayName("Course Name")]
        [Required]

        public string? course_name { get; set; }

        [NotMapped]
        public string full_name
        {
            get { return first_name + " " + middle_name + " " + last_name; }
        }
    }
}

