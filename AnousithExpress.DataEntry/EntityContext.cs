using AnousithExpress.DataEntry.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AnousithExpress.DataEntry
{
    public class EntityContext : DbContext
    {
        public EntityContext() : base("name = Dbconnectionstring")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public DbSet<TbUser> tbUsers { get; set; }
        public DbSet<TbTime> tbTimes { get; set; }
        public DbSet<TbStatus> tbStatuses { get; set; }
        public DbSet<TbRoute> tbRoutes { get; set; }
        public DbSet<TbRole> tbRoles { get; set; }
        public DbSet<TbItemStatus> tbItemStatuses { get; set; }
        public DbSet<TbItems> TbItems { get; set; }
        public DbSet<TbItemSentAllocation> tbItemSentAllocations { get; set; }
        public DbSet<TbItemSentHistory> tbItemSentHistories { get; set; }
        public DbSet<TbItemsPickUpAllocation> tbItemPickUpAllocations { get; set; }
        public DbSet<TbItemsPickupHistory> tbItemsPickupHistories { get; set; }
        public DbSet<TbCustomer> tbCustomers { get; set; }
        public DbSet<TbConsolidateList> tbConsolidateLists { get; set; }
        public DbSet<TbConsolidatedItems> tbConsolidatedItems { get; set; }
        public DbSet<TbPrice> tbPrices { get; set; }
    }
}
