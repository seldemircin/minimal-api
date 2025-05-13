using ch_15_identity.Entities.DTOs;

namespace ch_15_identity.Abstracts;

public interface IBookService
{
    int Count { get; } // readonly prop
    List<BookDto> GetBooks();
    BookDto GetBookById(int id);
    BookDto AddBook(BookDtoForInsertion? item);
    BookDto UpdateBook(int id, BookDtoForUpdate? item);
    void DeleteBookById(int id);
}