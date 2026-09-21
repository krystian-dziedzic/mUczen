namespace mUczen
{
    public partial class MainPage : ContentPage
    {
        private bool isPopupVisible = false;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Animate_Logo(object sender, EventArgs e)
        {
            while(true)
            {
                await Logo.ScaleToAsync(1.15, 800);
                await Logo.ScaleToAsync(1.0, 800);
            }
        }
        private void DisplayLoginPopup(object sender, EventArgs e)
        {
            if (!isPopupVisible)
            {
                isPopupVisible = true;
                LoginPopup.IsVisible = true;
            }
            //Chce zrobic animacje "wysuwania" popupu i takiego jakby przemieszczenia się loga na ten popup
            //Ale nie mam juz dzisiaj czasu więc to dla jutrzejszego mnie ;)
        }
    }
}
