﻿// <auto-generated />
using System;
using Globamantics.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Globamantics.Infrastructure.Migrations
{
    [DbContext(typeof(GlobomanticsDbContext))]
    [Migration("20240318042601_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.7");

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BugId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BugId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.ToDo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ParentId");

                    b.ToTable("ToDo");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ToDo");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.ToDoTask", b =>
                {
                    b.HasBaseType("Globamantics.Infrastructure.Data.Models.ToDo");

                    b.Property<DateTimeOffset>("DueDate")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("ToDoTask");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.Bug", b =>
                {
                    b.HasBaseType("Globamantics.Infrastructure.Data.Models.ToDoTask");

                    b.Property<int>("AffectedUsers")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AffectedVersion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AssignedToId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Severity")
                        .HasColumnType("INTEGER");

                    b.HasIndex("AssignedToId");

                    b.ToTable("ToDo", t =>
                        {
                            t.Property("AssignedToId")
                                .HasColumnName("Bug_AssignedToId");

                            t.Property("Description")
                                .HasColumnName("Bug_Description");
                        });

                    b.HasDiscriminator().HasValue("Bug");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.Feature", b =>
                {
                    b.HasBaseType("Globamantics.Infrastructure.Data.Models.ToDoTask");

                    b.Property<Guid>("AssignedToId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Component")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.HasIndex("AssignedToId");

                    b.HasDiscriminator().HasValue("Feature");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.Image", b =>
                {
                    b.HasOne("Globamantics.Infrastructure.Data.Models.Bug", null)
                        .WithMany("Images")
                        .HasForeignKey("BugId");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.ToDo", b =>
                {
                    b.HasOne("Globamantics.Infrastructure.Data.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Globamantics.Infrastructure.Data.Models.ToDo", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("CreatedBy");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.Bug", b =>
                {
                    b.HasOne("Globamantics.Infrastructure.Data.Models.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedTo");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.Feature", b =>
                {
                    b.HasOne("Globamantics.Infrastructure.Data.Models.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedTo");
                });

            modelBuilder.Entity("Globamantics.Infrastructure.Data.Models.Bug", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}