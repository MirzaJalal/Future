﻿using Future.Bangla.Web.Models;
using Future.Bangla.Web.Service.IService;
using Future.Bangla.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Future.Bangla.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult OrderIndex()
        {
            return View();
        }

        public async Task<IActionResult> OrderDetails(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();

            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            ResponseDto? responseDto = await _orderService.GetOrder(orderId);

            if(responseDto != null && responseDto.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(responseDto.Result));
            }
            if(!User.IsInRole(SD.RoleAdmin.ToUpper()) && userId != orderHeaderDto.UserId) 
            { 
                return NotFound();
            }
            return View(orderHeaderDto);
        }

        [HttpGet]
        public IActionResult GetAll () 
        { 
            List<OrderHeaderDto> orderHeaderList = new List<OrderHeaderDto>();

            string userId = "";

            if (!User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }

            ResponseDto response = _orderService.GetOrders(userId).GetAwaiter().GetResult();

            if(response.IsSuccess && response != null)
            {
               orderHeaderList =  JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
            }
            else
            {
                orderHeaderList = new List<OrderHeaderDto>();
            }

            return Json(new { data = orderHeaderList });
        }
    }
}
