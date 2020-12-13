﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NKYS.Models;

namespace NKYS.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20201213231024_externalIdIsString")]
    partial class externalIdIsString
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("NKYS.Models.Cycle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastCalculedSalaryTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<decimal?>("StandardWorkingHours")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ValidationTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Validity")
                        .HasColumnType("bit");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cycle");
                });

            modelBuilder.Entity("NKYS.Models.Department", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("NKYS.Models.Employe", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("DeductionPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("DepartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EntreEntrepriseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExternalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("FixSalary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("GroupsId")
                        .HasColumnType("bigint");

                    b.Property<bool>("HasDorm")
                        .HasColumnType("bit");

                    b.Property<bool>("IsChefOfGroup")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTemporaryEmploye")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("PositionPay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("SelfPayHousingReserves")
                        .HasColumnType("bit");

                    b.Property<bool>("SelfPaySocialSercurity")
                        .HasColumnType("bit");

                    b.Property<decimal?>("SeniorityPay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TechnicalLevel")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TransportFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GroupsId");

                    b.ToTable("Employe");
                });

            modelBuilder.Entity("NKYS.Models.EmployeDeductionConfiguration", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("EmployeId")
                        .HasColumnType("bigint");

                    b.Property<int?>("LinkedProductionValueTypeId")
                        .HasColumnType("int");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeId");

                    b.ToTable("EmployeDeductionConfiguration");
                });

            modelBuilder.Entity("NKYS.Models.Groups", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("DepartmentId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsFixSalary")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductionValueTypeId")
                        .HasColumnType("int");

                    b.Property<decimal?>("SharePropotion")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("NKYS.Models.ProductionValue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("CycleId")
                        .HasColumnType("bigint");

                    b.Property<int>("ProductionValueTypeId")
                        .HasColumnType("int");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Validity")
                        .HasColumnType("bit");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CycleId");

                    b.ToTable("ProductionValue");
                });

            modelBuilder.Entity("NKYS.Models.Salary", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<decimal?>("AbsentDeduct")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("AbsentHours")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("CycleId")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("DeferredHolidayHours")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("DormFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("DormOtherFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("EmployeId")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("FullPresencePay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("HousingReserves")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("OtherFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("OvertimePay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SelfPaySocialSercurityFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SeniorityPay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SocialSercurityFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TransportFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("WorkingHours")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("WorkingScore")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CycleId");

                    b.HasIndex("EmployeId");

                    b.ToTable("Salary");
                });

            modelBuilder.Entity("NKYS.Models.Employe", b =>
                {
                    b.HasOne("NKYS.Models.Groups", "Groups")
                        .WithMany("Employes")
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Groups");
                });

            modelBuilder.Entity("NKYS.Models.EmployeDeductionConfiguration", b =>
                {
                    b.HasOne("NKYS.Models.Employe", "Employe")
                        .WithMany("EmployeDeductionConfiguration")
                        .HasForeignKey("EmployeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employe");
                });

            modelBuilder.Entity("NKYS.Models.Groups", b =>
                {
                    b.HasOne("NKYS.Models.Department", "Department")
                        .WithMany("Groups")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("NKYS.Models.ProductionValue", b =>
                {
                    b.HasOne("NKYS.Models.Cycle", "Cycle")
                        .WithMany()
                        .HasForeignKey("CycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cycle");
                });

            modelBuilder.Entity("NKYS.Models.Salary", b =>
                {
                    b.HasOne("NKYS.Models.Cycle", "Cycle")
                        .WithMany()
                        .HasForeignKey("CycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NKYS.Models.Employe", "Employe")
                        .WithMany()
                        .HasForeignKey("EmployeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cycle");

                    b.Navigation("Employe");
                });

            modelBuilder.Entity("NKYS.Models.Department", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("NKYS.Models.Employe", b =>
                {
                    b.Navigation("EmployeDeductionConfiguration");
                });

            modelBuilder.Entity("NKYS.Models.Groups", b =>
                {
                    b.Navigation("Employes");
                });
#pragma warning restore 612, 618
        }
    }
}