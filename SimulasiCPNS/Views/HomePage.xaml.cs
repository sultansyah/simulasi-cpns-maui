using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class HomePage : ContentPage
{
	private readonly SettingService _settingService;
    private bool _isMascotAnimationRunning;

	public HomePage(SettingService settingService)
	{
		InitializeComponent();

		_settingService = settingService;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var setting = await _settingService.GetSettingAsync();

		if(setting is not null)
		{
			GreetingLabel.Text = $"Halo, {setting.FullName}";
		}

        StartMascotAnimation();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _isMascotAnimationRunning = false;
        MascotContainer.AbortAnimation("MascotBob");
        MascotContainer.AbortAnimation("MascotBlink");
        MascotLampGlowOuter.AbortAnimation("MascotLampOuter");
        MascotLampGlowInner.AbortAnimation("MascotLampInner");
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
            _ = AnimateLampPulseAsync();

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

    private async Task AnimateLampPulseAsync()
    {
        await Task.WhenAll(
            MascotLampGlowOuter.FadeTo(0.34, 280, Easing.SinOut),
            MascotLampGlowOuter.ScaleTo(1.12, 280, Easing.SinOut),
            MascotLampGlowInner.FadeTo(0.88, 280, Easing.SinOut),
            MascotLampGlowInner.ScaleTo(1.08, 280, Easing.SinOut));

        await Task.WhenAll(
            MascotLampGlowOuter.FadeTo(0.12, 520, Easing.SinInOut),
            MascotLampGlowOuter.ScaleTo(1.0, 520, Easing.SinInOut),
            MascotLampGlowInner.FadeTo(0.42, 520, Easing.SinInOut),
            MascotLampGlowInner.ScaleTo(1.0, 520, Easing.SinInOut));
    }
}
