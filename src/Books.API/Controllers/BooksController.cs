using Books.GRPC;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
	[ApiController]
	[Route("Books")]
	public class BooksController : ControllerBase
	{
		private readonly BookService.BookServiceClient _bookServiceClient;

		public BooksController(IConfiguration configuration)
		{
			var channel = GrpcChannel.ForAddress(configuration.GetValue<string>("GrpcBooksService:BaseUrl"));
			_bookServiceClient = new BookService.BookServiceClient(channel);
		}

		[HttpGet]
		public async Task<IActionResult> GetBooks(int limit, int page)
		{
			var response = _bookServiceClient.GetBooks(new GetBooksRequest { Limit = limit, Page = page });
			var books = new List<BookUnit>();
			await foreach (var book in response.ResponseStream.ReadAllAsync())
			{
				books.Add(book);
			}
			return Ok(books);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetBook(int id)
		{
			var response = await _bookServiceClient.GetBookAsync(new GetBookRequest { Id = id });
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> AddBook(AddBookRequest request)
		{
			var response = await _bookServiceClient.AddBookAsync(request);
			return Ok(response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBook(int id, UpdateBookRequest request)
		{
			request.Id = id; // Ensure the request has the correct ID
			var response = await _bookServiceClient.UpdateBookAsync(request);
			return Ok(response);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(int id)
		{
			var response = await _bookServiceClient.DeleteBookAsync(new DeleteBookRequest { Id = id });
			return Ok(response);
		}

		[HttpPost("FromExcel")]
		public async Task<IActionResult> CreateBooksFromExcel(IFormFile excelFile)
		{
			var request = new AddBooksFromExcelRequest();
			using (var ms = new MemoryStream())
			{
				await excelFile.CopyToAsync(ms);
				request.File = ByteString.CopyFrom(ms.ToArray());
			}
			var response = await _bookServiceClient.AddBooksFromExcelAsync(request);
			return Ok(response);
		}


	}
}
