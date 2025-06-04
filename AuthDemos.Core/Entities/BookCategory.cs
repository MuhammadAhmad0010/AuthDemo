using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthDemos.Core.Entities;

public class BookCategory
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }
    public Books Book { get; set; }
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
