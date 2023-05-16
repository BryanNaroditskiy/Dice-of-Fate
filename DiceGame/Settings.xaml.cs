namespace DiceGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            vibrationsSwitch.IsToggled = Preferences.Get("Vibrations",true);
            flashSwitch.IsToggled = Preferences.Get("Flash", true);
            gyroscopeSwitch.IsToggled = Preferences.Get("Gyroscope", true);
            textToSpeechSwitch.IsToggled = Preferences.Get("TextToSpeech", true);
        }

        private void ReturnHome(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("Vibrations", vibrationsSwitch.IsToggled);
            Preferences.Set("Flash", flashSwitch.IsToggled);
            Preferences.Set("Gyroscope", gyroscopeSwitch.IsToggled);
            Preferences.Set("TextToSpeech", textToSpeechSwitch.IsToggled);
        }
    }
}