using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace Urban_Planning_Simulation
{
    // Interaction logic for MainScreen.xaml
    public partial class MainScreen : SurfaceWindow
    {
        // Default constructor.
        public MainScreen()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
        }

        // Occurs when the window is about to close. 
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        // Adds handlers for window availability events.
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        // Removes handlers for window availability events.>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        // This is called when the user can interact with the application's window.
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        // This is called when the user can see but not interact with the application's window.
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        // This is called when the application's window is not visible or interactive.
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        // Called when the start button is clicked. Go to the main window we'll be using
        // (probably a better way to do this than creating a new window but for now it works)
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            OpenModeScreen mainWindow = new OpenModeScreen();
            mainWindow.Show();
        }

        // Called when "Test Mode" button is clicked
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            // Next line for test type in test mode
            // ElementMenuItem option = (ElementMenuItem) sender;

            TestModeScreen testWindow = new TestModeScreen();
            testWindow.Show();
        }
    }
}