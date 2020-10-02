using System;
using System.Collections.Generic;
using XMemes.Xamarin.ViewModels;
using XMemes.Xamarin.Views;
using Xamarin.Forms;

namespace XMemes.Xamarin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
