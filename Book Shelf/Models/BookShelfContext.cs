using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Book_Shelf.Models;

public partial class BookShelfContext : DbContext
{
    public BookShelfContext()
    {
    }

    public BookShelfContext(DbContextOptions<BookShelfContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-303LNE9\\MSSQLSERVER01;Database=BookShelf;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
}
