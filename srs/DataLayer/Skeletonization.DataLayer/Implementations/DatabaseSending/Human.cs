using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skeletonization.DataLayer.Implementations.DatabaseSending
{
    [Table("humans", Schema = "public")]
    public class Human
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Point> Points { get; set; }
        public Pose Pose { get; set; }
    }
}
