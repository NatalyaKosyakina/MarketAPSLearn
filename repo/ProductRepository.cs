using AutoMapper;
using Market.Models;
using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace Market.repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public ProductRepository(IMapper mapper, IMemoryCache cache)
        {
            _mapper = mapper;
            _cache = cache;
        }
        public int AddGroup(CategoryModel group)
        {
            using (var context = new MarketContext())
            {
                var entityGroup = context.Categories.FirstOrDefault(x =>
                x.Name.ToLower().Equals(group.Name.ToLower()));
                if (entityGroup == null)
                {
                    entityGroup = _mapper.Map<Category>(group);
                    context.Categories.Add(entityGroup);
                    context.SaveChanges();
                    _cache.Remove("categories");
                }
                return entityGroup.Id;
            }
        }

        public int AddProduct(ProductModel product)
        {
            using (var context = new MarketContext())
            {
                var entityProduct = context.Products.FirstOrDefault(x =>
                x.Name.ToLower().Equals(product.Name.ToLower()));
                if (entityProduct == null)
                {
                    entityProduct = _mapper.Map<Product>(product);
                    context.Products.Add(entityProduct);
                    context.SaveChanges();
                    _cache.Remove("products");
                }
                return entityProduct.Id;
            }
        }

        public IEnumerable<CategoryModel> GetGroups()
        {
            if (_cache.TryGetValue("categories", out List<CategoryModel> groups))
            {
                return groups;
            }
            using (var context = new MarketContext())
            {
                var list = context.Categories.Select(x => _mapper.Map<CategoryModel>(x)).ToList();
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
            using (var context = new MarketContext())
            {
                var list = context.Products.Select(x => _mapper.Map<ProductModel>(x)).ToList();
                _cache.Set("products", list, TimeSpan.FromMinutes(30));
                return list;
            }
        }
    }
}
