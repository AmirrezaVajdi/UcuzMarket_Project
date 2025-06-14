﻿using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagement.Infrastructure.EFCore.Mapping
{
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.OwnsMany(x => x.Permissions, builder =>
            {
                builder.HasKey(x => x.Id);
                builder.ToTable("RolePermissions");
                builder.Property(x => x.Name).HasColumnName("PermissionName");
                builder.Ignore(x => x.Name);
                builder.WithOwner(x => x.Role);
            });
        }
    }
}
