using Microsoft.AspNetCore.Identity;
using ShopSync.Context;
using ShopSync.Dtos;
using ShopSync.Security;
using System;
using System.Text.RegularExpressions;

namespace ShopSync.Service
{
    public class ShopSyncService
    {
        ShopSyncContext ShopSyncContext = new ShopSyncContext();
        private static Random random = new Random();
        #region Register Login
        public string Register(UserRequestDto model)
        {
            if (model.Email == null) { return "The mail field cannot be empty."; }
            if (model.Name == null) { return "The name field cannot be empty."; }
            if (model.Surname == null) { return "Surname field cannot be empty."; }
            if (!IsPasswordValid(model.Password)) { return "Password must be at least 8 characters and contain uppercase, lowercase and numbers."; }
            if (model.Email == "admin@gmail.com") { return "You cannot create admin user"; }

            var userRepo = ShopSyncContext.Users.ToList();
            var userCheck = userRepo.Where(x => x.Email == model.Email).FirstOrDefault();

            if (userCheck != null)
            {
                return "This e-mail address has been used before";
            }
            else
            {
                var user = new User();
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Password = model.Password;
                user.Email = model.Email;
                ShopSyncContext.Add(user);
                ShopSyncContext.SaveChanges();
                return "The user has been successfully created.";

            }
        }

        public static bool IsPasswordValid(string password)
        {
            string pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\\$%\\^&\\*])(?=.{8,})";
            bool isValid = Regex.IsMatch(password, pattern);

