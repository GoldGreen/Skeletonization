using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Skeletonization.DataLayer.Implementations.DatabaseSending
{
    [Table("reports", Schema = "public")]
    public class Report
    {
        [Key]
        public int Id { get; set; }

        public string Path { get; set; }
        public string Description { get; set; }

        public ICollection<Human> Humans { get; set; }
    }
}
