using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab6Cs.Models
{
    public class Student : StudentBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "StudentID")]
        public Guid ID { get; set; }
    }
}
