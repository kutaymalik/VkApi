using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vk.Base.Model;

namespace Vk.Data.Domain;

[Table("Address", Schema = "dbo")]
public class Address : BaseModel
{
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string County { get; set; }
    public string PostalCode { get; set; }


    public virtual Customer Customer { get; set; }
}


public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(x => x.InsertUserId).IsRequired();
        builder.Property(x => x.UpdateUserId).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.InsertDate).IsRequired();
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(x => x.CustomerId).IsRequired(true);
        builder.Property(x => x.AddressLine1).IsRequired().HasMaxLength(50);
        builder.Property(x => x.AddressLine2).IsRequired().HasMaxLength(50);
        builder.Property(x => x.City).IsRequired().HasMaxLength(50);
        builder.Property(x => x.County).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PostalCode).IsRequired().HasMaxLength(10);


        builder.HasIndex(x => x.CustomerId);
    }
}