using HRMS_V2.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_V2.Core.Entities
{
    public class Permissions : Entity, IAuditable, IRecordStatus
    {
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "char(1)")]
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
