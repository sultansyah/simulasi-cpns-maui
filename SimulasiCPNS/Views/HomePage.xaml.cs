using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class HomePage : ContentPage
{
	private readonly SettingService _settingService;
    private readonly QuestionService _questionService;
    private bool _isMascotAnimationRunning;

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

		if(setting is not null)
		{
			GreetingLabel.Text = $"Halo, {setting.FullName}";
		}

        var categories = await _questionService.GetCategoriesAsync();
        CategoryCollectionView.ItemsSource = categories;

        StartMascotAnimation();
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
            await MascotContainer.TranslateTo(0, -5, 850, Easing.SinInOut);
            await MascotContainer.ScaleTo(1.03, 850, Easing.SinInOut);
            await MascotContainer.RotateTo(-3, 850, Easing.SinInOut);

            await MascotContainer.TranslateTo(0, 1, 850, Easing.SinInOut);
            await MascotContainer.ScaleTo(1.0, 850, Easing.SinInOut);
            await MascotContainer.RotateTo(3, 850, Easing.SinInOut);

            await MascotContainer.RotateTo(0, 500, Easing.SinOut);
            await Task.Delay(150);
        }
    }
}
