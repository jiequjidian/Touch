﻿//------------------------------------------------------------------------------
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
    public DbSet<好氧1_NH3N> 好氧1_NH3N { get; set; }
    public DbSet<好氧2_NH3N> 好氧2_NH3N { get; set; }
    public DbSet<好氧3_DO> 好氧3_DO { get; set; }
    public DbSet<好氧4_DO> 好氧4_DO { get; set; }
    public DbSet<后缺氧1_MLSS> 后缺氧1_MLSS { get; set; }
    public DbSet<后缺氧2_MLSS> 后缺氧2_MLSS { get; set; }
    public DbSet<前缺氧1_DO> 前缺氧1_DO { get; set; }
    public DbSet<前缺氧2_DO> 前缺氧2_DO { get; set; }
}
