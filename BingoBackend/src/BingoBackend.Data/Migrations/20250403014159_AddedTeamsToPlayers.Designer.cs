﻿// <auto-generated />
using BingoBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BingoBackend.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250403014159_AddedTeamsToPlayers")]
    partial class AddedTeamsToPlayers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("BingoBackend.Data.Entities.PlayerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("BingoBackend.Data.Entities.TeamEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("PlayerEntityTeamEntity", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlayerId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("PlayerEntityTeamEntity");
                });

            modelBuilder.Entity("PlayerEntityTeamEntity", b =>
                {
                    b.HasOne("BingoBackend.Data.Entities.PlayerEntity", null)
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BingoBackend.Data.Entities.TeamEntity", null)
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
