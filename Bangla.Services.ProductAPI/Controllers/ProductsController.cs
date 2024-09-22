using AutoMapper;
using Bangla.Services.ProductAPI.Data;
using Bangla.Services.ProductAPI.Models;
using Bangla.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bangla.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ProductResponseDto _response;
        private IMapper _mapper;
        public ProductsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
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
        public ProductResponseDto Post([FromBody] ProductDto Product)
        {
            try
            {
                var ProductObj = _mapper.Map<Product>(Product);
                _db.Products.Add(ProductObj);
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

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ProductResponseDto Update([FromBody] ProductDto Product)
        {
            try
            {
                var ProductObj = _mapper.Map<Product>(Product);
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
