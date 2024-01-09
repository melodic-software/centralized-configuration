﻿// <auto-generated />
using System;
using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Configuration.EntityFramework.Sqlite.Migrations
{
    [DbContext(typeof(ConfigurationContext))]
    [Migration("20240107183212_AddOutboxMessage")]
    partial class AddOutboxMessage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ApplicationEntity", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ApplicationId");

                    b.Property<string>("AbbreviatedName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("INTEGER");

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
                        .HasColumnType("INTEGER")
                        .HasColumnName("ConfigurationValueDataTypeId");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ConfigurationValueDataTypeId");

                    b.ToTable("ConfigurationDataType", (string)null);

                    b.HasData(
                        new
                        {
                            ConfigurationValueDataTypeId = 1,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "String"
                        },
                        new
                        {
                            ConfigurationValueDataTypeId = 2,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Boolean"
                        });
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationEntryEntity", b =>
                {
                    b.Property<int>("ConfigurationEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ConfigurationEntryId");

                    b.Property<int>("ConfigurationEntryTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("LabelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ConfigurationEntryId");

                    b.HasIndex("ConfigurationEntryTypeId");

                    b.HasIndex("LabelId");

                    b.ToTable("ConfigurationEntry", (string)null);
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationEntryTypeEntity", b =>
                {
                    b.Property<int>("ConfigurationEntryTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ConfigurationEntryTypeId");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ConfigurationEntryTypeId");

                    b.ToTable("ConfigurationEntryType", (string)null);

                    b.HasData(
                        new
                        {
                            ConfigurationEntryTypeId = 1,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "ApplicationSetting"
                        },
                        new
                        {
                            ConfigurationEntryTypeId = 2,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "ConnectionString"
                        },
                        new
                        {
                            ConfigurationEntryTypeId = 3,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Name = "FeatureToggle"
                        });
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.ConfigurationValueEntity", b =>
                {
                    b.Property<int>("ConfigurationValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ConfigurationValueId");

                    b.Property<int?>("ApplicationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConfigurationEntryId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ConfigurationValueDataTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EnvironmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LocalContextId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

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
                        .HasColumnType("INTEGER")
                        .HasColumnName("EnvironmentId");

                    b.Property<string>("AbbreviatedDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasColumnType("TEXT");

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
                        .HasColumnType("INTEGER")
                        .HasColumnName("LabelId");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LabelId");

                    b.ToTable("Label", (string)null);
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.LocalContextEntity", b =>
                {
                    b.Property<int>("LocalContextId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("LocalContextId");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LocalContextId");

                    b.ToTable("LocalContext", (string)null);

                    b.HasData(
                        new
                        {
                            LocalContextId = 1,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Identifier = "default"
                        },
                        new
                        {
                            LocalContextId = 2,
                            DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc),
                            Identifier = "kyle.sexton@wachter.com"
                        });
                });

            modelBuilder.Entity("Configuration.EntityFramework.Entities.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<DateTime?>("DateTimeOccurred")
                        .HasColumnType("TEXT");

                    b.Property<string>("Error")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessage", (string)null);
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