using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class SplashPage : ContentPage
{
	private readonly SettingService _settingService;

	public SplashPage(SettingService settingService)
	{
		InitializeComponent();

        _settingService = settingService;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await Task.Delay(1000);

		var setting = await _settingService.GetSettingAsync();
		if (setting is null || string.IsNullOrWhiteSpace(setting.FullName))
		{
			await Shell.Current.GoToAsync("//onboarding");
		}

		await Shell.Current.GoToAsync("//home");
	}
}