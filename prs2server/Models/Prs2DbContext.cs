using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace prs2server.Models {
    
    public class Prs2DbContext : DbContext {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Requestline> Requestlines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>(e => {
                e.HasIndex("Username").IsUnique();
            });
            builder.Entity<Vendor>(e => {
                e.HasIndex("Code").IsUnique();
            });
            builder.Entity<Product>(e => {
                e.HasIndex("PartNbr").IsUnique();
            });
        }

        public Prs2DbContext(DbContextOptions<Prs2DbContext> options) : base(options) { }
    }
}
