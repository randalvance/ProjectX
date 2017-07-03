using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ProjectX.Data;
using ProjectX.Models;

namespace ProjectX.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170702134701_SchemaUpdates")]
    partial class SchemaUpdates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProjectX.Models.BankAccount", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName")
                        .IsRequired();

                    b.Property<string>("AccountNumber")
                        .IsRequired();

                    b.Property<decimal>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0m);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<byte[]>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("ID");

                    b.HasAlternateKey("AccountName");


                    b.HasAlternateKey("AccountNumber");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("ProjectX.Models.Transaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<decimal>("Balance");

                    b.Property<int>("BankAccountID");

                    b.Property<string>("Description");

                    b.Property<DateTime>("TransactionDate");

                    b.Property<int>("TransactionType");

                    b.HasKey("ID");

                    b.HasIndex("BankAccountID");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("ProjectX.Models.Transaction", b =>
                {
                    b.HasOne("ProjectX.Models.BankAccount", "BankAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("BankAccountID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
