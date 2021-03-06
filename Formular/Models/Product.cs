﻿
using System;

namespace Formular.Models
{
    public class Product : ICloneable
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int DefaultQuantity { get; }
        public string UnitOfMeasure { get; set; }

        public Product(string name, int quantity, string unitOfMeasure, int defaultQuantity = 0)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.UnitOfMeasure = unitOfMeasure;
            this.DefaultQuantity = defaultQuantity;
        }

        public override string ToString()
        {
            return string.Format("{0}x{1} {2}", Name, Quantity, UnitOfMeasure);
        }

        public object Clone()
        {
            Product newProduct = (Product)this.MemberwiseClone();

            return newProduct;
        }
    }
}
