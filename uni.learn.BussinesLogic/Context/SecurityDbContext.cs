using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using uni.learn.core.Entity;

namespace uni.learn.BussinesLogic.Context;

public class SecurityDbContext : IdentityDbContext<Usuario>
{

    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

}
