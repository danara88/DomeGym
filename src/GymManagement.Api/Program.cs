using GymManagement.Application;
using GymManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configure ProblemDetails middleware. Establish standard format for
    // returning deatails about HTTP errors.
    builder.Services.AddProblemDetails();

    // Add dependency injection modules
    builder.Services
        .AddApplication()
        .AddInfrastructure();
}

var app = builder.Build();
{
    // Wraps everything in a try catch if something goes worng
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
