﻿using AutoMapper;
using Caliburn.Micro;
using PRMDesktopUI.Library.Api;
using PRMDesktopUI.Library.Helpers;
using PRMDesktopUI.Library.Models;
using PRMDesktopUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private ISaleEndpoint _saleEndpoint;
        private IConfigHelper _configHelper;
        private IMapper _mapper;
        public SalesViewModel(IProductEndpoint productEndpoint, ISaleEndpoint saleEndpoint, 
            IConfigHelper configHelper, IMapper mapper)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _configHelper = configHelper;
            _mapper = mapper;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private BindingList<ProductDisplayModel> _products;
        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }


        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private int _itemQuantity = 1;
        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
                NotifyOfPropertyChange(() => SubTotal);
                NotifyOfPropertyChange(() => Tax);
                NotifyOfPropertyChange(() => CanCheckOut);

            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            ////using foreach loop
            //foreach (var item in Cart)
            //{
            //    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            //}
           
            subTotal = Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart);
            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;
            return taxAmount = Cart.Where(item => item.Product.IsTaxable).Sum(item => item.Product.RetailPrice * item.QuantityInCart * taxRate);
            
        }   

        public string SubTotal => CalculateSubTotal().ToString("C");
        public string Tax => CalculateTax().ToString("C");

        public string Total
        {
            get
            {
                return (CalculateSubTotal() + CalculateTax()).ToString("C");
            }

        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }
        public void AddToCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }


            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);


        }
        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                if (Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }

        }

        public void RemoveFromCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart -= ItemQuantity;
                //HACK
                Cart.Add(existingItem);
                Cart.Remove(existingItem);
                
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Remove(item);
            }


            SelectedProduct.QuantityInStock += ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                //make sure something in cart
                if(Cart.Count > 0)
                    output = true;
                return output;
            }
        }
        public async Task CheckOut()
        {
            //create a new sales model to post to the API
            SaleModel sale = new SaleModel();
            foreach( var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                }); 
            }

            await _saleEndpoint.PostSale(sale);

        }
    }
}





