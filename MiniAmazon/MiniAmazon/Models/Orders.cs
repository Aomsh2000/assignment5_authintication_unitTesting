using System.Text.Json.Serialization;

namespace MiniAmazon.Models
{
    public class Orders
    {
        ///OrderID, UserID, OrderDate, TotalAmount, Status
        public int OrderID { get; set; }
        public int UserID { get; set; }// FK
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }

        [JsonIgnore]
        // Navigation property for User and OrderItems
        public Users User { get; set; }
        [JsonIgnore]
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
