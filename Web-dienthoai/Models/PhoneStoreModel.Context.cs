﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_dienthoai.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QLDienThoaiEntities : DbContext
    {
        public QLDienThoaiEntities()
            : base("name=QLDienThoaiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<ChiTietDH> ChiTietDHs { get; set; }
        public virtual DbSet<ChiTietSP> ChiTietSPs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<DungLuong> DungLuongs { get; set; }
        public virtual DbSet<HinhSP> HinhSPs { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<LoaiKH> LoaiKHs { get; set; }
        public virtual DbSet<Mau> Maus { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<ThuongHieu> ThuongHieux { get; set; }
        public virtual DbSet<TSKTSP> TSKTSPs { get; set; }
    }
}