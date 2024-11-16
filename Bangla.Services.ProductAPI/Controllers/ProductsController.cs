using AutoMapper;
using Bangla.Services.ProductAPI.Data;
using Bangla.Services.ProductAPI.Models;
using Bangla.Services.ProductAPI.Models.Dto;
using Bangla.Services.ProductAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bangla.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IStorageService _blobStorageService;
        private ProductResponseDto _response;
        private IMapper _mapper;
        public ProductsController(ApplicationDbContext db, IStorageService blobStorageService, IMapper mapper)
        {
            _db = db;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _response = new ProductResponseDto();
        }

        [HttpGet]
        public ProductResponseDto Get()
        {
            try
            {
                IEnumerable<Product> products = _db.Products.ToList();

                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ProductResponseDto Get(int id)
        {
            try
            {
                Product product = _db.Products.First(c => c.ProductId == id);

                _response.Result = _mapper.Map<Product>(product); ;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ProductResponseDto> Post([FromForm] ProductDto Product)
        {
            try
            {
                string imageUrlFromBlob = null;

                if (Product.ImageFile is not null)
                {
                    imageUrlFromBlob = await _blobStorageService.UploadFileAsync(Product.ImageFile);
                }

                var ProductObj = _mapper.Map<Product>(Product);
                ProductObj.ImageUrl = imageUrlFromBlob;

                await _db.Products.AddAsync(ProductObj);
                await _db.SaveChangesAsync();

                var responseDto = _mapper.Map<ProductDto>(ProductObj);
                _response.Result = responseDto;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ProductResponseDto Update([FromForm] ProductDto Product)
        {
            try
            {
                string imageUrlFromBlob = null;

                if (Product.ImageFile is not null)
                {
                    imageUrlFromBlob =  _blobStorageService.UploadFileAsync(Product.ImageFile).GetAwaiter().GetResult();
                }

                var ProductObj = _mapper.Map<Product>(Product);
                ProductObj.ImageUrl = imageUrlFromBlob;

                _db.Products.Update(ProductObj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDto>(ProductObj); ;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ProductResponseDto Delete(int id)
        {
            try
            {
                var Product = _db.Products.FirstOrDefault(c => c.ProductId == id);
                _db.Products.Remove(Product);
                _db.SaveChanges();

                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = ex.Message;
            }

            return _response;
        }
    }


}
