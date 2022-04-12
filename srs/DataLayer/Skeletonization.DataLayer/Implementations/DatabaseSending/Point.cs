using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skeletonization.DataLayer.Implementations.DatabaseSending
{
    [Table("points", Schema = "public")]
    public class Point
    {
        [Key]
        public int Id { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public BodyPart BodyPart { get; set; }
    }
}
