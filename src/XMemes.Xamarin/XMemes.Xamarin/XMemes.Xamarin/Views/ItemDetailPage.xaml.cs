using System.ComponentModel;

using Xamarin.Forms;

using XMemes.Xamarin.ViewModels;

namespace XMemes.Xamarin.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}