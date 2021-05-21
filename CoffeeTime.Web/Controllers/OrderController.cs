using AutoMapper;
using CoffeeTime.Logics.Dto;
using CoffeeTime.Logics.Infrastructure;
using CoffeeTime.Logics.Interfaces;
using CoffeeTime.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeTime.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var orderDto = await orderService.GetCurrentOrderAsync();
                var orderViewModel = mapper.Map<OrderViewModel>(orderDto);

                ViewBag.Title = "Checkout";

                return View(orderViewModel);
            }
            catch (NotFoundException)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewBag.Message = "Your cart is empty";

                return View("Error");
            }
            catch
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                OrderDto order = mapper.Map<OrderDto>(model);
                await orderService.CheckoutAsync(order);

                ViewBag.Message = "Your order is completed";
                return View("Complete");

            }
            catch (NotFoundException)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;

                return View("ErrorCart");
            }
            catch
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return View("Error");
            }
        }
    }
}
