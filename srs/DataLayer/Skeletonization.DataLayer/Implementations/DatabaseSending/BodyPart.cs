using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skeletonization.DataLayer.Implementations.DatabaseSending
{
    [Table("bodyparts", Schema = "public")]
    public class BodyPart
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
