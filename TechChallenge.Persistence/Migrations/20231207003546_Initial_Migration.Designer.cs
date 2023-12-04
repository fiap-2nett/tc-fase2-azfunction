﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechChallenge.Persistence;

namespace TechChallenge.Persistence.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20231207003546_Initial_Migration")]
    partial class Initial_Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TechChallenge.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("TechChallenge.Domain.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("orderitems");
                });

            modelBuilder.Entity("TechChallenge.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("products");

                    b.HasData(
                        new
                        {
                            Id = 1000,
                            Name = "Camiseta Dragon’s Treasure – Black Edition",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1001,
                            Name = "Camiseta Angra – Cycles Of Pain",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1002,
                            Name = "Camiseta Raccoon City",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1003,
                            Name = "Camiseta Voyager Black Edition",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1004,
                            Name = "Camiseta Necronomicon Black Edition",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1005,
                            Name = "Camiseta Árvore de Gondor – Gold Edition",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1006,
                            Name = "Camiseta Lovecraft",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1007,
                            Name = "Camiseta Dark Side",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1008,
                            Name = "Camiseta de R’lyeh",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1009,
                            Name = "Camiseta Upside Down",
                            Price = 54.90m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 1010,
                            Name = "Camiseta Miskatonic University",
                            Price = 54.90m,
                            Quantity = 3
                        });
                });

            modelBuilder.Entity("TechChallenge.Domain.Entities.Order", b =>
                {
                    b.OwnsOne("TechChallenge.Domain.ValueObjects.Email", "CustomerEmail", b1 =>
                        {
                            b1.Property<int>("OrderId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("CustomerEmail")
                                .HasColumnType("nvarchar(256)")
                                .HasMaxLength(256);

                            b1.HasKey("OrderId");

                            b1.ToTable("orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });
                });

            modelBuilder.Entity("TechChallenge.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("TechChallenge.Domain.Entities.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TechChallenge.Domain.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
