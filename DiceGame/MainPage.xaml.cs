/*
 * DiceGame MainPage.xaml.cs
 * Logic associated with MainPage.xaml
 * Author- Cameron Grande
 * Date Developed- 9/26/22
 * Last Changed- 5/14/23
 * Rev: 3
 */

namespace DiceGame;

using MauiSpeechToTextSample;
using Plugin.Maui.Audio;
using System.Diagnostics;
using System.Globalization;

public partial class MainPage : ContentPage
{

    //declare global variables
    Random rnd = new Random();

    //init page
    public MainPage()
    {
        InitializeComponent();
        
        Accelerometer.Default.ShakeDetected += Accelerometer_ShakeDetected;
        if (Accelerometer.IsSupported)
        {
            Accelerometer.Start(SensorSpeed.UI);
        }
        if (Gyroscope.IsSupported)
        {
            Gyroscope.Start(SensorSpeed.UI);
        }
    }

    private ISpeechToText speechToText;
    private CancellationTokenSource tokenSource = new CancellationTokenSource();

    public Command ListenCommand { get; set; }
    public Command ListenCancelCommand { get; set; }
    public string RecognitionText { get; set; }

    public MainPage(ISpeechToText speechToText)
    {
        InitializeComponent();

        this.speechToText = speechToText;

        ListenCommand = new Command(Listen);
        ListenCancelCommand = new Command(ListenCancel);
        BindingContext = this;

        Accelerometer.Default.ShakeDetected += Accelerometer_ShakeDetected;
        if (Accelerometer.IsSupported)
        {
            Accelerometer.Start(SensorSpeed.UI);
        }
        if (Gyroscope.IsSupported)
        {
            Gyroscope.Start(SensorSpeed.UI);
        }
    }

    private async void Listen()
    {
        var isAuthorized = await speechToText.RequestPermissions();
        if (isAuthorized)
        {
            listenButton.Text = "Listening...";
            listenButton.IsEnabled = false;
            try
            {
                RecognitionText = await speechToText.Listen(CultureInfo.GetCultureInfo("en-US"),
                    new Progress<string>(partialText =>
                    {
                        if (DeviceInfo.Platform == DevicePlatform.Android)
                        {
                            RecognitionText = partialText;
                        }
                        else
                        {
                            RecognitionText += partialText + " ";
                        }

                        OnPropertyChanged(nameof(RecognitionText));
                    }), tokenSource.Token);
            }
            catch (Exception ex)
            {
                RecognitionText = "Predicting fortune...";
                await DisplayAlert("Error", ex.Message, "OK");
            }
            
            ListenResults.Text = RecognitionText;
            rollDiceLogic();
        }
        else
        {
            ListenResults.Text = "Predicting fortune...";
            rollDiceLogic();
        }
        listenButton.IsEnabled = true;
        listenButton.Text = "Listen";
    }

    private void ListenCancel()
    {
        tokenSource?.Cancel();
    }

    public void ListenButton(object sender, EventArgs e)
    {
        
        Listen();
        
    }

    /*
     * rollDice- logic associated with clicking the roll button
     * @param sender- object that triggered action
     * @param eventArgs- arguments from action
     * @returns none
     */
    private void rollDice(object sender, EventArgs e)
    {
        rollDiceLogic();

    }

    /*
     * Accelerometer_ShakeDetected- triggered when device shook
     * @returns none
     */
    void Accelerometer_ShakeDetected(object sender, EventArgs e)
    {
        if (Preferences.Get("Gyroscope", true))
        {
            rollDiceLogic();

        }
    }

    /*
     * rollDice- main events that occur when dice is rolled.
     * can be triggered from device shake or button click
     * @returns none
     */
    private void rollDiceLogic()
    {
        playRollSound();

        if (Preferences.Get("Vibrations", true))
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(TimeSpan.FromSeconds(1));
            }

        }


        int dice = rnd.Next(1, 7);

        ////toggle flashlight <diceroll> times
        if (Preferences.Get("Flash", true))
        {
            triggerFlashlight(dice);
        }


        //update image on screen
        if (dice == 1)
            CurrentDiceImage.Source = "dice_one.svg";
        else if (dice == 2)
            CurrentDiceImage.Source = "dice_two.svg";
        else if (dice == 3)
            CurrentDiceImage.Source = "dice_three.svg";
        else if (dice == 4)
            CurrentDiceImage.Source = "dice_four.svg";
        else if (dice == 5)
            CurrentDiceImage.Source = "dice_five.png";
        else if (dice == 6)
            CurrentDiceImage.Source = "dice_six.svg";

        //game logic- update state based on event
        switch (dice)
        {
            case 1:
                CurrentState.Text = "The dice has divined that it is unlikely to happen";
                break;
            case 2:
                CurrentState.Text = "The dice has divined that it could occur.";
                break;
            case 3:
                CurrentState.Text = "The future is murky and unable to be divined";
                break;
            case 4:
                CurrentState.Text = "The dice has divined that it is very likely to happen";
                break;
            case 5:
                CurrentState.Text = "The dice has divined that it will happen";

                break;
            case 6:
                CurrentState.Text = "Run while you still can! You have angered the Gods!";
                break;
        }

        if (Preferences.Get("TextToSpeech", true)) { 
            TextToSpeech.Default.SpeakAsync(CurrentState.Text);
            SemanticScreenReader.Announce(CurrentState.Text);
        }
    }

    /*
    * playRollSound- makes sound of dice rolling
    * @param diceValue- how many times to toggle flashlight
    * @returns none
    */
    public async void playRollSound()
    {
        AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("dicerolling.wav")).Play();
    }

    /*
    * triggerFlashLight- triggers flashlight number of times passed in
    * @param diceValue- how many times to toggle flashlight
    * @returns none
    */
    public void triggerFlashlight(int diceValue)
    {
        try
        {
            for (int i = 0; i < diceValue; i++)
            {
                Flashlight.Default.TurnOnAsync();
                Thread.Sleep(100);
                Flashlight.Default.TurnOffAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    /*
     * onCounterClicked- logic associated with clicking help button (loads help page)
     * @returns none
     */
    private void Help(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Help());
    }
}
