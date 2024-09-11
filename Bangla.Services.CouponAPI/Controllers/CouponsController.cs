using AutoMapper;
using Bangla.Services.CouponAPI.Data;
using Bangla.Services.CouponAPI.Models;
using Bangla.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Bangla.Services.CouponAPI.Controllers
{
    [Route("api/coupons")]
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

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string couponCode)
        {
            try
            {
                Coupon coupon = _db.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == couponCode.ToLower());
                
                if(coupon is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "not a valid code";
                }
                _response.Result = _mapper.Map<CouponDto>(coupon); ;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = ex.Message;
            }

            return _response;
        }


        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto coupon)
        {
            try
            {
                var couponObj = _mapper.Map<Coupon>(coupon);
                _db.Coupons.Add(couponObj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDto>(couponObj); ;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        public ResponseDto Update([FromBody] CouponDto coupon)
        {
            try
            {
                var couponObj = _mapper.Map<Coupon>(coupon);
                _db.Coupons.Update(couponObj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDto>(couponObj); ;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        public ResponseDto Delete(int id)
        {
            try
            {
                var coupon = _db.Coupons.FirstOrDefault(c => c.CouponId == id);
                _db.Coupons.Remove(coupon);
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
