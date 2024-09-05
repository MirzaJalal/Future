using AutoMapper;
using Bangla.Services.CouponAPI.Data;
using Bangla.Services.CouponAPI.Models;
using Bangla.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        private ResponseDto _response;
        public CouponsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> coupons = _db.Coupons.ToList();
                
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
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
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.CouponId == id);

                _response.Result = _mapper.Map<CouponDto>(coupon); ;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result= ex.Message;
            }

            return _response;
        }
    }


}
