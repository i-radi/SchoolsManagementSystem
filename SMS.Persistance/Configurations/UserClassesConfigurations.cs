using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SMS.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.Models.Entities;

namespace SMS.Persistance.Configurations;


public class UserClassesConfigurations : IEntityTypeConfiguration<UserClass>
{
    public void Configure(EntityTypeBuilder<UserClass> builder)
    {
        builder
           .HasKey(x => x.Id);

        builder.HasOne(u => u.Season)
                 .WithMany(d => d.UserClasses)
                 .HasForeignKey(k => k.SeasonId)
                 .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.Classes)
         .WithMany(d => d.UserClasses)
         .HasForeignKey(k => k.ClassId)
         .OnDelete(DeleteBehavior.NoAction);
    }
}

