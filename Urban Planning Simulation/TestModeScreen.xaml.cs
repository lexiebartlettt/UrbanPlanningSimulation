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
        // Constants
        private const int ROAD_BUTTON = 1;
        private const int HOUSE_BUTTON = 2;
        private const bool DEBUG_MODE = true;

        // Set default mode type
        private String houseType = "HouseEMI";

        // Flags for the current mode
        private Boolean canPlaceHouse;
        private Boolean canPlaceRoad;

        // Default constructor.
        public TestModeScreen()
        {
            // Set up layout
            InitializeComponent();
            InitializePanels();

            // Set debug mode settings
            if (DEBUG_MODE)
            {
                TestModeDebugText.Visibility = Visibility.Visible;
            }

            // Create test
            createTestOne();

            // Default to house mode
            HouseBorder.BorderThickness = new Thickness(5);
            canPlaceHouse = true;
            // RoadCanvas.IsEnabled = false;

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
        //                       Button Functions
        //======================================================================

        // When house button is clicked
        private void HouseButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonMode(HOUSE_BUTTON);

            // Set house flag and house type
            ElementMenuItem button = (ElementMenuItem)sender;
            houseType = button.Name;
        }

        // When road button is clicked
        private void RoadButton_Click(object sender, RoutedEventArgs e)
        {

            SetButtonMode(ROAD_BUTTON);
        }

        //======================================================================
        //                       Canvas Functions
        //======================================================================

        // For mouse clicks
        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (canPlaceHouse)
            {
                // RoadCanvas.IsEnabled = false;
                // redoList = new List<Object>();
                e.Handled = true;
                TestScatterView.UpdateLayout();
                Point mousePosition = e.GetPosition(this);

                // Setting the ScatterView image background
                ScatterViewItem item = new ScatterViewItem();
                item = SetSVHouseImage(item, houseType);

                item.Center = mousePosition;
                item.Orientation = 0;
                TestScatterView.Items.Add(item);
            }
            else if (canPlaceRoad)
            {
                // RoadCanvas.IsEnabled = true;
            }
        }

        // For hold gestures
        private void gesturebox_HoldGesture(object sender, TouchEventArgs e)
        {
            if (canPlaceHouse)
            {
                // redoList = new List<Object>();
                e.Handled = true;
                TestScatterView.UpdateLayout();
                Point p = e.TouchDevice.GetPosition(this);

                // Setting the ScatterView image background
                ScatterViewItem item = new ScatterViewItem();
                item = SetSVHouseImage(item, houseType);

                item.Center = p;
                item.Orientation = 0;
                TestScatterView.Items.Add(item);
            }
        }

        //======================================================================
        //                       Helper Functions
        //======================================================================
        // Sets the sizes for the StackPanels and the buttons within it.
        private void InitializePanels()
        {
            Double WindowHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            Double WindowWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            // Setting sizes for StackPanels
            RightButtons.Height = WindowHeight / 10;
            //LeftButtons.Height = WindowHeight / 10;

            SetButtonSize(HouseButton, WindowHeight / 10);
            SetButtonSize(RoadButton, WindowHeight / 10);         
            //SetButtonSize(UndoButton, WindowHeight / 10);
            //SetButtonSize(RedoButton, WindowHeight / 10);
            //SetButtonSize(ClearButton, WindowHeight / 10);
        }

        // Sets the passed button's width and height to be equal to size.
        private void SetButtonSize(SurfaceButton button, Double size)
        {
            button.Height = size;
            button.Width = size;
        }

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

        private void SetButtonMode(int button)
        {
            HouseBorder.BorderThickness = new Thickness(1);
            RoadBorder.BorderThickness = new Thickness(1);

            if (button == ROAD_BUTTON)
            {
                RoadBorder.BorderThickness = new Thickness(5);
                // RoadCanvas.IsEnabled = true;
                canPlaceHouse = false;
                canPlaceRoad = true;
            }
            else if (button == HOUSE_BUTTON)
            {
                HouseBorder.BorderThickness = new Thickness(5);
                // RoadCanvas.IsEnabled = false;
                canPlaceHouse = true;
                canPlaceRoad = false;
            }
        }

        // Sets the image of the house ScatterView based on which type of house is selected
        private ScatterViewItem SetSVHouseImage(ScatterViewItem sv, String type)
        {
            ScatterViewItem item = new ScatterViewItem();
            item.CanRotate = false;
            item.CanScale = false;
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
    }
}