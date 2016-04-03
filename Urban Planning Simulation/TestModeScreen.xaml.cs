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
    // Interaction logic for TestModeScreen.xaml
    public partial class TestModeScreen : SurfaceWindow
    {
        private const bool DEBUG_MODE = true;

        // Default constructor.
        public TestModeScreen()
        {
            // Set up layout
            InitializeComponent();

            if (DEBUG_MODE)
            {
                TestModeDebugText.Visibility = Visibility.Visible;
            }

            createTestOne();

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

        // Removes handlers for window availability events.
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

        //======================================================================
        //                       TestCase Functions
        //======================================================================
        private void createTestOne()
        {
            // Create house ScatterViewItems in proper position
            ScatterViewItem houseOne = new ScatterViewItem();
            houseOne = SetSVHouseImage("HouseEMI");
            houseOne.CanMove = false;
            houseOne.CanScale = false;
            houseOne.CanRotate = false;
            houseOne.Center = new Point(200, 200);

            TestScatterView.Items.Add(houseOne);
        }

        //======================================================================
        //                       Helper Functions
        //======================================================================
        private ScatterViewItem SetSVHouseImage(String type)
        {
            ScatterViewItem item = new ScatterViewItem();
            BitmapImage img = new BitmapImage();

            if (type.Equals("HouseEMI", StringComparison.Ordinal))
            {
                img = new BitmapImage(new Uri("Resources/iso_house_1.png", UriKind.Relative));

            }
            else if (type.Equals("BuildingEMI", StringComparison.Ordinal))
            {
                img = new BitmapImage(new Uri("Resources/iso_building_1.png", UriKind.Relative));
            }
            else if (type.Equals("SkyscraperEMI", StringComparison.Ordinal))
            {
                img = new BitmapImage(new Uri("Resources/iso_skyscraper_1.png", UriKind.Relative));
            }

            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = img;
            item.Background = imgBrush;

            return item;
        }

        // When in debug mode, the coordinates of mouse are shown in corner
        private void TestScatterView_MouseMove(object sender, MouseEventArgs e)
        {
            if (DEBUG_MODE)
            {
                Point location = e.GetPosition(this);
                TestModeDebugText.Text = "(" + location.ToString() + ")";
            }
        }
    }
}