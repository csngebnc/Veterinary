using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;

namespace Veterinary.Dal.EntityConfigurations
{
    class VeterinaryUserEntityConfiguration : IEntityTypeConfiguration<VeterinaryUser>
    {
        public void Configure(EntityTypeBuilder<VeterinaryUser> builder)
        {
            builder.Property(x => x.PhotoUrl)
                .HasDefaultValue("https://veterinary.blob.core.windows.net/defaults/placeholder.jpg");

            builder.Property(x => x.PhoneNumber)
                .HasDefaultValue("");


        }
    }
}
