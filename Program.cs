using chairs_dotnet7_api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapGet("/", GetChairs);
chairs.MapPost("/",PostChair);
chairs.MapGet("/{nombre}",GetChairByName);
chairs.MapPut("/{id}",PutChair);
chairs.MapPut("/{id}/stock", IncrementStock);
chairs.MapPost("/purchase",ShopChair);
chairs.MapDelete("/{id}",DeleteChair);

app.Run();

//TODO: ENDPOINTS SOLICITADOS
static IResult GetChairs(DataContext db)
{
    return TypedResults.Ok(db.Chairs.ToArray());
}

static IResult PostChair(DataContext db)
{   
    var existingChair = db.Chairs.Find();
    if(existingChair == null){
        return TypedResults.NotFound("La silla ya existe");
    }
    db.Chairs.Add(existingChair);
    db.SaveChanges();
    return TypedResults.Created($"/{existingChair.Id}",existingChair);
}

static IResult GetChairByName(string nombre, DataContext db)
{
    var existingChair = db.Chairs.Find(nombre);
    if (existingChair == null){
        return TypedResults.NotFound("La silla no existe");
    }
    return TypedResults.Ok();
}

static IResult PutChair(int id,DataContext db)
{
    var existingChair = db.Chairs.Find(id);
    if(existingChair == null){
        return TypedResults.NotFound("La silla no existe");
    }
    existingChair.Nombre = existingChair.Nombre;

    db.SaveChanges();
    return TypedResults.Ok();

}

static IResult IncrementStock(int stock,DataContext db)
{

    return TypedResults.Ok();
}

static IResult ShopChair(DataContext db)
{
    return TypedResults.Ok();
}

static IResult DeleteChair(int id,DataContext db)
{
    var existingChair = db.Chairs.Find(id);

    if (existingChair == null){
        return TypedResults.NotFound("La silla que quiere eliminar no existe");
    }

    db.Chairs.Remove(existingChair);
    db.SaveChanges();
    return TypedResults.NoContent();
}