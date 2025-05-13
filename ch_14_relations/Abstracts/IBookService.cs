using ch_14_relations.Entities;
using ch_14_relations.Entities.DTOs;

namespace ch_14_relations.Abstracts;

public interface IBookService
{
    int Count { get; } // readonly prop
    List<BookDto> GetBooks();
    BookDto GetBookById(int id);
    BookDto AddBook(BookDtoForInsertion? item);
    BookDto UpdateBook(int id, BookDtoForUpdate? item);
    void DeleteBookById(int id);
}