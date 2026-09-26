var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var api = app.MapGroup("/api");

//? Simula una base de datos
List<User> users = [
  new User(Guid.NewGuid(),"Juan", "Rodríguez", 22),
  new User(Guid.NewGuid(),"María", "Sánchez", 18),
  new User(Guid.NewGuid(),"Julián", "Pérez", 32),
  new User(Guid.NewGuid(),"María", "Sánchez Rosario", 29),
];


api.MapGet("/", () => {
  return new { Message = "Bienvenido a mi Api" };
});

api.MapGet("/users", () => {
  return users;
});

api.MapPost("/users", (User user) => {
  var newUser = user with { Uuid = Guid.NewGuid() };

  users.Add(newUser);

  return newUser;
});

api.MapGet("/user/{uuid}", (Guid uuid) => {
  var user = users.FirstOrDefault((u) => u.Uuid == uuid);
  if (user is null) {
    return Results.NotFound(new { Message = "Usuario no encontrado" });
  }

  return Results.Ok(new { User = user });
});

// TODO: Implementar UPDATE y DELETE

api.MapDelete("/user/{uuid}", (Guid uuid) => {
  return new NotImplementedException();
});

api.MapPatch("/user/{uuid}", (Guid uuid, User user) => {
  return new NotImplementedException();
});

app.Run("http://localhost:3000");


// Modelo
record User(Guid Uuid, string Nombre, string Apellido, int Edad);
