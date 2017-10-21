﻿// <auto-generated />
using ContestEACA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ContestEACA.Migrations
{
    [DbContext(typeof(ApplicationPostDbContext))]
    [Migration("20171021104642_PostAddLinkWork")]
    partial class PostAddLinkWork
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ContestEACA.Models.FileModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Path");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("ContestEACA.Models.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PostId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("ContestEACA.Models.Nomination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Nominations");
                });

            modelBuilder.Entity("ContestEACA.Models.Post", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("File");

                    b.Property<string>("LinkWork");

                    b.Property<int>("NominationId");

                    b.Property<int>("Rating");

                    b.Property<string>("TextWork");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("NominationId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ContestEACA.Models.Like", b =>
                {
                    b.HasOne("ContestEACA.Models.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContestEACA.Models.Post", b =>
                {
                    b.HasOne("ContestEACA.Models.Nomination", "Nomination")
                        .WithMany("Posts")
                        .HasForeignKey("NominationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
