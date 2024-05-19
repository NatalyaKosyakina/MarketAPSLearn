using AutoMapper;
using Market.Models;
using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Protocol;

namespace Market.repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private MarketContext _marketContext;
        public ProductRepository(IMapper mapper, IMemoryCache cache, MarketContext marketContext)
        {
            _mapper = mapper;
            _cache = cache;
            _marketContext = marketContext;
        }
        public int AddGroup(CategoryModel group)
        {
            using (_marketContext)
            {
                var entityGroup = _marketContext.Categories.FirstOrDefault(x =>
                x.Name.ToLower().Equals(group.Name.ToLower()));
                if (entityGroup == null)
                {
                    entityGroup = _mapper.Map<Category>(group);
                    _marketContext.Categories.Add(entityGroup);
                    _marketContext.SaveChanges();
                    _cache.Remove("categories");
                }
                return entityGroup.Id;
            }
        }

        public int AddProduct(ProductModel product)
        {
            using (_marketContext)
            {
                var entityProduct = _marketContext.Products.FirstOrDefault(x =>
                x.Name.ToLower().Equals(product.Name.ToLower()));
                if (entityProduct == null)
                {
                    entityProduct = _mapper.Map<Product>(product);
                    _marketContext.Products.Add(entityProduct);
                    _marketContext.SaveChanges();
                    _cache.Remove("products");
                }
                return entityProduct.Id;
            }
        }

        public bool DeleteProduct(int id)
        {
            using (_marketContext)
            {
                var product = _marketContext.Products.FirstOrDefault(x => x.Id == id);
                if (product == null)
                {
                    return false;
                }
                _marketContext.Products.Remove(product);
                _marketContext.SaveChanges();
                _cache.Remove("products");
                return true;
            }
        }

        public IEnumerable<CategoryModel> GetGroups()
        {
            if (_cache.TryGetValue("categories", out List<CategoryModel> groups))
            {
                return groups;
            }
            using (_marketContext)
            {
                var list = _marketContext.Categories.Select(x => _mapper.Map<CategoryModel>(x)).ToList();
                _cache.Set("categories", list, TimeSpan.FromMinutes(30));
                return list;
            }
        }

        public IEnumerable<ProductModel> GetProducts()
        {
            if (_cache.TryGetValue("products", out List<ProductModel> models))
            {
                return models;
            }
            using (_marketContext)
            {
                var list = _marketContext.Products.Select(x => _mapper.Map<ProductModel>(x)).ToList();
                _cache.Set("products", list, TimeSpan.FromMinutes(30));
                return list;
            }
        }

        public bool UpdatePrice(int id, int price)
        {
            using (_marketContext)
            {
                var product = _marketContext.Products.FirstOrDefault(x => x.Id == id);
                if (product == null)
                {
                    return false;
                }
                product.Price = price;
                _marketContext.SaveChanges();
                _cache.Remove("products");
                return true;
            }
        }

        public string GetCacheStatistic()
        {
            var memory = _cache.GetCurrentStatistics();
            return memory.ToJson();
        }
    }
}
