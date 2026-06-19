using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class CategoryPage : ContentPage
{
    private readonly QuestionService _questionService;
    private bool _hasPlayedIntroAnimation;

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
}
