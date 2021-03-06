﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ShareCar.Db;
using ShareCar.Db.Entities;
using System;

namespace ShareCar.Db.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Number");

                    b.Property<string>("Street");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.DriverNote", b =>
                {
                    b.Property<int>("DriverNoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RideId");

                    b.Property<string>("Text");

                    b.HasKey("DriverNoteId");

                    b.HasIndex("RideId");

                    b.ToTable("DriverNotes");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.DriverSeenNote", b =>
                {
                    b.Property<int>("DriverSeenNoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DriverNoteId");

                    b.Property<int>("RideRequestId");

                    b.Property<bool>("Seen")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.HasKey("DriverSeenNoteId");

                    b.HasIndex("DriverNoteId");

                    b.HasIndex("RideRequestId");

                    b.ToTable("DriverSeenNotes");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.Passenger", b =>
                {
                    b.Property<int>("PassengerId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Completed");

                    b.Property<string>("Email");

                    b.Property<bool>("PassengerResponded");

                    b.Property<int>("RideId");

                    b.HasKey("PassengerId");

                    b.HasIndex("Email");

                    b.HasIndex("RideId");

                    b.ToTable("Passengers");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.Ride", b =>
                {
                    b.Property<int>("RideId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DriverEmail");

                    b.Property<int>("NumberOfSeats")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(4);

                    b.Property<DateTime>("RideDateTime");

                    b.Property<int>("RouteId");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.HasKey("RideId");

                    b.HasIndex("RouteId");

                    b.ToTable("Rides");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.RideRequest", b =>
                {
                    b.Property<int>("RideRequestId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressId");

                    b.Property<string>("DriverEmail");

                    b.Property<string>("PassengerEmail");

                    b.Property<int>("RideId");

                    b.Property<bool>("SeenByDriver");

                    b.Property<bool>("SeenByPassenger");

                    b.Property<int>("Status");

                    b.HasKey("RideRequestId");

                    b.HasIndex("AddressId");

                    b.HasIndex("DriverEmail");

                    b.HasIndex("PassengerEmail");

                    b.HasIndex("RideId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.RideRequestNote", b =>
                {
                    b.Property<int>("RideRequestNoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RideRequestId");

                    b.Property<bool>("Seen")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Text");

                    b.HasKey("RideRequestNoteId");

                    b.HasIndex("RideRequestId");

                    b.ToTable("RideRequestNotes");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FromId");

                    b.Property<string>("Geometry")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<int>("ToId");

                    b.HasKey("RouteId");

                    b.HasIndex("FromId");

                    b.HasIndex("ToId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.UnauthorizedUser", b =>
                {
                    b.Property<int>("UnauthorizedUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<int>("VerificationCode");

                    b.HasKey("UnauthorizedUserId");

                    b.HasIndex("Email");

                    b.ToTable("UnauthorizedUsers");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("CarColor");

                    b.Property<string>("CarModel");

                    b.Property<string>("CognizantEmail");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FacebookEmail");

                    b.Property<long?>("FacebookId");

                    b.Property<bool>("FacebookVerified");

                    b.Property<string>("FirstName");

                    b.Property<string>("GoogleEmail");

                    b.Property<long?>("GoogleId");

                    b.Property<bool>("GoogleVerified");

                    b.Property<int?>("HomeAddressId");

                    b.Property<string>("LastName");

                    b.Property<string>("LicensePlate");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<int>("NumberOfSeats");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("Phone");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PictureUrl");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("HomeAddressId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ShareCar.Db.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareCar.Db.Entities.DriverNote", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.Ride", "Ride")
                        .WithMany()
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareCar.Db.Entities.DriverSeenNote", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.DriverNote", "Note")
                        .WithMany()
                        .HasForeignKey("DriverNoteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ShareCar.Db.Entities.RideRequest", "Request")
                        .WithMany()
                        .HasForeignKey("RideRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareCar.Db.Entities.Passenger", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("Email");

                    b.HasOne("ShareCar.Db.Entities.Ride", "Ride")
                        .WithMany("Passengers")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareCar.Db.Entities.Ride", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.Route", "Route")
                        .WithMany("Rides")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareCar.Db.Entities.RideRequest", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.Address", "RequestAddress")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ShareCar.Db.Entities.User", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverEmail");

                    b.HasOne("ShareCar.Db.Entities.User", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerEmail");

                    b.HasOne("ShareCar.Db.Entities.Ride", "RequestedRide")
                        .WithMany("Requests")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareCar.Db.Entities.RideRequestNote", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.RideRequest", "RideRequest")
                        .WithMany()
                        .HasForeignKey("RideRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareCar.Db.Entities.Route", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.Address", "FromAddress")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ShareCar.Db.Entities.Address", "ToAddress")
                        .WithMany()
                        .HasForeignKey("ToId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareCar.Db.Entities.UnauthorizedUser", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("Email");
                });

            modelBuilder.Entity("ShareCar.Db.Entities.User", b =>
                {
                    b.HasOne("ShareCar.Db.Entities.Address", "HomeAddress")
                        .WithMany()
                        .HasForeignKey("HomeAddressId");
                });
#pragma warning restore 612, 618
        }
    }
}
