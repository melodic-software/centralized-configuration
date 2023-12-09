﻿// <auto-generated />
using System;
using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Configuration.EntityFramework.SqlServer.Migrations
{
    [DbContext(typeof(ConfigurationContext))]
    [Migration("20230929232857_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ApplicationEntity", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ApplicationId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationId"));

                    b.Property<string>("AbbreviatedName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApplicationId");

                    b.ToTable("Application", (string)null);

                    b.HasData(
                        new
                        {
                            ApplicationId = 1,
                            AbbreviatedName = "Demo App",
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Description = "This is a demo application.",
                            DomainId = new Guid("500f86a2-65f7-4fc2-836a-2b14f8686209"),
                            IsActive = true,
                            IsDeleted = false,
                            Name = "Demo Application",
                            UniqueName = "Demo Application-500f86a2"
                        },
                        new
                        {
                            ApplicationId = 2,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            DomainId = new Guid("a49262fd-9ab9-452e-92b9-bfb742c94bd0"),
                            IsActive = true,
                            IsDeleted = false,
                            Name = "Demo API",
                            UniqueName = "Demo API-a49262fd"
                        },
                        new
                        {
                            ApplicationId = 3,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            DomainId = new Guid("dd65fe33-97d0-4632-b7f4-677ffd2fdf14"),
                            IsActive = false,
                            IsDeleted = false,
                            Name = "Demo WinForm Application",
                            UniqueName = "Demo WinForm Application-dd65fe33"
                        });
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationDataTypeEntity", b =>
                {
                    b.Property<int>("ConfigurationValueDataTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ConfigurationValueDataTypeId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConfigurationValueDataTypeId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConfigurationValueDataTypeId");

                    b.ToTable("ConfigurationDataType", (string)null);

                    b.HasData(
                        new
                        {
                            ConfigurationValueDataTypeId = 1,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "string"
                        },
                        new
                        {
                            ConfigurationValueDataTypeId = 2,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "boolean"
                        });
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationEntryEntity", b =>
                {
                    b.Property<int>("ConfigurationEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ConfigurationEntryId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConfigurationEntryId"));

                    b.Property<int>("ConfigurationEntryTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LabelId")
                        .HasColumnType("int");

                    b.HasKey("ConfigurationEntryId");

                    b.HasIndex("ConfigurationEntryTypeId");

                    b.HasIndex("LabelId");

                    b.ToTable("ConfigurationEntry", (string)null);
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationEntryTypeEntity", b =>
                {
                    b.Property<int>("ConfigurationEntryTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ConfigurationEntryTypeId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConfigurationEntryTypeId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConfigurationEntryTypeId");

                    b.ToTable("ConfigurationEntryType", (string)null);

                    b.HasData(
                        new
                        {
                            ConfigurationEntryTypeId = 1,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "application setting"
                        },
                        new
                        {
                            ConfigurationEntryTypeId = 2,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "connection string"
                        },
                        new
                        {
                            ConfigurationEntryTypeId = 3,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "feature toggle"
                        });
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationValueEntity", b =>
                {
                    b.Property<int>("ConfigurationValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ConfigurationValueId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConfigurationValueId"));

                    b.Property<int?>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<int>("ConfigurationEntryId")
                        .HasColumnType("int");

                    b.Property<int?>("ConfigurationValueDataTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EnvironmentId")
                        .HasColumnType("int");

                    b.Property<int?>("LocalContextId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConfigurationValueId");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("ConfigurationEntryId");

                    b.HasIndex("ConfigurationValueDataTypeId");

                    b.HasIndex("EnvironmentId");

                    b.HasIndex("LocalContextId");

                    b.ToTable("ConfigurationValue", (string)null);
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.EnvironmentEntity", b =>
                {
                    b.Property<int>("EnvironmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EnvironmentId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnvironmentId"));

                    b.Property<string>("AbbreviatedDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EnvironmentId");

                    b.ToTable("Environment", (string)null);

                    b.HasData(
                        new
                        {
                            EnvironmentId = 1,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Local",
                            DomainId = new Guid("fc52da1b-439a-4ffa-90f9-a9f7f585deb9"),
                            IsActive = true,
                            UniqueName = "Local-fc52da1b"
                        },
                        new
                        {
                            EnvironmentId = 2,
                            AbbreviatedDisplayName = "Dev",
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Development",
                            DomainId = new Guid("69c89b3b-654c-4461-baad-bd4f115d8a11"),
                            IsActive = true,
                            UniqueName = "Development-69c89b3b"
                        },
                        new
                        {
                            EnvironmentId = 3,
                            AbbreviatedDisplayName = "Test",
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Testing",
                            DomainId = new Guid("629e10af-8c7e-4b76-9e01-985e28a6a08c"),
                            IsActive = true,
                            UniqueName = "Testing-629e10af"
                        },
                        new
                        {
                            EnvironmentId = 4,
                            AbbreviatedDisplayName = "Stage",
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Staging",
                            DomainId = new Guid("105c1e10-8701-43ca-a457-cd01f14591e5"),
                            IsActive = true,
                            UniqueName = "Staging-105c1e10"
                        },
                        new
                        {
                            EnvironmentId = 5,
                            AbbreviatedDisplayName = "Prod",
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Production",
                            DomainId = new Guid("bf420f08-a3ec-4bf9-bb87-90dc04c4b6b9"),
                            IsActive = true,
                            UniqueName = "Production-bf420f08"
                        },
                        new
                        {
                            EnvironmentId = 6,
                            AbbreviatedDisplayName = "Train",
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Training",
                            DomainId = new Guid("f2000413-1926-44f4-ba3f-19ca8b2da955"),
                            IsActive = false,
                            UniqueName = "Training-f2000413"
                        });
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.LabelEntity", b =>
                {
                    b.Property<int>("LabelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LabelId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LabelId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LabelId");

                    b.ToTable("Label", (string)null);
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.LocalContextEntity", b =>
                {
                    b.Property<int>("LocalContextId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LocalContextId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocalContextId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocalContextId");

                    b.ToTable("LocalContext", (string)null);

                    b.HasData(
                        new
                        {
                            LocalContextId = 1,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Identifier = "kyle.sexton@wachter.com"
                        });
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationEntryEntity", b =>
                {
                    b.HasOne("Configuration.EntityFramework.Entities.ConfigurationEntryTypeEntity", "ConfigurationEntryType")
                        .WithMany("ConfigurationEntries")
                        .HasForeignKey("ConfigurationEntryTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Configuration.EntityFramework.Entities.LabelEntity", "Label")
                        .WithMany("ConfigurationEntries")
                        .HasForeignKey("LabelId");

                    b.Navigation("ConfigurationEntryType");

                    b.Navigation("Label");
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationValueEntity", b =>
                {
                    b.HasOne("Configuration.EntityFramework.Entities.ApplicationEntity", "Application")
                        .WithMany("ConfigurationValues")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("Configuration.EntityFramework.Entities.ConfigurationEntryEntity", "ConfigurationEntry")
                        .WithMany("Values")
                        .HasForeignKey("ConfigurationEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Configuration.EntityFramework.Entities.ConfigurationDataTypeEntity", "DataType")
                        .WithMany("ConfigurationValues")
                        .HasForeignKey("ConfigurationValueDataTypeId");

                    b.HasOne("Configuration.EntityFramework.Entities.EnvironmentEntity", "Environment")
                        .WithMany("ConfigurationValues")
                        .HasForeignKey("EnvironmentId");

                    b.HasOne("Configuration.EntityFramework.Entities.LocalContextEntity", "LocalContext")
                        .WithMany("ConfigurationValues")
                        .HasForeignKey("LocalContextId");

                    b.Navigation("Application");

                    b.Navigation("ConfigurationEntry");

                    b.Navigation("DataType");

                    b.Navigation("Environment");

                    b.Navigation("LocalContext");
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ApplicationEntity", b =>
                {
                    b.Navigation("ConfigurationValues");
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationDataTypeEntity", b =>
                {
                    b.Navigation("ConfigurationValues");
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationEntryEntity", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationEntryTypeEntity", b =>
                {
                    b.Navigation("ConfigurationEntries");
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.EnvironmentEntity", b =>
                {
                    b.Navigation("ConfigurationValues");
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.LabelEntity", b =>
                {
                    b.Navigation("ConfigurationEntries");
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.LocalContextEntity", b =>
                {
                    b.Navigation("ConfigurationValues");
                });
#pragma warning restore 612, 618
        }
    }
}
