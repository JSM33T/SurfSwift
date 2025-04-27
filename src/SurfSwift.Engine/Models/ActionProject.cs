using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfSwift.WorkerService.Models
{
    [Table("tblActionProject")]  // Table name should match your DB table
    public class ActionProject
    {
        [Key]
        public int ActionProjectId { get; set; }  // Change long to int to match DB schema
        public string ProjectName { get; set; }
        public bool IsPublished { get; set; }
        public int UserId { get; set; }  // Correct this to match your DB column name
                                         // Navigation Property - User (the actual relationship)
        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<ActionTemplate> Template { get; set; }
    }
}
