﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResourceManager.Infrastructure.Persistence;

#nullable disable

namespace ResourceManager.Infrastructure.Migrations
{
    [DbContext(typeof(ResourceManagerDbContext))]
    partial class ResourceManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ResourceManager.Domain.Resources.Resource", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsWithdrawn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Bit")
                        .HasDefaultValue(false)
                        .HasColumnName("Withdrawn");

                    b.HasKey("Id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("ResourceManager.Domain.Resources.ResourceLock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ResourceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Until")
                        .HasColumnType("datetime2")
                        .HasColumnName("ActiveUntil");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ResourceId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("ResourceLocks");
                });

            modelBuilder.Entity("ResourceManager.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                            Email = "admin@rm.io",
                            Role = 1
                        },
                        new
                        {
                            Id = new Guid("fc851687-8e22-4fc4-a0be-8431a6e9d8ea"),
                            Email = "slim.john@gmail.com",
                            Role = 0
                        });
                });

            modelBuilder.Entity("ResourceManager.Domain.Resources.ResourceLock", b =>
                {
                    b.HasOne("ResourceManager.Domain.Resources.Resource", null)
                        .WithOne("ResourceLock")
                        .HasForeignKey("ResourceManager.Domain.Resources.ResourceLock", "ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ResourceManager.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ResourceManager.Domain.Resources.Resource", b =>
                {
                    b.Navigation("ResourceLock");
                });
#pragma warning restore 612, 618
        }
    }
}
