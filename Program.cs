var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var api = app.MapGroup("/api");

//? Simula una base de datos
List<User> users = [
  new User(){ Uuid = Guid.NewGuid(), FirstName = "Juan", LastName  = "Rodríguez", Age = 22 },
  new User(){ Uuid = Guid.NewGuid(), FirstName = "María", LastName  = "Sánchez", Age = 18 },
  new User(){ Uuid = Guid.NewGuid(), FirstName = "Julián", LastName  = "Pérez", Age = 32 },
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

api.MapGet("/user-ages", (int age) => {
  var userAges = users.FindAll(user => user.Age > age);

  if (userAges.Count <= 0) {
    return Results.NotFound(new { Message = "No existen usuarios con esos criterios" });
  }

  return Results.Ok(userAges);
});

api.MapDelete("/user/{uuid}", (Guid uuid) => {
  // LinQ
  var user = users.FirstOrDefault((usr) => usr.Uuid == uuid);

  if (user is null) {
    return Results.NotFound(new { Message = "Usuario no encontrado" });
  }

  users.Remove(user);

  return Results.NoContent();
});

api.MapPut("/user/{uuid}", (Guid uuid, User user) => {
  var oldUser = users.FirstOrDefault((usr) => usr.Uuid == uuid);

  if (oldUser is null) {
    return Results.NotFound(new { Message = "Usuario no encontrado" });
  }

  oldUser.FirstName = user.FirstName;
  oldUser.LastName = user.LastName;
  oldUser.Age = user.Age;


  return Results.NoContent();
});

app.Run("http://localhost:3000");


// Modelo
record User() {
  public Guid Uuid { get; init; }
  required public string FirstName { get; set; }
  required public string LastName { get; set; }
  required public int Age { get; set; }
};
