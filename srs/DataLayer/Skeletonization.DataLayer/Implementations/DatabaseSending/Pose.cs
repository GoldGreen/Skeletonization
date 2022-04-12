using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skeletonization.DataLayer.Implementations.DatabaseSending
{
    [Table("poses", Schema = "public")]
    public class Pose
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
