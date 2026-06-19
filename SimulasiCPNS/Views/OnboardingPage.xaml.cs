using SimulasiCPNS.Models;
using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class OnboardingPage : ContentPage
{
	private readonly SettingService _settingService;

	public OnboardingPage(SettingService settingService)
	{
		InitializeComponent();

		_settingService = settingService;
    }

	private async void OnStartClicked(object sender, EventArgs e)
	{
		var fullname = FullNameEntry.Text?.Trim();
		if(string.IsNullOrWhiteSpace(fullname))
		{
			await DisplayAlertAsync("Oops", "Nama lengkap wajib diisi", "Ok");
			return;
		}

		var setting = new Setting
		{
			FullName = fullname,
			ReminderEnabled = ReminderSwitch.IsToggled,
			ReminderTime = ReminderTimePicker.Time?.ToString(@"hh\:mm") ?? ""
		};

		await _settingService.SaveSettingAsync(setting);
		await Shell.Current.GoToAsync("//home");
	}
}