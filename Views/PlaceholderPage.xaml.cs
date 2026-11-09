namespace mUczen.Views;

public partial class PlaceholderPage : ContentPage
{
    public PlaceholderPage(string title)
    {
        InitializeComponent();
        Title = title;
        SectionTitleLabel.Text = title;
    }
}