using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ch_12_auto_mapper.Abstracts;
using ch_12_auto_mapper.Entities;
using ch_12_auto_mapper.Entities.DTOs;
using ch_12_auto_mapper.Entities.Exceptions;
using ch_12_auto_mapper.Repositories;

namespace ch_12_auto_mapper.Services;

public class BookServiceV3 : IBookService
{
    private readonly BookRepository _bookRepository;
    private readonly IMapper _mapper;
    
    public BookServiceV3(BookRepository bookRepository,IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public int Count => _bookRepository.GetAll().Count;
    public List<Book> GetBooks() => _bookRepository.GetAll();

    public Book? GetBookById(int id)
    {
        if (!(id > 0 && id <= 1000))
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID 1 ile 1000 arasında olmalıdır.");
        }
        return _bookRepository.Get(id);
    } 

    public Book AddBook(BookDtoForInsertion? newBook)
    {
        if (newBook == null)
        {
            throw new ArgumentException("Eklenmek istenen book nesnesi null olamaz.");
        }

        var validationResult = new List<ValidationResult>();
        var context = new ValidationContext(newBook);
        var isValid = Validator.TryValidateObject(newBook, context, validationResult,true);

        if (!isValid)
        {
            var error = string.Join(" , ", validationResult);
            throw new ValidationException(error);
        }
        
        var book = _mapper.Map<Book>(newBook);
        _bookRepository.Add(book);
        return book;
    } 
    public Book UpdateBook(int id, BookDtoForUpdate? item)
    {
        if (!(id > 0 && id <= 1000))
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID 1 ile 1000 arasında olmalıdır.");
        }
    
        if (item == null)
        {
            throw new ArgumentException("Güncellenmek istenen kitap null olamaz.");
        }
    
        var validationResult = new List<ValidationResult>();
        var context = new ValidationContext(item);
        var isValid = Validator.TryValidateObject(item, context, validationResult, true);

        if (!isValid)
        {
            var error = string.Join(" , ", validationResult);
            throw new ValidationException(error);
        }
        
        var book = _bookRepository.Get(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }

        _mapper.Map(item, book);
        _bookRepository.Update(book);
        return book;
    }

    public void DeleteBookById(int id)
    {
        if (!(id > 0 && id <= 1000))
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID 1 ile 1000 arasında olmalıdır.");
        }

        var book = _bookRepository.Get(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }
        _bookRepository.Delete(book);
    }
}