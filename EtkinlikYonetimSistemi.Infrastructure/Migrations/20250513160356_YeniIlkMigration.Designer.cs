﻿// <auto-generated />
using System;
using EtkinlikYonetimSistemi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EtkinlikYonetimSistemi.Infrastructure.Migrations
{
    [DbContext(typeof(EventDbContext))]
    [Migration("20250513160356_YeniIlkMigration")]
    partial class YeniIlkMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Bilet", b =>
                {
                    b.Property<int>("BiletId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BiletId"));

                    b.Property<int>("EtkinlikId")
                        .HasColumnType("int");

                    b.Property<decimal>("Fiyat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StokAdedi")
                        .HasColumnType("int");

                    b.HasKey("BiletId");

                    b.HasIndex("EtkinlikId");

                    b.ToTable("Biletler");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Etkinlik", b =>
                {
                    b.Property<int>("EtkinlikId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EtkinlikId"));

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("BiletFiyati")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("KalanBilet")
                        .HasColumnType("int");

                    b.Property<int>("Kapasite")
                        .HasColumnType("int");

                    b.Property<int>("KategoriId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Tarih")
                        .HasColumnType("datetime2");

                    b.HasKey("EtkinlikId");

                    b.HasIndex("KategoriId");

                    b.ToTable("Etkinlikler");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.IlgiAlani", b =>
                {
                    b.Property<int>("IlgiAlaniId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IlgiAlaniId"));

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("AnaKategoriMi")
                        .HasColumnType("bit");

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UstKategoriId")
                        .HasColumnType("int");

                    b.HasKey("IlgiAlaniId");

                    b.HasIndex("UstKategoriId");

                    b.ToTable("IlgiAlanlari");

                    b.HasData(
                        new
                        {
                            IlgiAlaniId = 1,
                            Aciklama = "Müzik etkinlikleri",
                            Ad = "Müzik",
                            AnaKategoriMi = true,
                            BasePrice = 200.00m,
                            UstKategoriId = 1
                        },
                        new
                        {
                            IlgiAlaniId = 2,
                            Aciklama = "Spor etkinlikleri",
                            Ad = "Spor",
                            AnaKategoriMi = true,
                            BasePrice = 180.00m,
                            UstKategoriId = 2
                        },
                        new
                        {
                            IlgiAlaniId = 3,
                            Aciklama = "Teknoloji etkinlikleri",
                            Ad = "Teknoloji",
                            AnaKategoriMi = true,
                            BasePrice = 150.00m,
                            UstKategoriId = 3
                        },
                        new
                        {
                            IlgiAlaniId = 5,
                            Aciklama = "Tiyatro etkinlikleri",
                            Ad = "Tiyatro",
                            AnaKategoriMi = true,
                            BasePrice = 300.00m,
                            UstKategoriId = 5
                        },
                        new
                        {
                            IlgiAlaniId = 4,
                            Aciklama = "Gezi etkinlikleri",
                            Ad = "Gezi",
                            AnaKategoriMi = true,
                            BasePrice = 250.00m,
                            UstKategoriId = 4
                        },
                        new
                        {
                            IlgiAlaniId = 11,
                            Aciklama = "Rock mÃ¼zik etkinlikleri",
                            Ad = "Rock",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 1
                        },
                        new
                        {
                            IlgiAlaniId = 12,
                            Aciklama = "Pop mÃ¼zik etkinlikleri",
                            Ad = "Pop",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 1
                        },
                        new
                        {
                            IlgiAlaniId = 13,
                            Aciklama = "Klasik mÃ¼zik etkinlikleri",
                            Ad = "Klasik",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 1
                        },
                        new
                        {
                            IlgiAlaniId = 14,
                            Aciklama = "Halk mÃ¼ziÄŸi etkinlikleri",
                            Ad = "Halk MÃ¼zigi",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 1
                        },
                        new
                        {
                            IlgiAlaniId = 15,
                            Aciklama = "Metal mÃ¼zik etkinlikleri",
                            Ad = "Metal",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 1
                        },
                        new
                        {
                            IlgiAlaniId = 16,
                            Aciklama = "Rap mÃ¼zik etkinlikleri",
                            Ad = "Rap",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 1
                        },
                        new
                        {
                            IlgiAlaniId = 17,
                            Aciklama = "DJ etkinlikleri",
                            Ad = "DJ",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 1
                        },
                        new
                        {
                            IlgiAlaniId = 21,
                            Aciklama = "Futbol maÃ§larÄ±  ",
                            Ad = "Futbol",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 2
                        },
                        new
                        {
                            IlgiAlaniId = 22,
                            Aciklama = "Basketbol maÃ§larÄ±",
                            Ad = "Basketbol",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 2
                        },
                        new
                        {
                            IlgiAlaniId = 23,
                            Aciklama = "Voleybol maÃ§larÄ±",
                            Ad = "Voleybol",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 2
                        },
                        new
                        {
                            IlgiAlaniId = 24,
                            Aciklama = "Formula 1 yarÄ±ÅŸlarÄ±",
                            Ad = "Formula 1",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 2
                        },
                        new
                        {
                            IlgiAlaniId = 25,
                            Aciklama = "Doğa yürüyüşleri",
                            Ad = "Doga Yuruyusu",
                            AnaKategoriMi = false,
                            BasePrice = 0m,
                            UstKategoriId = 2
                        });
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Kullanici", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LoginSayisi")
                        .HasColumnType("int");

                    b.Property<bool>("OnayliMi")
                        .HasColumnType("bit");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SifreHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Kullanicilar");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.SatinAlim", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int?>("KullaniciId")
                        .HasColumnType("int");

                    b.Property<int>("SatinAlimId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SatinAlimTarihi")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("KullaniciId");

                    b.ToTable("SatinAlimlar");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.SatinAlimBileti", b =>
                {
                    b.Property<int>("SatinAlimId")
                        .HasColumnType("int");

                    b.Property<int>("BiletId")
                        .HasColumnType("int");

                    b.HasKey("SatinAlimId", "BiletId");

                    b.HasIndex("BiletId");

                    b.ToTable("SatinAlimBiletleri");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Sepet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("KullaniciId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OlusturmaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int>("SepetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KullaniciId");

                    b.ToTable("Sepetler");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.SepetBileti", b =>
                {
                    b.Property<int>("SepetId")
                        .HasColumnType("int");

                    b.Property<int>("BiletId")
                        .HasColumnType("int");

                    b.Property<int>("Adet")
                        .HasColumnType("int");

                    b.Property<int?>("BiletId1")
                        .HasColumnType("int");

                    b.HasKey("SepetId", "BiletId");

                    b.HasIndex("BiletId");

                    b.HasIndex("BiletId1");

                    b.ToTable("SepetBiletleri");
                });

            modelBuilder.Entity("IlgiAlaniKullanici", b =>
                {
                    b.Property<int>("IlgiAlanlariIlgiAlaniId")
                        .HasColumnType("int");

                    b.Property<int>("KullanicilarId")
                        .HasColumnType("int");

                    b.HasKey("IlgiAlanlariIlgiAlaniId", "KullanicilarId");

                    b.HasIndex("KullanicilarId");

                    b.ToTable("IlgiAlaniKullanici", (string)null);
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Bilet", b =>
                {
                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Etkinlik", "Etkinlik")
                        .WithMany("Biletler")
                        .HasForeignKey("EtkinlikId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etkinlik");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Etkinlik", b =>
                {
                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.IlgiAlani", "IlgiAlani")
                        .WithMany("Etkinlikler")
                        .HasForeignKey("KategoriId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IlgiAlani");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.IlgiAlani", b =>
                {
                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.IlgiAlani", "UstKategori")
                        .WithMany("AltKategoriler")
                        .HasForeignKey("UstKategoriId");

                    b.Navigation("UstKategori");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.SatinAlim", b =>
                {
                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Kullanici", "Kullanici")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Kullanici", null)
                        .WithMany("SatinAlimlar")
                        .HasForeignKey("KullaniciId");

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.SatinAlimBileti", b =>
                {
                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Bilet", "Bilet")
                        .WithMany()
                        .HasForeignKey("BiletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.SatinAlim", "SatinAlim")
                        .WithMany("SatinAlimBiletleri")
                        .HasForeignKey("SatinAlimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bilet");

                    b.Navigation("SatinAlim");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Sepet", b =>
                {
                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Kullanici", "Kullanici")
                        .WithMany("Sepetler")
                        .HasForeignKey("KullaniciId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.SepetBileti", b =>
                {
                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Bilet", "Bilet")
                        .WithMany()
                        .HasForeignKey("BiletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Bilet", null)
                        .WithMany("SepetBiletleri")
                        .HasForeignKey("BiletId1");

                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Sepet", "Sepet")
                        .WithMany("SepetBiletleri")
                        .HasForeignKey("SepetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bilet");

                    b.Navigation("Sepet");
                });

            modelBuilder.Entity("IlgiAlaniKullanici", b =>
                {
                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.IlgiAlani", null)
                        .WithMany()
                        .HasForeignKey("IlgiAlanlariIlgiAlaniId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EtkinlikYonetimSistemi.Domain.Entities.Kullanici", null)
                        .WithMany()
                        .HasForeignKey("KullanicilarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Bilet", b =>
                {
                    b.Navigation("SepetBiletleri");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Etkinlik", b =>
                {
                    b.Navigation("Biletler");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.IlgiAlani", b =>
                {
                    b.Navigation("AltKategoriler");

                    b.Navigation("Etkinlikler");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Kullanici", b =>
                {
                    b.Navigation("SatinAlimlar");

                    b.Navigation("Sepetler");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.SatinAlim", b =>
                {
                    b.Navigation("SatinAlimBiletleri");
                });

            modelBuilder.Entity("EtkinlikYonetimSistemi.Domain.Entities.Sepet", b =>
                {
                    b.Navigation("SepetBiletleri");
                });
#pragma warning restore 612, 618
        }
    }
}
