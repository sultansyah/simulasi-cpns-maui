using SimulasiCPNS.Models;
using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

[QueryProperty(nameof(Mode), "mode")]
public partial class CategoryPage : ContentPage
{
    public string _mode = "";
    private readonly QuestionService _questionService;

    public string Mode
    {
        get => _mode;
        set
        {
            SetCardModeActive(value);
        }
    }

    public CategoryPage(QuestionService questionService)
	{
		InitializeComponent();
        _questionService = questionService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var categories = await _questionService.GetCategoriesAsync();
        CategoryListLayout.BindingContext = categories;
    }

    private void SetCardModeActive(string selectedMode)
    {
        _mode = selectedMode;

        var primaryColor = (Color)Application.Current!.Resources["Primary"];
        var defaultTitleColor = Color.FromArgb("#111827");
        var practiceDescriptionColor = Color.FromArgb("#9F1239");
        var simulationDescriptionColor = Color.FromArgb("#9A3412");
        var activeDescriptionColor = Color.FromArgb("#FFE4E6");

        if(selectedMode == "practice")
        {
            PracticeCard.BackgroundColor = primaryColor;
            SimulationCard.BackgroundColor = Colors.White;
            PracticeCheckBadge.IsVisible = true;
            SimulationCheckBadge.IsVisible = false;

            PracticeTitleLabel.TextColor = Colors.White;
            PracticeDescriptionLabel.TextColor = activeDescriptionColor;

            SimulationTitleLabel.TextColor = defaultTitleColor;
            SimulationDescriptionLabel.TextColor = simulationDescriptionColor;
            return;
        }

        PracticeCard.BackgroundColor = Colors.White;
        SimulationCard.BackgroundColor = primaryColor;
        PracticeCheckBadge.IsVisible = false;
        SimulationCheckBadge.IsVisible = true;

        PracticeTitleLabel.TextColor = defaultTitleColor;
        PracticeDescriptionLabel.TextColor = practiceDescriptionColor;

        SimulationTitleLabel.TextColor = Colors.White;
        SimulationDescriptionLabel.TextColor = activeDescriptionColor;
    }

    private async void OnPracticeTapped(object sender, TappedEventArgs args)
    {
        SetCardModeActive("practice");
    }

    private async void OnSimulationTapped(object sender, TappedEventArgs args)
    {
        SetCardModeActive("simulation");
    }

    private async void OnCategoryTapped(object sender, TappedEventArgs args)
    {
        if (sender is not Grid grid) return;
        if (grid.BindingContext is not CategoryDisplayItem category) return;

        await Shell.Current.GoToAsync(
            nameof(QuestionPage),
            new Dictionary<string, object>
            {
                ["mode"] = _mode,
                ["category"] = category
            }
        );
    }
}
