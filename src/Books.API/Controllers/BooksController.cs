﻿using Books.GRPC;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
	[ApiController]
	public class BooksController : ControllerBase
	{
		private readonly BookService.BookServiceClient _bookServiceClient;

		public BooksController(BookService.BookServiceClient bookServiceClient)
		{
			_bookServiceClient = bookServiceClient;
		}
		// GET: /Books
		[HttpGet]
		[Route("Books")]
		public async Task<IActionResult> GetBooks()
		{
			var response = _bookServiceClient.GetBooks(new GetBooksRequest());
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