﻿

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class QPCHARTEntities : DbContext
{
    public QPCHARTEntities()
        : base("name=QPCHARTEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<Menu> Menu { get; set; }

    public DbSet<SysUser> SysUser { get; set; }

    public DbSet<华青路> 华青路 { get; set; }

    public DbSet<通波塘> 通波塘 { get; set; }

    public DbSet<新区路> 新区路 { get; set; }

    public DbSet<新业路> 新业路 { get; set; }

    public DbSet<赵巷A> 赵巷A { get; set; }

    public DbSet<赵巷B> 赵巷B { get; set; }

    public DbSet<赵巷C> 赵巷C { get; set; }

    public DbSet<赵重路> 赵重路 { get; set; }

}

