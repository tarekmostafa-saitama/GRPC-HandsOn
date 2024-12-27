using Grpc.Core;

namespace Books.GRPC.Services;

public class BookService: GRPC.BookService.BookServiceBase
{
	public BookService()
	{
		
	}

	public override Task<Book> GetBook(GetBookRequest request, ServerCallContext context)
	{
		return base.GetBook(request, context);
	}

	public override Task GetBooks(GetBooksRequest request, IServerStreamWriter<Book> responseStream, ServerCallContext context)
	{
		return base.GetBooks(request, responseStream, context);
	}

	public override Task<AddBookResponse> AddBook(AddBookRequest request, ServerCallContext context)
	{
		return base.AddBook(request, context);
	}

	public override Task<UpdateBookResponse> UpdateBook(UpdateBookRequest request, ServerCallContext context)
	{
		return base.UpdateBook(request, context);
	}

	public override Task<DeleteBookResponse> DeleteBook(DeleteBookRequest request, ServerCallContext context)
	{
		return base.DeleteBook(request, context);
	}
}