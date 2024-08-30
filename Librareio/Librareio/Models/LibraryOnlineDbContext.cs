using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Librareio.Models;

public partial class LibraryOnlineDbContext : DbContext
{
    public LibraryOnlineDbContext()
    {
    }

    public LibraryOnlineDbContext(DbContextOptions<LibraryOnlineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-FHLVNBL;Initial Catalog=LibraryOnlineDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PK__BOOKS__18CD33CA86D94289");

            entity.ToTable("BOOKS");

            entity.Property(e => e.SubmissionId).HasColumnName("SUBMISSION_ID");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AUTHOR");
            entity.Property(e => e.BookName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BOOK_NAME");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.DisplayImage).HasColumnName("DISPLAY_IMAGE");
            entity.Property(e => e.PdfFile).HasColumnName("PDF_FILE");
            entity.Property(e => e.SubmissionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("SUBMISSION_DATE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
