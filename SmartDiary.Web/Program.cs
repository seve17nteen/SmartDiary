using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartDiary.Web.Data;
using SmartDiary.Web.Models;
using Microsoft.AspNetCore.Identity;
using SmartDiary.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Добавляем контроллеры API
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<ITaskService, TaskService>();

// ========== ДОБАВЛЯЕМ JWT АУТЕНТИФИКАЦИЮ ==========
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

builder.Services.AddAuthorization();

// ========== ДОБАВЛЯЕМ CORS ==========
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    SeedData(context, userManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Сначала аутентификация
app.UseAuthorization();   // Потом авторизация

// ========== ДОБАВЛЯЕМ CORS МИДДЛВЭР ==========
app.UseCors("AllowReactApp");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=Index}/{id?}");

// Добавляем маршруты для API
app.MapControllers();

app.Run();

// функция для загрузки тестовых данных
static void SeedData(ApplicationDbContext context, UserManager<User> userManager)
{
    context.Database.EnsureCreated();

    if (context.Users.Any())
    {
        return;
    }

    // Создаем пользователя через UserManager (чтобы хэшировать пароль)
    var user = new User
    {
        UserName = "testuser",
        Email = "test@example.com"
    };

    var result = userManager.CreateAsync(user, "Test123!").GetAwaiter().GetResult();

    if (!result.Succeeded)
    {
        return;
    }

    var projects = new[]
    {
        new Project
        {
            Name = "Личные дела",
            Description = "Личные задачи",
            Color = "FF5733",
            OwnerId = user.Id
        },
        new Project
        {
            Name = "Работа",
            Description = "Рабочие задачи",
            Color = "33FF57",
            OwnerId = user.Id
        },
        new Project
        {
            Name = "Учеба",
            Description = "Учебные задачи",
            Color = "3357FF",
            OwnerId = user.Id
        }
    };

    context.Projects.AddRange(projects);
    context.SaveChanges();

    var tags = new[]
    {
        new Tag { Name = "Важное", OwnerId = user.Id },
        new Tag { Name = "Срочное", OwnerId = user.Id },
        new Tag { Name = "Идея", OwnerId = user.Id },
        new Tag { Name = "Личное", OwnerId = user.Id },
        new Tag { Name = "Рабочее", OwnerId = user.Id }
    };

    context.Tags.AddRange(tags);
    context.SaveChanges();

    var tasks = new[]
    {
        new TodoTask
        {
            Title = "Купить продукты",
            Description = "Молоко, хлеб, яйца",
            Status = "New",
            Priority = "Medium",
            UserId = user.Id,
            ProjectId = projects[0].Id,
            Deadline = DateTime.UtcNow.AddDays(1)
        },
        new TodoTask
        {
            Title = "Сдать отчет",
            Description = "Подготовить квартальный отчет",
            Status = "InProgress",
            Priority = "High",
            UserId = user.Id,
            ProjectId = projects[1].Id,
            Deadline = DateTime.UtcNow.AddHours(5)
        },
        new TodoTask
        {
            Title = "Прочитать книгу",
            Description = "Глава 3",
            Status = "New",
            Priority = "Low",
            UserId = user.Id,
            ProjectId = projects[2].Id,
            Deadline = null
        },
        new TodoTask
        {
            Title = "Позвонить маме",
            Description = "",
            Status = "New",
            Priority = "Medium",
            UserId = user.Id,
            ProjectId = null,
            Deadline = DateTime.UtcNow.AddDays(2)
        }
    };

    context.Tasks.AddRange(tasks);
    context.SaveChanges();

    var taskTags = new[]
    {
        new TaskTag { TaskId = tasks[0].Id, TagId = tags[1].Id },
        new TaskTag { TaskId = tasks[0].Id, TagId = tags[3].Id },
        new TaskTag { TaskId = tasks[1].Id, TagId = tags[0].Id },
        new TaskTag { TaskId = tasks[1].Id, TagId = tags[1].Id },
        new TaskTag { TaskId = tasks[1].Id, TagId = tags[4].Id },
        new TaskTag { TaskId = tasks[2].Id, TagId = tags[2].Id },
        new TaskTag { TaskId = tasks[3].Id, TagId = tags[3].Id }
    };

    context.TaskTags.AddRange(taskTags);
    context.SaveChanges();
}