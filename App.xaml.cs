using Microsoft.Extensions.DependencyInjection;
using mUczen.Services;
namespace mUczen
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light;
            mUczen.Services.Database.Initialize();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}