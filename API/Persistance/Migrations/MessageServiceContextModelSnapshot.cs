﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistance;

namespace mySimpleMessageService.Persistance.Migrations
{
    [DbContext(typeof(MessageServiceContext))]
    partial class MessageServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Persistance.Entities.ContactEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Persistance.Entities.MessageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContactReceivedId")
                        .HasColumnType("int");

                    b.Property<int?>("ContactSentId")
                        .HasColumnType("int");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContactReceivedId");

                    b.HasIndex("ContactSentId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Persistance.Entities.MessageEntity", b =>
                {
                    b.HasOne("Persistance.Entities.ContactEntity", "ContactReceived")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("ContactReceivedId");

                    b.HasOne("Persistance.Entities.ContactEntity", "ContactSent")
                        .WithMany("MessagesSent")
                        .HasForeignKey("ContactSentId");

                    b.Navigation("ContactReceived");

                    b.Navigation("ContactSent");
                });

            modelBuilder.Entity("Persistance.Entities.ContactEntity", b =>
                {
                    b.Navigation("MessagesReceived");

                    b.Navigation("MessagesSent");
                });
#pragma warning restore 612, 618
        }
    }
}
