using MinimalApisDemo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enabling Swagger middleware so we can actually see it in action.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var toDos = new List<ToDo>
{
    new ToDo { Id = 1, Task="Learn Minimal Apis" ,IsComplete = false },
    new ToDo { Id = 2, Task="Learn Minimal Apis 2" ,IsComplete = false }
};

//GetAll
app.MapGet("/todos", () => Results.Ok(toDos))
    .WithName("GetAllToDos")
    .WithTags("ToDos");

//GetById
app.MapGet("/todos/{id:int}", (int id) =>
{
    var todo = toDos.FirstOrDefault(t => t.Id == id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
})
   .WithName("GetToDoById")
   .WithTags("ToDos");

//Create
app.MapPost("/todos", (ToDo newToDo) =>
{
    newToDo.Id = toDos.Any() ? toDos.Max(t => t.Id) + 1 : 1;
    toDos.Add(newToDo);
    return Results.Created($"/todos/{newToDo.Id}", newToDo);
})
    .WithName("CreateToDo")
    .WithTags("ToDos");

//Update
app.MapPut("/todos/{id:int}", (int id, ToDo updateToDo) =>
{
    var todo = toDos.FirstOrDefault(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    todo.Task = updateToDo.Task;
    todo.IsComplete = updateToDo.IsComplete;

    return Results.NoContent();
})
    .WithName("UpdateToDo")
    .WithTags("ToDos");

//Delete
app.MapDelete("/todos/{id:int}", (int id) =>
{
    var todo = toDos.FirstOrDefault(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    toDos.Remove(todo);
    return Results.NoContent();
})
    .WithName("DeleteToDo")
    .WithTags("ToDos");


app.UseHttpsRedirection();

app.Run();
