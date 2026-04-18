namespace LKM_PAA.Models
{
    public class Borrowing
    {
        public int user_id { get; set; }
        public int book_id { get; set; }
        public DateTime borrow_date { get; set; }
        public DateTime? return_date { get; set; }
    }
}
