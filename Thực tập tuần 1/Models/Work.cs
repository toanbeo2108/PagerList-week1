using System.ComponentModel.DataAnnotations;

namespace Thực_tập_tuần_1.Models
{
    public class Work
    {
        [Key]
        public Guid IdWork { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public float WorkTime { get; set; } // thời gian làm việc tính theo hour
        public int? Status { get; set; } //( 1.cần làm , 2.đang làm, 3.đã làm xong ) 
    }
}
