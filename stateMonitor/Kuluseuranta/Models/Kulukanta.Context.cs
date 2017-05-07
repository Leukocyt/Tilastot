﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kuluseuranta.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class kulutEntities : DbContext
    {
        public kulutEntities()
            : base("name=kulutEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<kulut> kulut { get; set; }
        public virtual DbSet<kulutyypit> kulutyypit { get; set; }
        public virtual DbSet<paikat> paikat { get; set; }
    
        [DbFunction("kulutEntities", "F_Menot_Per_Viikko")]
        public virtual IQueryable<F_Menot_Per_Viikko_Result> F_Menot_Per_Viikko(Nullable<System.DateTime> start, Nullable<System.DateTime> end, string typeList, string placeList, Nullable<int> groupType, Nullable<int> resMax)
        {
            var startParameter = start.HasValue ?
                new ObjectParameter("start", start) :
                new ObjectParameter("start", typeof(System.DateTime));
    
            var endParameter = end.HasValue ?
                new ObjectParameter("end", end) :
                new ObjectParameter("end", typeof(System.DateTime));
    
            var typeListParameter = typeList != null ?
                new ObjectParameter("typeList", typeList) :
                new ObjectParameter("typeList", typeof(string));
    
            var placeListParameter = placeList != null ?
                new ObjectParameter("placeList", placeList) :
                new ObjectParameter("placeList", typeof(string));
    
            var groupTypeParameter = groupType.HasValue ?
                new ObjectParameter("groupType", groupType) :
                new ObjectParameter("groupType", typeof(int));
    
            var resMaxParameter = resMax.HasValue ?
                new ObjectParameter("resMax", resMax) :
                new ObjectParameter("resMax", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<F_Menot_Per_Viikko_Result>("[kulutEntities].[F_Menot_Per_Viikko](@start, @end, @typeList, @placeList, @groupType, @resMax)", startParameter, endParameter, typeListParameter, placeListParameter, groupTypeParameter, resMaxParameter);
        }
    
        public virtual ObjectResult<S_Menot_Per_Viikko_Result> S_Menot_Per_Viikko(Nullable<System.DateTime> start, Nullable<System.DateTime> end, string typeList, string placeList, Nullable<int> groupType, Nullable<int> resMax)
        {
            var startParameter = start.HasValue ?
                new ObjectParameter("start", start) :
                new ObjectParameter("start", typeof(System.DateTime));
    
            var endParameter = end.HasValue ?
                new ObjectParameter("end", end) :
                new ObjectParameter("end", typeof(System.DateTime));
    
            var typeListParameter = typeList != null ?
                new ObjectParameter("typeList", typeList) :
                new ObjectParameter("typeList", typeof(string));
    
            var placeListParameter = placeList != null ?
                new ObjectParameter("placeList", placeList) :
                new ObjectParameter("placeList", typeof(string));
    
            var groupTypeParameter = groupType.HasValue ?
                new ObjectParameter("groupType", groupType) :
                new ObjectParameter("groupType", typeof(int));
    
            var resMaxParameter = resMax.HasValue ?
                new ObjectParameter("resMax", resMax) :
                new ObjectParameter("resMax", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<S_Menot_Per_Viikko_Result>("S_Menot_Per_Viikko", startParameter, endParameter, typeListParameter, placeListParameter, groupTypeParameter, resMaxParameter);
        }
    }
}
