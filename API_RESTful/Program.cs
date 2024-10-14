var builder = WebApplication.CreateBuilder(args);

// AGREGA LOS CONTROLADORES DE LA API:
builder.Services.AddControllers();

// DOCUMENTACION CON Swagger PARA TESTEAR:
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

app.UseAuthorization();

app.MapControllers();

app.Run();
