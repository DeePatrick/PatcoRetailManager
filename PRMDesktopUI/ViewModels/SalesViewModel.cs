using Caliburn.Micro;
using PRMDesktopUI.Library.Api;
using PRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PRMDesktopUI.ViewModels
{
    public class SalesViewModel: Screen
    {
        private IProductEndpoint _productEndpoint;
        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        private BindingList<ProductModel> _products;
        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private int _itemQuantity;
        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
                NotifyOfPropertyChange(() => SubTotal);
            }
        }
        public string SubTotal
        {
            get {

                //TODO replace with Calculation
                decimal subtotal = 0;

                foreach (var item in Cart)
                {

                    subtotal += (Convert.ToDecimal(item.Product.RetailPrice) * item.QuantityInCart);
                }
                return subtotal.ToString("C");
            }

        }
        public string Tax
        {
            get
            {
                //TODO replace with Calculation
                decimal tax = 0;

                foreach(var item in Cart)
                {

                    tax += (Convert.ToDecimal(item.Product.RetailPrice) * item.QuantityInCart) * );
                }
                return tax.ToString("C");
            }

        }
        public string Total
        {
            get
            {

                //TODO replace with Calculation
                return "£0.00";
            }

        }
        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                // make sure something is selected
                // make sure there is an item quantity
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }
               
                return output;
            }
        }
        public void AddToCart()
        {
            CartItemModel item = new CartItemModel
            {
                Product = SelectedProduct,
                QuantityInCart = ItemQuantity
            };
            Cart.Add(item);
        }
        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                //make sure something is selected
                // make sure there is an item quantity
                return output;
            }
        }
        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
        }
        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                //make sure something in cart

                return output;
            }
        }
        public void CheckOut()
        {

        }
    }
}





