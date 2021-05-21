using AutoMapper;
using CoffeeTime.Logics.Dto;
using CoffeeTime.Logics.Infrastructure;
using CoffeeTime.Logics.Interfaces;
using CoffeeTime.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeTime.Web.Controllers
{
    public class CoffeeController : Controller
    {
        private readonly ICoffeeService coffeeService;
        private readonly IMapper mapper;

        public CoffeeController(ICoffeeService coffeeService, IMapper mapper)
        {
            this.coffeeService = coffeeService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var coffeeDataDtos = await coffeeService.GetAllCoffeesDataAsync();
                var viewModel = mapper.Map<List<CoffeeForShowcaseViewModel>>(coffeeDataDtos);

                ViewBag.Title = "Choose coffee";

                return View(viewModel);
            }
            catch (NotFoundException)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("Error");
            }
            catch
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(string name)
        {
            try
            {
                var coffeeDataDto = await coffeeService.GetCoffeeDataAsync(name);
                var model = mapper.Map<CoffeeViewModel>(coffeeDataDto);

                ViewBag.Title = "Customize Coffee";

                return View(model);
            }
            catch (NotFoundException)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("Error");
            }
            catch
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CoffeeViewModel model)
        {
            try
            {
                CoffeeDto coffeeDto = mapper.Map<CoffeeDto>(model);
                await coffeeService.CreateNewCoffeeAsync(coffeeDto);
            }
            catch (NotFoundException)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("Error");
            }
            catch
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return View("Error");
            }

            return RedirectToAction("Create", "Order");
        }
    }
}
