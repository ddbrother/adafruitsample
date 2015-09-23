﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lesson_201
{
    /// <summary>
    /// The application main page.  Because we are running headless we will not see anything
    /// even though it is begin generated at runtime.  This acts as the main entry point for the 
    /// application functionality.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // which GPIO pin do we want to use to control the LED light
        const int GPIOToUse = 18;

        // The class which wraps our LED.
        InternetLed internetLed;

        public MainPage()
        {
            this.InitializeComponent();
        }

        // This method will be called by the application framework when the page is first loaded.
        protected override async void OnNavigatedTo(NavigationEventArgs navArgs)
        {
            Debug.WriteLine("MainPage::OnNavigatedTo");

            MakePinWebAPICall();

            try
            {
                // Create a new InternetLed object
                internetLed = new InternetLed(GPIOToUse);

                // Initialize it for use
                internetLed.InitalizeLed();

                // Now have it make the web API call and get the led blink delay
                int blinkDelay = await internetLed.GetBlinkDelayFromWeb();

                for (int i = 0; i < 100; i++)
                {
                    internetLed.Blink();
                    await Task.Delay(blinkDelay);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        // This will put your pin on the world map of makers using this lesson.
        // Microsoft will receive the IP address of your Raspberry Pi2
        // this will be used to determine the rough geographic location of the device, in 
        // latitude and longitude.  This information will be stored for use in generating the
        // pin map showing the location of people who have also run this sample.
        // This data will not be shared with any outside party.
        /// </summary>
        public void MakePinWebAPICall()
        {
            HttpClient client = new HttpClient();

            // Comment this line to opt out of the pin map.
            client.GetStringAsync("http://adafruitsample.azurewebsites.net/api?Lesson=201");
        }

    }
}