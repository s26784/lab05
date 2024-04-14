using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var _animals = new List<Animal>
{
    new Animal { IdAnimal = 1, Name = "Pusheck", Category = "Cat", Color = "Grey", Weight = 6.1 },
    new Animal { IdAnimal = 2, Name = "Otamendi", Category = "Dog", Color = "White", Weight = 15.3 },
    new Animal { IdAnimal = 3, Name = "Dotter", Category = "Hamster", Color = "Brown", Weight = 0.18 },
    new Animal { IdAnimal = 4, Name = "Toady", Category = "Snake", Color = "White", Weight = 15.3 },
    new Animal { IdAnimal = 5, Name = "Dillon", Category = "Dog", Color = "White", Weight = 15.3 }
};

var _visits = new List<Visit>
{
    new Visit { IdVisit = 1, IdAnimal = 1, Date = DateTime.Parse("2018-01-11"), Description = "Regular checkup", Price = 50.00 },
    new Visit { IdVisit = 2, IdAnimal= 2, Date = DateTime.Parse("2020-03-23"), Description = "Tick removal", Price = 100.00 },
    new Visit { IdVisit = 3, IdAnimal = 2, Date = DateTime.Parse("2022-04-05"), Description = "Fleas removal", Price = 120.00 },
    new Visit { IdVisit = 4, IdAnimal = 2, Date = DateTime.Parse("2023-08-08"), Description = "Regular checkup", Price = 65.00 },
    new Visit { IdVisit = 5, IdAnimal = 5, Date = DateTime.Parse("2024-11-11"), Description = "Gastric lavage", Price = 200.00 }
};

foreach (var visit in _visits)
{
    var animal = _animals.FirstOrDefault(a => a.IdAnimal == visit.IdAnimal);
    if (animal != null)
    {
        animal.Visits.Add(visit);
    }
}

app.MapGet("/api/animals", () => Results.Ok(_animals))
    .WithName("GetAnimals")
    .WithOpenApi();

app.MapGet("/api/animals/{id:int}", (int id) =>
    {
        var animal = _animals.FirstOrDefault(a => a.IdAnimal == id);
        if (animal == null)
        {
            return Results.NotFound($"Animal with id {id} was not found");
        }
        return Results.Ok(animal);
    })
    .WithName("GetAnimalById")
    .WithOpenApi();

app.MapPost("/api/animals", (Animal animal) =>
    {
        _animals.Add(animal);
        return Results.StatusCode(StatusCodes.Status201Created);
    })
    .WithName("CreateAnimal")
    .WithOpenApi();

app.MapPut("/api/animals/{id:int}", (int id, Animal animal) =>
    {
        var animalToEdit = _animals.FirstOrDefault(a => a.IdAnimal == id);
        if (animalToEdit == null)
        {
            return Results.NotFound($"Animal with id {id} was not found");
        }
        _animals.Remove(animalToEdit);
        _animals.Add(animal);
        return Results.NoContent();
    })
    .WithName("UpdateAnimal")
    .WithOpenApi();

app.MapDelete("/api/animals/{id:int}", (int id) =>
    {
        var animalToDelete = _animals.FirstOrDefault(s => s.IdAnimal == id);
        if (animalToDelete == null)
        {
            return Results.NoContent();
        }
        _animals.Remove(animalToDelete);
        return Results.NoContent();
    })
    .WithName("DeleteAnimal")
    .WithOpenApi();


app.MapGet("/api/animals/{idAnimal:int}/visits", (int idAnimal) =>
{
    var animal = _animals.FirstOrDefault(a => a.IdAnimal == idAnimal);
    if (animal == null)
    {
        return Results.NotFound($"Animal with id {idAnimal} was not found");
    }
    return Results.Ok(animal.Visits);
})
.WithName("GetVisitsForAnimal")
.WithOpenApi();


app.MapPost("/api/animals/{idAnimal:int}/visits", (int idAnimal, Visit visit) =>
{
    var animal = _animals.FirstOrDefault(a => a.IdAnimal == idAnimal);
    if (animal == null)
    {
        return Results.NotFound($"Animal with id {idAnimal} was not found");
    }
    visit.IdVisit = _visits.Count + 1;
    visit.IdAnimal = idAnimal;
    _visits.Add(visit);
    animal.Visits.Add(visit);
    return Results.Created($"/api/visit/{idAnimal}/visits/{visit.IdVisit}", visit);
})
.WithName("CreateVisitForAnimal")
.WithOpenApi();


app.Run();
