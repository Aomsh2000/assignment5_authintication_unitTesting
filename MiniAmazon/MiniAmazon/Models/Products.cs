using System.Text.Json.Serialization;

namespace MiniAmazon.Models
{
    public class Products
    {
        ///ProductID, Name, Description, Price, Stock, CreatedBy
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description  { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string CreatedBy { get; set; }

        // Navigation property for the relationship between Product and OrderItems
        [JsonIgnore]
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
