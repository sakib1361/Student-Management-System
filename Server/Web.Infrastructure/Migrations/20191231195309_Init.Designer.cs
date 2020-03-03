﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Student.Infrastructure.DBModel;

namespace Student.Infrasructure.Migrations
{
    [DbContext(typeof(StudentDBContext))]
    [Migration("20191231195309_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("CoreEngine.Model.DBModel.Batch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndsOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfSemester")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SemesterDuration")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartsOn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("CourseCredit")
                        .HasColumnType("TEXT");

                    b.Property<string>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CourseName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("SemesterId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SemesterId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.DBFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NoticeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("NoticeId");

                    b.ToTable("DBFiles");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.DBUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<int?>("BatchId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BloodGroup")
                        .HasColumnType("TEXT");

                    b.Property<bool>("ClassRepresentative")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EnrolledIn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Occupation")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RequirePasswordChange")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<int>("Roll")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkHistory")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.ToTable("DBUsers");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.DBUserTodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DBUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ToDoItemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DBUserId");

                    b.HasIndex("ToDoItemId");

                    b.ToTable("DBUserTodoItem");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomNo")
                        .HasColumnType("TEXT");

                    b.Property<int>("TeacherId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TeacherName")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("TimeOfDay")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.Notice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BatchId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("FutureNotification")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PostType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.HasIndex("CourseId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Notices");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.Semester", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BatchId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndsOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartsOn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.ToTable("Semesters");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.StudentCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Grade")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("GradePoint")
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentCourses");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.ToDoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ToDoItems");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Student.Infrasructure.DBModel.IdentityDBUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("DBUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("DBUserId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.Course", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.Semester", "Semester")
                        .WithMany("Courses")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.DBFile", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.Course", null)
                        .WithMany("CourseMaterials")
                        .HasForeignKey("CourseId");

                    b.HasOne("CoreEngine.Model.DBModel.Notice", null)
                        .WithMany("DBFiles")
                        .HasForeignKey("NoticeId");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.DBUser", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.Batch", "Batch")
                        .WithMany("Students")
                        .HasForeignKey("BatchId");
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.DBUserTodoItem", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.DBUser", "DBUser")
                        .WithMany()
                        .HasForeignKey("DBUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreEngine.Model.DBModel.ToDoItem", "ToDoItem")
                        .WithMany("Participents")
                        .HasForeignKey("ToDoItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.Lesson", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.Course", "Course")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.Notice", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.Batch", "Batch")
                        .WithMany()
                        .HasForeignKey("BatchId");

                    b.HasOne("CoreEngine.Model.DBModel.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId");

                    b.HasOne("CoreEngine.Model.DBModel.DBUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.Semester", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.Batch", "Batch")
                        .WithMany("Semesters")
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreEngine.Model.DBModel.StudentCourse", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.Course", "Course")
                        .WithMany("StudentCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreEngine.Model.DBModel.DBUser", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Student.Infrasructure.DBModel.IdentityDBUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Student.Infrasructure.DBModel.IdentityDBUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Student.Infrasructure.DBModel.IdentityDBUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Student.Infrasructure.DBModel.IdentityDBUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Student.Infrasructure.DBModel.IdentityDBUser", b =>
                {
                    b.HasOne("CoreEngine.Model.DBModel.DBUser", "DBUser")
                        .WithMany()
                        .HasForeignKey("DBUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
