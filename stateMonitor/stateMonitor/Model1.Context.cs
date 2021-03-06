﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace stateMonitor
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class internetEntities1 : DbContext
    {
        public internetEntities1()
            : base("name=internetEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<irkki> irkki { get; set; }
        public virtual DbSet<lampotilaTaulu> lampotilaTaulu { get; set; }
        public virtual DbSet<irkki_old> irkki_old { get; set; }
        public virtual DbSet<memos> memos { get; set; }
    
        [DbFunction("internetEntities1", "F_ROWS")]
        public virtual IQueryable<F_ROWS_Result> F_ROWS(Nullable<System.DateTime> start, Nullable<System.DateTime> end, string channel)
        {
            var startParameter = start.HasValue ?
                new ObjectParameter("start", start) :
                new ObjectParameter("start", typeof(System.DateTime));
    
            var endParameter = end.HasValue ?
                new ObjectParameter("end", end) :
                new ObjectParameter("end", typeof(System.DateTime));
    
            var channelParameter = channel != null ?
                new ObjectParameter("channel", channel) :
                new ObjectParameter("channel", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<F_ROWS_Result>("[internetEntities1].[F_ROWS](@start, @end, @channel)", startParameter, endParameter, channelParameter);
        }
    }
}
