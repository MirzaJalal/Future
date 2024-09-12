using Future.Bangla.Web.Models;
using Future.Bangla.Web.Service;
using Future.Bangla.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Future.Bangla.Web.Controllers
{
    public class CouponsController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list = new List<CouponDto>();

            ResponseDto? response = await _couponService.GetAllCouponAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

		public async Task<IActionResult> CouponCreate()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto coupon)
        {
            if (ModelState.IsValid)
            {
                var responseDto = await _couponService.CreateCouponsAsync(coupon);

                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Coupon Created Successfully!";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message;
                }

            }
            return View(coupon);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
			ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

			if (response != null && response.IsSuccess)
			{
				CouponDto? coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(coupon);
			}
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto coupon)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(coupon.CouponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Deleted Successfully!";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(coupon);
        }
    }

}