            return isValid;
        }

        public UserResponseDto Login(LoginRequestDto model)
        {
            if (model.Email == "admin@gmail.com" && model.Password == "12345")
            {
                UserResponseDto adminUser = new UserResponseDto()
                {
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    Surname = "User",
                    Id = 666,
                };
                return adminUser;
            }

            var userRepo = ShopSyncContext.Users.ToList();
            var userCheck = userRepo.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();
            if (userCheck != null)
            {
                var user = new UserResponseDto()
                {
                    Email = userCheck.Email,
                    Name = userCheck.Name,
                    Surname = userCheck.Surname,
                    Id = userCheck.Id,
                };
                return user;
            }
            else
            {
                return new UserResponseDto();

            }
        }
        #endregion Register Login

        #region Category Transactions
        public string CategoryAddOrUpdate(CategoryRequestDto model)
        {
            if (model.Name == null) { return "Category Name cannot be empty"; }
            var categoriesRepo = ShopSyncContext.Categories.ToList();
            var checkCategoryName = categoriesRepo.Where(x => x.Name == model.Name).FirstOrDefault();
            if (checkCategoryName != null)
                return "There is already a category with this name, you cannot add it.";

            if (model.Id > 0)
            {
                var dbItem = categoriesRepo.Where(x => x.Id == model.Id).FirstOrDefault();
                if (dbItem != null)
                {

                    dbItem.Name = model.Name;
                    ShopSyncContext.SaveChanges();
                    return "Category Updated Successfully.";
                }
            }

            var categorie = new Category();
            categorie.Name = model.Name;
            ShopSyncContext.Add(categorie);
            ShopSyncContext.SaveChanges();
            return "Category Added Successfully.";
        }
        public List<Category> GetAllCategories()
        {
            return ShopSyncContext.Categories.ToList();
        }
        public string CategoryDelete(long id)
        {
            var deletedCategory = ShopSyncContext.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (deletedCategory != null)
            {
                ShopSyncContext.Remove(deletedCategory);
                ShopSyncContext.SaveChanges();
                return "The category has been successfully deleted.";
            }
            return "Category not found.";
        }
        #endregion Category Transactions

        #region Product Transactions
        public string ProductAddOrUpdate(ProductRequestDto model)
        {
            if (model.Name == null) { return "Product Name cannot be empty"; }

            var productRepo = ShopSyncContext.Products.ToList();
            var checkproduct = productRepo.Where(x => x.Name == model.Name).FirstOrDefault();
            if (checkproduct != null)
                return "There is already a product with this name, change the product name.";

            if (model.Id > 0)
            {
                var dbItem = productRepo.Where(x => x.Id == model.Id).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.Name = model.Name;
                    dbItem.CategoryId = model.CategoryId;
                    dbItem.Image = model.Image;
                    ShopSyncContext.SaveChanges();
                    return "The product has been successfully updated";
                }
            }

            var product = new Products();
            product.Name = model.Name;
            product.CategoryId = model.CategoryId;
            product.Image = model.Image;
            ShopSyncContext.Add(product);
            ShopSyncContext.SaveChanges();
            return "The product has been successfully added";
        }
        public List<Products> GetAllProducts(ProductFilterDto model)
        {

            if (!String.IsNullOrEmpty(model.GeneralSearch) && model.CategoryId > 0)
            {
                var lowerCase = model.GeneralSearch.ToLower();
                var prodCheck = ShopSyncContext.Products.Where(x => x.Name.ToLower().Contains(lowerCase) && x.CategoryId == model.CategoryId).Skip(model.Cursor).Take(10).ToList();

                foreach (var product in prodCheck)
                {
                    product.Category = ShopSyncContext.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
                }
                return prodCheck;
            }
            else if (!String.IsNullOrEmpty(model.GeneralSearch) && (model.CategoryId == 0 || model.CategoryId == null))
            {
                var lowerCase = model.GeneralSearch.ToLower();
                var prodCheck = ShopSyncContext.Products.Where(x => x.Name.ToLower().Contains(lowerCase)).Skip(model.Cursor).Take(10).ToList();
                foreach (var product in prodCheck)
                {
                    product.Category = ShopSyncContext.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
                }
                return prodCheck;
            }
            else if (String.IsNullOrEmpty(model.GeneralSearch) && model.CategoryId > 0)
            {
                var prodCheck = ShopSyncContext.Products.Where(x => x.CategoryId == model.CategoryId).Skip(model.Cursor).Take(10).ToList();
                foreach (var product in prodCheck)
                {
                    product.Category = ShopSyncContext.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
                }
                return prodCheck;
            }
            else
            {
                var prodList = ShopSyncContext.Products.Skip(model.Cursor).Take(10).ToList();
                foreach (var product in prodList)
                {
                    product.Category = ShopSyncContext.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
                }
                return prodList;
            }
        }
        public Products? GetProductById(long id)
        {
            var product = ShopSyncContext.Products.Where(x => x.Id == id).FirstOrDefault();
            if (product != null)
            {
                product.Category = ShopSyncContext.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
                return product;
            }
            return null;
        }
        public string ProductDelete(long id)
        {
            var deletedProduct = ShopSyncContext.Products.Where(x => x.Id == id).FirstOrDefault();
            if (deletedProduct != null)
            {
                ShopSyncContext.Remove(deletedProduct);
                ShopSyncContext.SaveChanges();
                return "The product has been successfully deleted.";
            }
            return "Product not found.";
        }
        #endregion Product Transactions

        #region ShoppingList Transactions
        public string ShoppingListCreate(ShoppingListRequestDto model)
        {
            var shoppingListCheck = ShopSyncContext.ShoppingLists.Where(x => x.Name == model.Name && x.UserId == model.UserId).FirstOrDefault();

            if (shoppingListCheck != null)
                return "You already have a list with this name, please choose another name.";

            var shoppingList = new ShoppingList();
            shoppingList.UserId = model.UserId;
            shoppingList.Name = model.Name;
            shoppingList.IsBought = false;
            shoppingList.IsGoShopping = false;
            shoppingList.ShoppingListId = random.Next(1000000, 10000000);

            ShopSyncContext.Add(shoppingList);
            ShopSyncContext.SaveChanges();
            return "List Created.";
        }

        public string AddProductBuyShoppingList(AddProductShoppingListRequestDto model)
        {
            var shoppingListCheck = ShopSyncContext.ShoppingLists.Where(x => x.ShoppingListId == model.ShoppingListId && x.UserId == model.UserId).FirstOrDefault();

            if (shoppingListCheck == null)
            {
                return "List Not Found";
            }

            var allShoppingList = ShopSyncContext.ShoppingLists.Where(x => x.ShoppingListId == model.ShoppingListId && x.UserId == model.UserId).ToList();

            foreach (var item in allShoppingList)
            {
                if (item.IsBought == false && item.ProductId == model.ProductId)
                {
                    return "This product is already in the list, you cannot add the same product twice.";

                }
            }
            var shoppingList = new ShoppingList();
            shoppingList.UserId = model.UserId;
            shoppingList.IsBought = false;
            shoppingList.IsGoShopping = false;
            shoppingList.ProductId = model.ProductId;
            shoppingList.ShoppingListId = shoppingListCheck.ShoppingListId;
            shoppingList.Name = shoppingListCheck.Name;
            shoppingList.Description = model.Description;

            ShopSyncContext.Add(shoppingList);
            ShopSyncContext.SaveChanges();
            return "The product has been successfully added to the list.";
        }




        public List<ShoppingList> GetShoppingLists(long userId)
        {
            var shoppingListRepo = ShopSyncContext.ShoppingLists.Where(x => x.UserId == userId).ToList();

            shoppingListRepo = shoppingListRepo.DistinctBy(x => x.ShoppingListId).ToList();

            return shoppingListRepo;
        }

        public List<ShoppingList> GetShoppingListById(long shoppingListId)
        {
            var shoppingListRepo = ShopSyncContext.ShoppingLists.Where(x => x.ShoppingListId == shoppingListId && x.ProductId != null).ToList();

            return shoppingListRepo;
        }

        public string BoughtShoppingProduct(BoughtShoppingProductRequestDto model)
        {
            var shoppingRepo = ShopSyncContext.ShoppingLists.Where(x => x.ShoppingListId == model.ShoppingListId && x.ProductId == model.ProductId && x.UserId == model.UserId).FirstOrDefault();
            if (shoppingRepo == null)
                return "This product was not found in the list.";

            shoppingRepo.IsBought = true;
            ShopSyncContext.Update(shoppingRepo);
            ShopSyncContext.SaveChanges();
            return "You have successfully purchased the product";

        }

        public string ShoppingListProductDescriptionAddOrUpdate(ShoppingListProductDescriptionChangeRequestDto model)
        {
            var shoppingRepo = ShopSyncContext.ShoppingLists.Where(x => x.ShoppingListId == model.ShoppingListId && x.UserId == model.UserId && x.ProductId == model.ProductId && x.IsBought == false).FirstOrDefault();
            if (shoppingRepo == null)
                return "List Not Found";

            shoppingRepo.Description = model.Description;
            ShopSyncContext.Update(shoppingRepo);
            ShopSyncContext.SaveChanges();
            return "Description Added/Updated Successfully";

        }

        public string ShoppingListDelete(long shoppingListId)
        {
            var deletedBasketItem = ShopSyncContext.ShoppingLists.Where(x => x.ShoppingListId == shoppingListId).ToList();
            if (deletedBasketItem.Count() > 0)
            {
                foreach (var item in deletedBasketItem)
                {
                    ShopSyncContext.Remove(item);
                    ShopSyncContext.SaveChanges();
                }
                return "List Successfully Deleted";

            }

            return "List not found.";
        }

        public string ShoppingListProductDelete(long id)
        {
            var deletedBasketItem = ShopSyncContext.ShoppingLists.Where(x => x.Id == id).FirstOrDefault();
            if (deletedBasketItem != null)
            {
                ShopSyncContext.Remove(deletedBasketItem);
                ShopSyncContext.SaveChanges();
                return "Product Deleted Successfully";

            }

            return "The product was not found.";
        }

        public string IGoShopping(IGoShoppingRequestDto model)
        {
            var shoppingRepo = ShopSyncContext.ShoppingLists.Where(x => x.Id == model.ShoppingListId && x.UserId == model.UserId).ToList();
            if (shoppingRepo == null)
                return "List Not Found";

            foreach (var shopping in shoppingRepo)
            {
                shopping.IsGoShopping = true;
                ShopSyncContext.Update(shopping);
                ShopSyncContext.SaveChanges();
            }
            return "You went shopping.";

        }

        public string ShoppingCompleted(IGoShoppingRequestDto model)
        {
            var shoppingRepo = ShopSyncContext.ShoppingLists.Where(x => x.Id == model.ShoppingListId && x.UserId == model.UserId).ToList();
            if (shoppingRepo == null)
                return "List Not Found";

            foreach (var shopping in shoppingRepo)
            {
                shopping.IsGoShopping = true;
                ShopSyncContext.Update(shopping);
                ShopSyncContext.SaveChanges();
            }
            return "Shopping is complete.";

        }
        #endregion ShoppingList Transactions    


    }
}
