using System.ComponentModel.DataAnnotations;

namespace BillMealMVC.Models
{
    public class ItemCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public string Description { get; set; }
    }
}