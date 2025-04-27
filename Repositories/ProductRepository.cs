using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.Data;
using VNFarm.DTOs.Filters;
using VNFarm.Entities;
using VNFarm.Interfaces.Repositories;

namespace VNFarm.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly DbSet<Review> _reviewsSet;
        
        public ProductRepository(VNFarmContext context) : base(context)
        {
            _reviewsSet = context.Set<Review>();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted && p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByStoreAsync(int storeId)
        {
            return await _dbSet
                .Where(p => p.StoreId == storeId && !p.IsDeleted && p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync(int page, int count)
        {
            return await _dbSet
                .Where(p => !p.IsDeleted && p.IsActive)
                .OrderByDescending(p => p.SoldQuantity)
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync();
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _dbSet.FindAsync(productId);
            if (product == null)
                return;

            product.StockQuantity = quantity;
            product.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _reviewsSet
                .Where(r => r.ProductId == productId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
        
        public async Task<Review?> AddReviewAsync(Review review)
        {
            // Thêm đánh giá mới
            review.CreatedAt = DateTime.Now;
            await _reviewsSet.AddAsync(review);
            
            // Cập nhật điểm đánh giá trung bình cho sản phẩm
            Product? product = await _dbSet.FindAsync(review.ProductId);
            if (product != null)
            {
                var reviews = await _reviewsSet
                    .Where(r => r.ProductId == review.ProductId && !r.IsDeleted)
                    .ToListAsync();
                
                // Tính điểm đánh giá trung bình
                double averageRating = reviews.Count > 0 
                    ? reviews.Average(r => r.Rating) 
                    : review.Rating;
                
                product.AverageRating = (product.AverageRating * product.ReviewCount + review.Rating) / (product.ReviewCount + 1);
                product.ReviewCount += 1;
                product.UpdatedAt = DateTime.Now;
            }
            
            await _context.SaveChangesAsync();
            return review;
        }
    }
} 