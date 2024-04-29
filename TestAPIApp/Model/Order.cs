namespace TestAPIApp.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
