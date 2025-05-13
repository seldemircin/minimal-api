var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var lambdaVariable = () => "Hello World!";

string LocalFunction()
{
    return "Hello World!";
}

app.MapGet("/hello", () => "Hello World!"); // lamba handler.
app.MapPost("/hello", lambdaVariable); // lambda variable handler.
app.MapPut("/hello", LocalFunction); // local function handler.
app.MapDelete("/hello", new HelloHandler().Hello); // instance member handler.


app.UseHttpsRedirection();
app.Run();


class HelloHandler
{
    public string Hello()
    {
        return "Hello World!";
    }
}