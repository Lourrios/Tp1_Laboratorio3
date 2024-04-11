namespace Tp1Laboratorio3
{
    public class Book
    {
        private string Title { get; set; }
        private int Pages { get; set; }
        private string Author { get; set; }
        public Book() { }
        public Book(string title, int page, string author) { Title = title; Pages = page; Author = author; }

    }
}
