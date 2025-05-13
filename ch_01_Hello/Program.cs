var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


string message = "Hello World!";

app.MapGet("/hello", () =>
{
    return new Response(message);
});



app.UseHttpsRedirection();
app.Run();


class Response
{
    public Response(string msg)
    {
        Message = msg;
    }
    public string Message { get; set; }
    public DateTime Date => DateTime.Now;
}