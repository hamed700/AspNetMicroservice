﻿using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        MongoClient client = new
            (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        IMongoDatabase database = 
            client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

        CatalogInitial.SetDefaultValues(Products);
    }

    public IMongoCollection<Product> Products { get; }
}
