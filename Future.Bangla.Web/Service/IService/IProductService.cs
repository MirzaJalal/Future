﻿using Future.Bangla.Web.Models;

namespace Future.Bangla.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductAsync(string couponCode);
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductsAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
