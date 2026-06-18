using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class HomePage : ContentPage
{
	private readonly SettingService _settingService;
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
	}
}