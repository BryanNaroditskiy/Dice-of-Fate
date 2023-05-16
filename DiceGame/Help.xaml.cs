/*
 * DiceGame Help.xaml.cs
 * Logic associated with Help.xaml
 * Course- IST440
 * Author- Cameron Grande
 * Date Developed- 9/26/22
 * Last Changed- 9/29/22
 * Rev: 2
 */

namespace DiceGame;

public partial class Help : ContentPage
{

    /*
     * Constructor- load page
     */
	public Help()
	{
		InitializeComponent();
	}

    /*
      * returnHome- returns home
      * @param sender- object that triggered action
      * @param eventArgs- arguments from action
      * @returns none
      */
    private void ReturnHome(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}