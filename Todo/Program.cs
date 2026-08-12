using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddOpenApi();
var app = builder.Build();


if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

var todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapPatch("/{id}", PatchTodo);
todoItems.MapDelete("/{id}", DeleteTodo);

app.Run();

static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToListAsync());
}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id) is Todo todo ? 
        TypedResults.Ok(new TodoItemDTO(todo)) :
        TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, TodoDb db)
{

    var todo = new Todo
    {
        IsComplete = todoItemDTO.IsComplete,
        Name = todoItemDTO.Name,
    };

    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    todoItemDTO = new TodoItemDTO(todo);

    return TypedResults.Created($"/todoitems/{todo.Id}", todoItemDTO);
}

static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDto, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);

    if(todo is null) return TypedResults.NotFound();

    todo.Name = todoItemDto.Name;
    todo.IsComplete = todoItemDto.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

/// <summary>
/// Patch a todo item
/// </summary>
/// <param name="id">The id of the todo item to patch</param>
/// <param name="inputPatch">The patch to apply to the todo item</param>
/// <param name="db">The database context</param>
/// <returns>A result indicating the success of the operation</returns>
static async Task<IResult> PatchTodo(int id, TodoPatch inputPatch, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);

    if(todo is null) return TypedResults.NotFound();

    if(inputPatch.Name is not null) todo.Name = inputPatch.Name;
    if(inputPatch.IsComplete is not null) todo.IsComplete = inputPatch.IsComplete.Value;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

/// <summary>
/// Delete a todo item
/// </summary>
/// <param name="id">The id of the todo item to delete</param>
/// <param name="db">The database context</param>
/// <returns>A result indicating the success of the operation</returns>
static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if(await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}