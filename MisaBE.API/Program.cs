using MisaBE.BL.GridConfig;
using MisaBE.BL.Organization;
using MisaBE.BL.SalaryComposition;
using MisaBE.BL.SalaryCompositionSystem;
using MisaBE.DL.GridConfig;
using MisaBE.DL.Organization;
using MisaBE.DL.SalaryComposition;
using MisaBE.DL.SalaryCompositionSystem;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'");

// ---- DL (Data Layer) ----
builder.Services.AddScoped<ISalaryCompositionDL>(_ => new SalaryCompositionDL(connStr));
builder.Services.AddScoped<ISalaryCompositionSystemDL>(_ => new SalaryCompositionSystemDL(connStr));
builder.Services.AddScoped<IOrganizationDL>(_ => new OrganizationDL(connStr));
builder.Services.AddScoped<IGridConfigDL>(_ => new GridConfigDL(connStr));

// ---- BL (Business Logic Layer) ----
builder.Services.AddScoped<ISalaryCompositionBL, SalaryCompositionBL>();
builder.Services.AddScoped<ISalaryCompositionSystemBL, SalaryCompositionSystemBL>();
builder.Services.AddScoped<IOrganizationBL, OrganizationBL>();
builder.Services.AddScoped<IGridConfigBL, GridConfigBL>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MISA Payroll API", Version = "v1" });
});

builder.Services.AddCors(opt =>
    opt.AddPolicy("AllowFE", p =>
        p.WithOrigins(
            "http://localhost:5173",   // Vite dev server
            "http://localhost:3000"
          )
         .AllowAnyHeader()
         .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFE");
app.UseAuthorization();
app.MapControllers();
app.Run();
