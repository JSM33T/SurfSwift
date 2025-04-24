using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfSwift.WorkerService.Models
{
    [Table("tblActionProject")]
    public class ActionProject
        {
            [Key]
            public int ActionProjectId { get; set; }
            public string ProjectName { get; set; }
            public bool IsPublished { get; set; }

            public int CreatedByUserId { get; set; }
            [ForeignKey(nameof(CreatedByUserId))]
            public User CreatedByUser { get; set; }

            public ICollection<ActionTemplate> ActionTemplates { get; set; }
        }

}
