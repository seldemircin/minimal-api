namespace ch_11_repo_in_use.Abstracts;

public interface IBookService
{
    int Count { get; } // readonly prop
    List<Book> GetBooks();
    Book? GetBookById(int id);
    void AddBook(Book item);
    Book UpdateBook(int id, Book item);
    void DeleteBookById(int id);
}