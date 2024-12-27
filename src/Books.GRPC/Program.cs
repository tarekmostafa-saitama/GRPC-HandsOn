using Books.GRPC.DbContext;
using Books.GRPC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();


builder.Services.AddDbContext<BooksContext>(options =>
{
	options.UseInMemoryDatabase("Books");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<BookService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
