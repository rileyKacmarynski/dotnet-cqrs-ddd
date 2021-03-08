﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SampleStore.Domain.Customers;
using SampleStore.Domain.Customers.Orders;
using SampleStore.Domain.SharedKernel.ValueObjects;
using SampleStore.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Domain.Customers
{
    internal sealed class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        internal const string OrdersList = "_orders";
        internal const string OrderProducts = "_orderProducts";

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", SchemaNames.Orders);

            builder.HasKey(b => b.Id);

            builder.Property("_welcomeEmailWasSent").HasColumnName("WelcomeEmailWasSent");
            builder.Property("_email").HasColumnName("Email");
            builder.Property("_name").HasColumnName("Name");

            builder.OwnsMany<Order>(OrdersList, x =>
            {
                x.WithOwner().HasForeignKey("CustomerId");

                x.ToTable("Orders", SchemaNames.Orders);

                x.Property<bool>("_isRemoved").HasColumnName("IsRemoved");
                x.Property<DateTime>("_orderDate").HasColumnName("OrderDate");
                x.Property<DateTime?>("_orderChangeDate").HasColumnName("OrderChangeDate");
                x.HasKey("Id");

                x.Property("_status").HasColumnName("StatusId")
                .HasConversion(new EnumToNumberConverter<OrderStatus, byte>());

                x.OwnsMany<OrderProduct>(OrderProducts, y =>
                {
                    y.WithOwner().HasForeignKey("OrderId");

                    y.ToTable("OrderProducts", SchemaNames.Orders);
                    y.Property<Guid>("OrderId");
                    y.Property<Guid>("ProductId");

                    y.HasKey("OrderId", "ProductId");

                    y.OwnsOne<MoneyValue>("Price", mv =>
                    {
                        mv.Property(p => p.Currency).HasColumnName("Currency");
                        mv.Property(p => p.Value).HasColumnName("Value");
                    });

                    //y.OwnsOne<MoneyValue>("ValueInEUR", mv =>
                    //{
                    //    mv.Property(p => p.Currency).HasColumnName("CurrencyEUR");
                    //    mv.Property(p => p.Value).HasColumnName("ValueInEUR");
                    //});
                });

                //x.OwnsOne<MoneyValue>("_value", y =>
                //{
                //    y.Property(p => p.Currency).HasColumnName("Currency");
                //    y.Property(p => p.Value).HasColumnName("Value");
                //});

                //x.OwnsOne<MoneyValue>("_valueInEUR", y =>
                //{
                //    y.Property(p => p.Currency).HasColumnName("CurrencyEUR");
                //    y.Property(p => p.Value).HasColumnName("ValueInEUR");
                //});
            });
        }
    }
}
