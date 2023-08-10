using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using ShopSync.Context;
using ShopSync.Dtos;
using ShopSync.Service;
using ShopSync.Security;
using Microsoft.AspNetCore.Authorization;

namespace ShopSync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopSyncController : ControllerBase
    {
        ShopSyncService _ShopSyncService = new ShopSyncService();

        private readonly IConfiguration _configuration;

        public ShopSyncController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region Register Login
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserRequestDto model)
        {
            var result = _ShopSyncService.Register(model);
            return Ok(result);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDto model)
        {
            var result = _ShopSyncService.Login(model);
            if (result.Id > 0)
                result.Token = TokenHandler.CreateToken(_configuration, result);
            return Ok(result);
        }
        #endregion Register Login

        #region Category Transactions

        [Authorize]
        [HttpPost("CategoryAddOrUpdate")]
        public IActionResult CategoryAddOrUpdate([FromBody] CategoryRequestDto model)
        {
            var result = _ShopSyncService.CategoryAddOrUpdate(model);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            var result = _ShopSyncService.GetAllCategories();
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("CategoryDelete")]
        public IActionResult CategoryDelete([FromQuery] long id)
        {
            var result = _ShopSyncService.CategoryDelete(id);
            return Ok(result);
        }


        #endregion Category Transactions

        #region Product Transactions

        [Authorize]
        [HttpPost("ProductAddOrUpdate")]
        public IActionResult ProductAddOrUpdate([FromBody] ProductRequestDto model)
        {
            var result = _ShopSyncService.ProductAddOrUpdate(model);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts([FromQuery] ProductFilterDto model)
        {
            var result = _ShopSyncService.GetAllProducts(model);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetProductById")]
        public IActionResult GetProductById([FromQuery] long id)
        {
            var result = _ShopSyncService.GetProductById(id);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("ProductDelete")]
        public IActionResult ProductDelete([FromQuery] long id)
        {
            var result = _ShopSyncService.ProductDelete(id);
            return Ok(result);
        }

        #endregion Product Transactions

        #region ShoppingList Transactions



        [Authorize]
        [HttpPost("ShoppingListCreate")]
        public IActionResult ShoppingListCreate([FromBody] ShoppingListRequestDto model)
        {
            var result = _ShopSyncService.ShoppingListCreate(model);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("AddProductBuyShoppingList")]
        public IActionResult AddProductBuyShoppingList([FromBody] AddProductShoppingListRequestDto model)
        {
            var result = _ShopSyncService.AddProductBuyShoppingList(model);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("BoughtShoppingProduct")]
        public IActionResult BoughtShoppingProduct([FromBody] BoughtShoppingProductRequestDto model)
        {
            var result = _ShopSyncService.BoughtShoppingProduct(model);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetShoppingLists")]
        public IActionResult GetBasketLists([FromQuery] long userId)
        {
            var result = _ShopSyncService.GetShoppingLists(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetShoppingListById")]
        public IActionResult GetShoppingListById([FromQuery] long shoppingListId)
        {
            var result = _ShopSyncService.GetShoppingListById(shoppingListId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("ShoppingListProductDescriptionAddOrUpdate")]
        public IActionResult ShoppingListProductDescriptionAddOrUpdate([FromBody] ShoppingListProductDescriptionChangeRequestDto model)
        {
            var result = _ShopSyncService.ShoppingListProductDescriptionAddOrUpdate(model);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("ShoppingListDelete")]
        public IActionResult ShoppingListDelete([FromQuery] long shoppingListId)
        {
            var result = _ShopSyncService.ShoppingListDelete(shoppingListId);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("ShoppingListProductDelete")]
        public IActionResult ShoppingListProductDelete([FromQuery] long id)
        {
            var result = _ShopSyncService.ShoppingListProductDelete(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("IGoShopping")]
        public IActionResult IGoShopping([FromBody] IGoShoppingRequestDto model)
        {
            var result = _ShopSyncService.IGoShopping(model);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("ShoppingCompleted")]
        public IActionResult ShoppingCompleted([FromBody] IGoShoppingRequestDto model)
        {
            var result = _ShopSyncService.ShoppingCompleted(model);
            return Ok(result);
        }
        #endregion ShoppingList Transactions

    }
}