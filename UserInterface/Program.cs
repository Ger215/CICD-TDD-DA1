using DataAccess.Context;
using DataAccess.Repository;
using Logic;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserInterface.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDBLocalConnection"))
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationWindowsDBConnection"))
);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSingleton<UserSession>();

builder.Services.AddScoped<IUserAuthentication,ApplicationController>();
builder.Services.AddScoped<IUserModification,ApplicationController>();
builder.Services.AddScoped<ICategoryController,ApplicationController>();
builder.Services.AddScoped<IAccountController,ApplicationController>();
builder.Services.AddScoped<IGeneralAccountController,ApplicationController>();
builder.Services.AddScoped<ICreditCardAccountController,ApplicationController>();
builder.Services.AddScoped<ITransactionController,ApplicationController>();
builder.Services.AddScoped<IExchangeRateController,ApplicationController>();
builder.Services.AddScoped<ISpendingGoalController,ApplicationController>();
builder.Services.AddScoped<IReportByCategory,ReportController>();
builder.Services.AddScoped<IReportGoalByMonth,ReportController>();
builder.Services.AddScoped<IReportExpenses,ReportController>();
builder.Services.AddScoped<IReportIncomeOutcome,ReportController>();
builder.Services.AddScoped<IReportCreditCardExpenses,ReportController>();
builder.Services.AddScoped<IReportGeneralAccountBalance,ReportController>();


builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<AccountsRepository>();
builder.Services.AddScoped<TransactionsRepository>();
builder.Services.AddScoped<ExchangeRatesRepository>();
builder.Services.AddScoped<SpendingGoalsRepository>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
