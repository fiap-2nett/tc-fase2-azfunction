using System.Collections.Generic;
using TechChallenge.Domain.Entities;
using TechChallenge.Persistence.Core.Primitives;

namespace TechChallenge.Persistence.Seeds
{
    internal sealed class ProductSeed : EntitySeedConfiguration<Product>
    {
        public override IEnumerable<object> Seed()
        {
            yield return new { Id = 1000, Name = "Camiseta Dragon’s Treasure – Black Edition",  Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1001, Name = "Camiseta Angra – Cycles Of Pain",             Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1002, Name = "Camiseta Raccoon City",                       Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1003, Name = "Camiseta Voyager Black Edition",              Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1004, Name = "Camiseta Necronomicon Black Edition",         Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1005, Name = "Camiseta Árvore de Gondor – Gold Edition",    Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1006, Name = "Camiseta Lovecraft",                          Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1007, Name = "Camiseta Dark Side",                          Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1008, Name = "Camiseta de R’lyeh",                          Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1009, Name = "Camiseta Upside Down",                        Price = 54.90M, Quantity = 3 };
            yield return new { Id = 1010, Name = "Camiseta Miskatonic University",              Price = 54.90M, Quantity = 3 };
        }
    }
}
