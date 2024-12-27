using Books.GRPC.DbContext;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Books.GRPC.Services;

public class BookService: GRPC.BookService.BookServiceBase
{
	private readonly BooksContext _booksContext;

	public BookService(BooksContext booksContext)
	{
		_booksContext = booksContext;
	}

	public override async Task<BookUnit> GetBook(GetBookRequest request, ServerCallContext context)
	{
		// Get the book from the database
		var book = await _booksContext.Books.FindAsync(request.Id);
		if (book == null)
		{
			throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
		}
		// return the book
		return new BookUnit
		{
			Id = book.Id,
			Title = book.Title,
			Author = book.Author,
			Description = book.Description,
			Image = book.Image
		};
	}

	public override async Task GetBooks(GetBooksRequest request, IServerStreamWriter<BookUnit> responseStream, ServerCallContext context)
	{
		// Get all the books from the database
		var books = await _booksContext.Books.AsNoTracking().ToListAsync();
		foreach (var book in books)
		{
			await responseStream.WriteAsync(new BookUnit
			{
				Id = book.Id,
				Title = book.Title,
				Author = book.Author,
				Description = book.Description,
				Image = book.Image
			});
		}

	}

	public override async Task<AddBookResponse> AddBook(AddBookRequest request, ServerCallContext context)
	{
		// Add the book to the database
		var book = new Book
		{
			Title = request.Title,
			Author = request.Author,
			Description = request.Description,
			Image = request.Image
		};
		_booksContext.Books.Add(book);
		await _booksContext.SaveChangesAsync();
		// return the response
		return new AddBookResponse
		{
			Id = book.Id
		};
	}

	public override async Task<UpdateBookResponse> UpdateBook(UpdateBookRequest request, ServerCallContext context)
	{
		// Get the book from the database
		var book = await _booksContext.Books.FindAsync(request.Id);
		if (book == null)
		{
			throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
		}
		// Update the book
		book.Title = request.Title;
		book.Author = request.Author;
		book.Description = request.Description;
		book.Image = request.Image;
		await _booksContext.SaveChangesAsync();
		// return the response
		return new UpdateBookResponse
		{
			Success = true
		};

	}

	public override async Task<DeleteBookResponse> DeleteBook(DeleteBookRequest request, ServerCallContext context)
	{
		// Get the book from the database
		var book = await _booksContext.Books.FindAsync(request.Id);
		if (book == null)
		{
			throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
		}
		// Delete the book
		_booksContext.Books.Remove(book);
		await _booksContext.SaveChangesAsync();
		// return the response
		return new DeleteBookResponse
		{
			Success = true
		};

	}
}