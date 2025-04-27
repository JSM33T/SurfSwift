using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfSwift.DBEngine.Models
{
    [Table("tblUser")]
    public class User
    {
        [Key]
        public int UserId { get; set; }  // Change long to int to match DB schema
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public bool IsValidUser { get; set; }

        public ICollection<ActionProject> Project { get; set; }
    }
}
