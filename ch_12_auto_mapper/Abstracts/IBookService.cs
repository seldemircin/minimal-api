using ch_12_auto_mapper.Entities;
using ch_12_auto_mapper.Entities.DTOs;

namespace ch_12_auto_mapper.Abstracts;

public interface IBookService
{
    int Count { get; } // readonly prop
    List<Book> GetBooks();
    Book? GetBookById(int id);
    Book AddBook(BookDtoForInsertion? item);
    Book UpdateBook(int id, BookDtoForUpdate? item);
    void DeleteBookById(int id);
}