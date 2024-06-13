using AccountManagement.Domain.RoleAgg;
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

                var adminRolePermissions = new List<Permission>()
                {
                    new(60),
                    new(61),
                    new(62),
                    new(63),
                    new(64),
                    new(65),
                    new(66),
                    new(67),
                    new(68),
                    new(69),
                    new(70),
                    new(71),
                    new(72),
                    new(73),
                    new(74),
                    new(75),
                    new(76),
                    new(77),
                    new(78),
                    new(79),
                    new(80),
                    new(81),
                    new(82),
                    new(83),
                    new(84),
                    new(85),
                    new(86),
                    new(87),
                    new(88),
                    new(89),
                    new(90),
                    new(91),
                    new(92),
                    new(93),
                    new(94),
                    new(95),
                    new(96),
                    new(97),
                    new(98),
                    new(99),
                    new(100),
                    new(101),
                    new(102),
                    new(103),
                    new(104),
                    new(105),
                    new(106),
                    new(107),
                    new(108),
                    new(109),
                    new(110),
                    new(111),
                    new(112),
                    new(113),
                    new(114),
                    new(115),
                    new(116),
                    new(117),
                    new(118),
                };

                Role[] roles =
                {
                    new Role("مدیر سیستم" , adminRolePermissions) ,
                    new Role("کاربر سیستم" , null) ,
                    new Role("محتوا گذار" , null) ,
                    new Role("کاربر همکار" , null) ,

                };

                builder.HasData(roles);
            });
        }
    }
}
