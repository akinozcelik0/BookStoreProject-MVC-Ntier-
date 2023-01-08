﻿using Microsoft.EntityFrameworkCore;
using RaidBookStore.Models;

namespace RaidBookStore.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<CoverType> CoverTypes { get; set; }

}
