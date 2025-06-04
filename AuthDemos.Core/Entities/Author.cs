using System.ComponentModel.DataAnnotations;
namespace AuthDemos.Core.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
