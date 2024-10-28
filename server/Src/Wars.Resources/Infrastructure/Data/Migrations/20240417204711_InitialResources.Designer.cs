﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wars.Resources.Infrastructure.Data;

#nullable disable

namespace Wars.Resources.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ResourcesContext))]
    [Migration("20240417204711_InitialResources")]
    partial class InitialResources
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Resources")
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Wars.Resources.Domain.Village", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.ComplexProperty<Dictionary<string, object>>("ResourceBuilding", "Wars.Resources.Domain.Village.ResourceBuilding#ResourceBuilding", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("ClayPit")
                                .HasColumnType("integer");

                            b1.Property<int>("IronMine")
                                .HasColumnType("integer");

                            b1.Property<int>("LumberCamp")
                                .HasColumnType("integer");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("ResourceInventory", "Wars.Resources.Domain.Village.ResourceInventory#ResourceInventory", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<float>("Clay")
                                .HasPrecision(18, 6)
                                .HasColumnType("real");

                            b1.Property<float>("Iron")
                                .HasPrecision(18, 6)
                                .HasColumnType("real");

                            b1.Property<DateTimeOffset>("UpdatedAt")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<int>("WarehouseCapacity")
                                .HasColumnType("integer");

                            b1.Property<float>("Wood")
                                .HasPrecision(18, 6)
                                .HasColumnType("real");
                        });

                    b.HasKey("Id");

                    b.ToTable("Villages", "Resources");
                });
#pragma warning restore 612, 618
        }
    }
}