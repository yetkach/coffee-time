using CoffeeTime.Logics.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;

namespace CoffeeTime.Logics.Services
{
    public class OrderGuidService : IOrderGuidService
    {
        private const string Key = "userId";
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrderGuidService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentGuid()
        {
            return httpContextAccessor.HttpContext.Session.GetString(Key);
        }

        public void SetNewGuid()
        {
            string newGuid = Guid.NewGuid().ToString();
            httpContextAccessor.HttpContext.Session.SetString(Key, newGuid);
        }
    }
}
