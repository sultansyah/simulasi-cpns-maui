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

        await Task.Yield();
        await PlayIntroAnimationAsync();
    }

    private async Task PlayIntroAnimationAsync()
    {
        if (_hasPlayedIntroAnimation)
        {
            return;
        }

        _hasPlayedIntroAnimation = true;

        ModeSection.Opacity = 0;
        ModeSection.TranslationY = 18;

        HeadingSection.Opacity = 0;
        HeadingSection.TranslationY = 14;

        CategoryCard.Opacity = 0;
        CategoryCard.TranslationY = 22;

        foreach (var child in CategoryListLayout.Children.OfType<VisualElement>())
        {
            child.Opacity = 0;
            child.TranslationY = 12;
        }

        await Task.WhenAll(
            ModeSection.FadeTo(1, 220, Easing.CubicOut),
            ModeSection.TranslateTo(0, 0, 220, Easing.CubicOut));

        await Task.WhenAll(
            HeadingSection.FadeTo(1, 200, Easing.CubicOut),
            HeadingSection.TranslateTo(0, 0, 200, Easing.CubicOut));

        await Task.WhenAll(
            CategoryCard.FadeTo(1, 240, Easing.CubicOut),
            CategoryCard.TranslateTo(0, 0, 240, Easing.CubicOut));

        var animatedChildren = CategoryListLayout.Children.OfType<VisualElement>().ToList();

        for (var index = 0; index < animatedChildren.Count; index++)
        {
            var child = animatedChildren[index];
            await Task.WhenAll(
                child.FadeTo(1, 150, Easing.CubicOut),
                child.TranslateTo(0, 0, 150, Easing.CubicOut));
        }
    }
}
