using Books.GRPC;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
	[ApiController]
	public class BooksController : ControllerBase
	{
		private readonly BookService.BookServiceClient _bookServiceClient;

		public BooksController(IConfiguration configuration)
		{
			var channel = GrpcChannel.ForAddress(configuration.GetValue<string>("GrpcBooksService:BaseUrl"));
			_bookServiceClient = new BookService.BookServiceClient(channel);
		}
		// GET: /Books
		[HttpGet]
		[Route("Books")]
		public async Task<IActionResult> GetBooks(int limit, int page)
		{
			var response = _bookServiceClient.GetBooks(new GetBooksRequest()
				{Limit = limit, Page = page});
			var books = new List<BookUnit>();
			await foreach (var book in response.ResponseStream.ReadAllAsync())
			{
				books.Add(book);
			}
			return Ok(books);
		}
		// GET: /Books/1
		[HttpGet]
		[Route("Books/{id}")]
		public async Task<IActionResult> GetBook(int id)
		{
			var response = await _bookServiceClient.GetBookAsync(new GetBookRequest { Id = id });
			return Ok(response);
		}
		// POST: /Books
		[HttpPost]
		[Route("Books")]
		public async Task<IActionResult> AddBook(AddBookRequest request)
		{
			var response = await _bookServiceClient.AddBookAsync(request);
			return Ok(response);
		}
		// PUT: /Books/1
		[HttpPut]
		[Route("Books/{id}")]
		public async Task<IActionResult> UpdateBook(int id, UpdateBookRequest request)
		{
			var response = await _bookServiceClient.UpdateBookAsync(request);
			return Ok(response);
		}
		// DELETE: /Books/1
		[HttpDelete]
		[Route("Books/{id}")]
		public async Task<IActionResult> DeleteBook(int id)
		{
			var response = await _bookServiceClient.DeleteBookAsync(new DeleteBookRequest { Id = id });
			return Ok(response);
		}

	}
}
