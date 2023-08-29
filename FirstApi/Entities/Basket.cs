namespace FirstApi.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string User { get; set; }
    }
}
