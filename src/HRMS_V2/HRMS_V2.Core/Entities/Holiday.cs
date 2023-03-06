using HRMS_V2.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS_V2.Core.Entities
{
    public class Holiday : Entity, IAuditable, IRecordStatus
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Day { get; set; }

            [Column(TypeName = "char(1)")]
            public string RecordStatus { get; set; }
            public DateTime CreatedDate { get ; set ; }
            public int? CreatedBy { get ; set ; }
            public string? CreatedByName { get ; set ; }
            public DateTime? ModifiedDate { get ; set ; }
            public int? ModifiedBy { get ; set ; }
            public string? ModifiedByName { get ; set ; }
    }
}
