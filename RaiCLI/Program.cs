﻿using RaiCLI.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RaiCLI.DbContextEF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//Dbcontext
builder.Services.AddDbContext<aspnetWebApplicationIdentityContext>(options =>
            options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

//starting class
builder.Services.AddScoped<ISelector,Selector>();



var app = builder.Build();

app.RunAsync();



var _selector=app.Services.GetRequiredService<ISelector>();

IRaiCLI? CommandClass =  _selector.GetClass(args);
if (CommandClass != null)
    CommandClass.Invoke(args);
else
    Console.WriteLine("Command not found");