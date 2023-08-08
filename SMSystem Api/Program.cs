using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("connaction")));
builder.Services.AddScoped<IStudentApiRepository,StudentApiRepository>();
builder.Services.AddScoped<IDepartmentApiRepository,DepartmentApiRepository>();
builder.Services.AddScoped<ITeacherApiRepository,TeacherApiRepository>();
builder.Services.AddScoped<ISubjectApiRepository,SubjectApiRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
