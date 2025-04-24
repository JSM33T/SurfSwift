using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfSwift.WorkerService.Models
{
    [Table("tblActionTemplate")]
    public class ActionTemplate
    {
        [Key]
        public int ActionTemplateId { get; set; }
        public string TemplateName { get; set; }
        public string ActionJson { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public ActionProject Project { get; set; }
    }

}
