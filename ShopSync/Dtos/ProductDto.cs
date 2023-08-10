﻿using ShopSync.Context;

namespace ShopSync.Dtos
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
    }
}
