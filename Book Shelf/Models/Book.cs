namespace Book_Shelf.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string ImageURL { get; set; } = null!;
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

   
    }
}