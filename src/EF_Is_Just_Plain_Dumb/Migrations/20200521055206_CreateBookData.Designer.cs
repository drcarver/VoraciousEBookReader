﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleEPUBReader.Database;

namespace EF_Is_Just_Plain_Dumb.Migrations
{
    [DbContext(typeof(BooksContext))]
    [Migration("20200521055206_CreateBooks")]
    partial class CreateBooks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("SimpleEpubReader.Database.Books", b =>
                {
                    b.Property<string>("BookId")
                        .HasColumnType("TEXT");

                    b.Property<string>("BookSeries")
                        .HasColumnType("TEXT");

                    b.Property<string>("BookSource")
                        .HasColumnType("TEXT");

                    b.Property<int>("BookType")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DenormDownloadDate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DenormPrimaryAuthor")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DownloadDataId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Imprint")
                        .HasColumnType("TEXT");

                    b.Property<string>("Issued")
                        .HasColumnType("TEXT");

                    b.Property<string>("LCC")
                        .HasColumnType("TEXT");

                    b.Property<string>("LCCN")
                        .HasColumnType("TEXT");

                    b.Property<string>("LCSH")
                        .HasColumnType("TEXT");

                    b.Property<string>("Language")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NavigationDataId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NotesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PGEditionInfo")
                        .HasColumnType("TEXT");

                    b.Property<string>("PGNotes")
                        .HasColumnType("TEXT");

                    b.Property<string>("PGProducedBy")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ReviewId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("TitleAlternative")
                        .HasColumnType("TEXT");

                    b.HasKey("BookId");

                    b.HasIndex("DownloadDataId");

                    b.HasIndex("NavigationDataId");

                    b.HasIndex("NotesId");

                    b.HasIndex("ReviewId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.BookNavigationData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BookId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrSpot")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrStatus")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("FirstNavigationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("MostRecentNavigationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("NCatalogViews")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NReading")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NSpecificSelection")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NSwipeLeft")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NSwipeRight")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("TimeMarkedDone")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BookNavigationData");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.BookNotes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BookId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BookNotes");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.DownloadData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BookId")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrFileStatus")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DownloadDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DownloadData");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.FilenameAndFormatData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BooksBookId")
                        .HasColumnType("TEXT");

                    b.Property<string>("BookId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Extent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileType")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("MimeType")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BooksBookId");

                    b.ToTable("FilenameAndFormatData");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Aliases")
                        .HasColumnType("TEXT");

                    b.Property<int>("BirthDate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BooksBookId")
                        .HasColumnType("TEXT");

                    b.Property<int>("DeathDate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("PersonType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Webpage")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BooksBookId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.UserNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BackgroundColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("BookId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("BookNotesId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ForegroundColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("Icon")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("MostRecentModificationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("SelectedText")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BookNotesId");

                    b.ToTable("UserNote");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.UserReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BookId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("MostRecentModificationDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("NStars")
                        .HasColumnType("REAL");

                    b.Property<string>("Review")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserReview");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.Books", b =>
                {
                    b.HasOne("SimpleEpubReader.Database.DownloadData", "DownloadData")
                        .WithMany()
                        .HasForeignKey("DownloadDataId");

                    b.HasOne("SimpleEpubReader.Database.BookNavigationData", "NavigationData")
                        .WithMany()
                        .HasForeignKey("NavigationDataId");

                    b.HasOne("SimpleEpubReader.Database.BookNotes", "Notes")
                        .WithMany()
                        .HasForeignKey("NotesId");

                    b.HasOne("SimpleEpubReader.Database.UserReview", "Review")
                        .WithMany()
                        .HasForeignKey("ReviewId");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.FilenameAndFormatData", b =>
                {
                    b.HasOne("SimpleEpubReader.Database.Books", null)
                        .WithMany("Files")
                        .HasForeignKey("BooksBookId");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.Person", b =>
                {
                    b.HasOne("SimpleEpubReader.Database.Books", null)
                        .WithMany("People")
                        .HasForeignKey("BooksBookId");
                });

            modelBuilder.Entity("SimpleEpubReader.Database.UserNote", b =>
                {
                    b.HasOne("SimpleEpubReader.Database.BookNotes", null)
                        .WithMany("Notes")
                        .HasForeignKey("BookNotesId");
                });
#pragma warning restore 612, 618
        }
    }
}