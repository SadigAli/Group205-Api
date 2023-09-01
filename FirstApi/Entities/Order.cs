namespace FirstApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public double Total { get; set; } = 0;
        public DateTime Date { get; set; }
        public string User { get; set; }
        public Status OrderStatus { get; set; } = Status.Pending;
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }

    public enum Status
    {
        Pending,
        Accepted,
        Rejected
    }
}
