using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class SplashPage : ContentPage
{
	private readonly SettingService _settingService;
    private readonly DatabaseService _databaseService;
    private bool _hasNavigated;

	public SplashPage(SettingService settingService, DatabaseService databaseService)
	{
		InitializeComponent();

		_settingService = settingService;
        _databaseService = databaseService;

    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// seeding
		await _databaseService.SeedQuestionsAsync();

        if (_hasNavigated) return;

		_hasNavigated = true;

		var setting = await _settingService.GetSettingAsync();
		if(setting is null || string.IsNullOrWhiteSpace(setting.FullName))
		{
			await Shell.Current.GoToAsync("//onboarding");
			return;
		}
        await Shell.Current.GoToAsync("//home");
    }
}
