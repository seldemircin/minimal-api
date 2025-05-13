using ch_16_jwt.Entities.DTOs.Books;

namespace ch_16_jwt.Abstracts;

public interface IBookService
{
    int Count { get; } // readonly prop
    List<BookDto> GetBooks();
    BookDto GetBookById(int id);
    BookDto AddBook(BookDtoForInsertion? item);
    BookDto UpdateBook(int id, BookDtoForUpdate? item);
    void DeleteBookById(int id);
}