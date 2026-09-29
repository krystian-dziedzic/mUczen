using Microsoft.Extensions.DependencyInjection;
using mUczen.Services;
namespace mUczen
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            mUczen.Services.Database.Initialize();

            mUczen.Services.Database.testFunctionInsert();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}