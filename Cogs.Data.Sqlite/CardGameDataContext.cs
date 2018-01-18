using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cogs.Data.Sqlite.Entities;
using SQLite.CodeFirst;

namespace Cogs.Data.Sqlite
{
    public class CardGameDataContext : DbContext
    {
        public CardGameDataContext(string connectionString) : base(connectionString)
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<CardGameDataContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<CardAttribute> CardAttributes { get; set; }
        public DbSet<CardLayout> CardLayouts { get; set; }
        public DbSet<CardAttributeValue> CardAttributeValues { get; set; }
    }
}
