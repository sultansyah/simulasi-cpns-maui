using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class HomePage : ContentPage
{
    private readonly SettingService _settingService;
    private readonly QuestionService _questionService;
    private bool _isMascotAnimationRunning;
    private bool _hasPlayedIntroAnimation;

    public HomePage(SettingService settingService, QuestionService questionService)
    {
        InitializeComponent();

        _settingService = settingService;
        _questionService = questionService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var setting = await _settingService.GetSettingAsync();

        if (setting is not null)
        {
            GreetingLabel.Text = $"Halo, {setting.FullName}! 👋";
        }

        var featuredQuestion = await _questionService.GetFeaturedQuestionAsync();
        if (featuredQuestion is not null)
        {
            FeaturedCategoryIconLabel.Text = featuredQuestion.CategoryIcon;
            FeaturedCategoryLabel.Text = featuredQuestion.Category;
            FeaturedSubCategoryLabel.Text = $"{featuredQuestion.SubCategoryIcon} {featuredQuestion.SubCategory}";
        }

        StartMascotAnimation();
        await PlayIntroAnimationAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _isMascotAnimationRunning = false;
        MascotContainer.AbortAnimation("MascotBob");
        MascotContainer.AbortAnimation("MascotBlink");
    }

    private void StartMascotAnimation()
    {
        if (_isMascotAnimationRunning)
        {
            return;
        }

        _isMascotAnimationRunning = true;
        _ = AnimateMascotAsync();
    }

    private async Task AnimateMascotAsync()
    {
        while (_isMascotAnimationRunning)
        {
            await MascotContainer.TranslateTo(0, -4, 900, Easing.SinInOut);
            await MascotContainer.ScaleTo(1.02, 900, Easing.SinInOut);
            await MascotContainer.RotateTo(-2, 900, Easing.SinInOut);

            await MascotContainer.TranslateTo(0, 1, 900, Easing.SinInOut);
            await MascotContainer.ScaleTo(1.0, 900, Easing.SinInOut);
            await MascotContainer.RotateTo(2, 900, Easing.SinInOut);

            await MascotContainer.RotateTo(0, 450, Easing.SinOut);
            await Task.Delay(160);
        }
    }

    private async Task PlayIntroAnimationAsync()
    {
        if (_hasPlayedIntroAnimation)
        {
            return;
        }

        _hasPlayedIntroAnimation = true;

        HeroContent.Opacity = 0;
        HeroContent.TranslationY = 18;

        MascotContainer.Opacity = 0;
        MascotContainer.Scale = 0.92;

        ProgressCard.Opacity = 0;
        ProgressCard.TranslationY = 26;

        ContinueCard.Opacity = 0;
        ContinueCard.Scale = 0.97;
        ContinueCard.TranslationY = 18;

        PracticeCard.Opacity = 0;
        PracticeCard.TranslationY = 20;

        SimulationCard.Opacity = 0;
        SimulationCard.TranslationY = 24;

        ProgressPercentLabel.Scale = 0.92;

        await Task.WhenAll(
            HeroContent.FadeTo(1, 320, Easing.CubicOut),
            HeroContent.TranslateTo(0, 0, 320, Easing.CubicOut),
            MascotContainer.FadeTo(1, 360, Easing.CubicOut),
            MascotContainer.ScaleTo(1, 360, Easing.CubicOut));

        await Task.WhenAll(
            ProgressCard.FadeTo(1, 320, Easing.CubicOut),
            ProgressCard.TranslateTo(0, 0, 320, Easing.CubicOut));

        await Task.WhenAll(
            ContinueCard.FadeTo(1, 250, Easing.CubicOut),
            ContinueCard.ScaleTo(1, 250, Easing.CubicOut),
            ContinueCard.TranslateTo(0, 0, 250, Easing.CubicOut));

        await Task.WhenAll(
            PracticeCard.FadeTo(1, 220, Easing.CubicOut),
            PracticeCard.TranslateTo(0, 0, 220, Easing.CubicOut),
            SimulationCard.FadeTo(1, 280, Easing.CubicOut),
            SimulationCard.TranslateTo(0, 0, 280, Easing.CubicOut));

        await ProgressPercentLabel.ScaleTo(1, 260, Easing.CubicOut);

        await ContinueCard.ScaleTo(1.02, 180, Easing.SinInOut);
        await ContinueCard.ScaleTo(1.0, 180, Easing.SinInOut);
    }
}
