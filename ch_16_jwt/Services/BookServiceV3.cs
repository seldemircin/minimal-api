using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ch_16_jwt.Abstracts;
using ch_16_jwt.Configuration;
using ch_16_jwt.Entities;
using ch_16_jwt.Entities.DTOs.Books;
using ch_16_jwt.Entities.Exceptions;
using ch_16_jwt.Repositories;

namespace ch_16_jwt.Services;

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

    public List<BookDto> GetBooks()
    {
        var books = _bookRepository.GetAll();
        return _mapper.Map<List<BookDto>>(books);
    }

    public BookDto GetBookById(int id)
    {
        id.ValidateIdInRange();
        
        var book = _bookRepository.Get(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }
        
        return _mapper.Map<BookDto>(book);
    } 

    public BookDto AddBook(BookDtoForInsertion? newBook)
    {
        if (newBook == null)
        {
            throw new ArgumentException("Eklenmek istenen book nesnesi null olamaz.");
        }

        Validate(newBook);
        
        var book = _mapper.Map<Book>(newBook);
        _bookRepository.Add(book);
        return _mapper.Map<BookDto>(_bookRepository.Get(book.Id));
    } 
    public BookDto UpdateBook(int id, BookDtoForUpdate? item)
    {
        id.ValidateIdInRange();
    
        if (item == null)
        {
            throw new ArgumentException("Güncellenmek istenen kitap null olamaz.");
        }
    
        Validate(item);
        
        var book = _bookRepository.Get(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }
        
        _mapper.Map(item, book);
        _bookRepository.Update(book);
        
        return _mapper.Map<BookDto>(_bookRepository.Get(id));
    }

    public void DeleteBookById(int id)
    {
        id.ValidateIdInRange();
        
        var book = _bookRepository.Get(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }
        
        _bookRepository.Delete(book);
    }

    private void Validate<T>(T item)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(item!);
        var isValid = Validator.TryValidateObject(item!, context, validationResults, true);

        if (!isValid)
        {
            var errors = string.Join(" , ", validationResults);
            throw new ValidationException(errors);
        }
    }
}