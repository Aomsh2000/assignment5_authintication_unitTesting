using System.Text.Json.Serialization;

namespace MiniAmazon.Models
{
    public class OrderItems
    {
        //OrderItemID, OrderID, ProductID, Quantity, Price
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        // Navigation properties for Order and Product
        [JsonIgnore]
        public Orders Order { get; set; }
        [JsonIgnore]
        public Products Product { get; set; }

    }
}
