var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/employees", () => new Embloyee().GetAllEmployees());  
app.MapGet("/employees/search", (string q) => Embloyee.Search(q)); // localhost/employees/search?q=....
app.MapGet("/employees/{id:int}", (int id) => new Embloyee().GetEmbloyeeById(id)); // localhost/employees/{id}
app.MapPost("/employees", (Embloyee emp) => Embloyee.CreateOneEmbloyee(emp)); 

app.UseHttpsRedirection();
app.Run();


class Embloyee
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public decimal Salary { get; set; }
    
    private static List<Embloyee> Embloyees = new List<Embloyee>()
    {
        new Embloyee(){Id = 1,FullName = "Ahmet Güneş",Salary = 50000},
        new Embloyee(){Id = 2,FullName = "Selahattin Demirçin",Salary = 275000},
        new Embloyee(){Id = 3,FullName = "Sıla Bulut",Salary = 75000}
    };

    public List<Embloyee> GetAllEmployees() => Embloyees;
    public Embloyee? GetEmbloyeeById(int id) => Embloyees.SingleOrDefault(e => e.Id == id);
    public static Embloyee CreateOneEmbloyee(Embloyee embloyee)
    {
        Embloyees.Add(embloyee);
        return embloyee;
    }

    public static List<Embloyee> Search(string q) =>
        Embloyees.Where(e => e.FullName != null && e.FullName.ToLower().Contains(q.ToLower())).ToList();
}

