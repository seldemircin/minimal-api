using ch_13_configuration.Entities;
using ch_13_configuration.Entities.DTOs;

namespace ch_13_configuration.Abstracts;

public interface IBookService
{
    int Count { get; } // readonly prop
    List<Book> GetBooks();
    Book GetBookById(int id);
    Book AddBook(BookDtoForInsertion? item);
    Book UpdateBook(int id, BookDtoForUpdate? item);
    void DeleteBookById(int id);
}