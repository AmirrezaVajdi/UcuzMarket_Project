using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Infrastructure.EFCore;
using BlogManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EfCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;

namespace ServiceHost
{
    public class DataBaseCreator
    {
        public static void Migrate(IServiceProvider provider)
        {
            var accountContext = provider.GetRequiredService<AccountContext>();
            var blogContext = provider.GetRequiredService<BlogContext>();
            var commentContext = provider.GetRequiredService<CommentContext>();
            var discountContext = provider.GetRequiredService<DiscountContext>();
            var inventoryContext = provider.GetRequiredService<InventoryContext>();
            var shopContext = provider.GetRequiredService<ShopContext>();

            var isDbCreated = accountContext.Database.EnsureCreated();

            if (isDbCreated)
            {
                blogContext.Database.Migrate();
                commentContext.Database.Migrate();
                discountContext.Database.Migrate();
                inventoryContext.Database.Migrate();
                shopContext.Database.Migrate();

                var account = new Account("کاربر ادمین پیش فرض", "admin", "10000.zmlstgGPOBLwzIQhde+BpQ==.WuOA2mKKSjZbDXpF23/y29s1c1EBPaNMk2iFeYttUhE=", "09999999999", 1, "");

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

                accountContext.Roles.AddRange(roles);
                accountContext.SaveChanges();
                accountContext.Accounts.Add(account);
                accountContext.SaveChanges();
            }
        }
    }
}
