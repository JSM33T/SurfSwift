using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfSwift.WorkerService.Models
{
    [Table("tblActionTemplate")]  // Ensure table name is correct
    public class ActionTemplate
    {
        [Key]
        public int ActionTemplateId { get; set; }
        public string TemplateName { get; set; }
        public string ActionJson { get; set; }

        public bool IsPublished { get; set; }

        public int ActionProjectId { get; set; }  // This should match your foreign key in DB

        [ForeignKey("ActionProjectId")]
        public ActionProject Project { get; set; }
    }
}
