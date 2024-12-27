using Microsoft.EntityFrameworkCore;

namespace Books.GRPC.DbContext;

public class BooksContext : Microsoft.EntityFrameworkCore.DbContext
{
	public BooksContext(DbContextOptions<BooksContext> options) : base(options)
	{
	}
	public DbSet<Book> Books { get; set; }

}