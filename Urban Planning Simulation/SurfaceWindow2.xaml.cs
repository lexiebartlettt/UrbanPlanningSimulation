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
    /// <summary>
    /// Interaction logic for SurfaceWindow2.xaml
    /// </summary>
    public partial class SurfaceWindow2 : SurfaceWindow
    {
        // Flags for the current mode
        private Boolean canPlaceHouse;
        private Boolean canPlaceRoad;

        // Current selected types
        private String houseType = "HouseEMI";

        private List<ScatterViewItem> redoList = new List<ScatterViewItem>();
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow2()
        {
            InitializeComponent();
            inkCanvas1.IsEnabled = false;
            inkCanvas1.Visibility = System.Windows.Visibility.Hidden;
            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            // Default to house mode
            HouseBorder.BorderThickness = new Thickness(5);
            MainPanel.ScrollToVerticalOffset(4000);
            MainPanel.ScrollToHorizontalOffset(4000);
            MainPanel.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            MainPanel.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            MainPanel.PanningMode = PanningMode.None;
            canPlaceHouse = true;

            // Initialize button panels
            InitializePanels();

        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        // When house button is clicked
        private void HouseButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas1.IsEnabled = false;
            // Change thickness to depict you are in that mode
            HouseBorder.BorderThickness = new Thickness(5);
            RoadBorder.BorderThickness = new Thickness(1);
            FreeRoamBorder.BorderThickness = new Thickness(1);
            MainPanel.PanningMode = PanningMode.None;

            // Set house flag and house type
            ElementMenuItem button = (ElementMenuItem)sender;
            canPlaceHouse = true;
            canPlaceRoad = false;
            houseType = button.Name;
        }

        // For mouse clicks
        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (canPlaceHouse)
            {
                inkCanvas1.IsEnabled = false;
                redoList = new List<ScatterViewItem>();
                e.Handled = true;
                MainPanel.UpdateLayout();
                Point mousePosition = e.GetPosition(this);
                mousePosition.X += MainPanel.HorizontalOffset;
                mousePosition.Y += MainPanel.VerticalOffset;

                // Setting the ScatterView image background
                ScatterViewItem item = new ScatterViewItem();
                item = SetSVHouseImage(item, houseType);

                item.Center = mousePosition;
                item.Orientation = 0;
                MainScatterview.Items.Add(item);
                //item.BringIntoView();
            }
            else if (canPlaceRoad)
            {
                inkCanvas1.IsEnabled = true;
            }
        }
        

        // For hold gestures
        private void gesturebox_HoldGesture(object sender, TouchEventArgs e)
        {
            if (canPlaceHouse)
            {
                redoList = new List<ScatterViewItem>();
                e.Handled = true;
                MainPanel.UpdateLayout();
                Point p = e.TouchDevice.GetPosition(this);
                p.X += MainPanel.HorizontalOffset;
                p.Y += MainPanel.VerticalOffset;

                // Setting the ScatterView image background
                ScatterViewItem item = new ScatterViewItem();
                item = SetSVHouseImage(item, houseType);

                item.Center = p;
                item.Orientation = 0;
                MainScatterview.Items.Add(item);
            }
        }

        // When tag is detected
        private void UrbanTagVisualizer_VisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            TagVisualization1 objectTag = (TagVisualization1)e.TagVisualization;
            switch (objectTag.VisualizedTag.Value)
            {
                case 0:
                    objectTag.HouseModel.Content = "HOUSE ADDED";
                    objectTag.myHouse.Fill = SurfaceColors.Accent1Brush;
                    break;
                default:
                    objectTag.HouseModel.Content = "UNKNOWN MODEL";
                    objectTag.myHouse.Fill = SurfaceColors.ControlAccentBrush;
                    break;
            }
        }

        // When road button is clicked
        private void RoadButton_Click(object sender, RoutedEventArgs e)
        {
            // Change thickness to depict you are in that mode
            HouseBorder.BorderThickness = new Thickness(1);
            RoadBorder.BorderThickness = new Thickness(5);
            FreeRoamBorder.BorderThickness = new Thickness(1);
            MainPanel.PanningMode = PanningMode.None;
            canPlaceHouse = false;
            canPlaceRoad = true;
            inkCanvas1.IsEnabled = true;
            inkCanvas1.Visibility = System.Windows.Visibility.Visible;
            //MessageBox.Show("Road");
        }

        // When free roam button is clicked
        private void FreeRoamButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas1.IsEnabled = false;
            inkCanvas1.Visibility = System.Windows.Visibility.Hidden;
            // Change thickness to depict you are in that mode
            HouseBorder.BorderThickness = new Thickness(1);
            RoadBorder.BorderThickness = new Thickness(1);
            FreeRoamBorder.BorderThickness = new Thickness(5);
            MainPanel.PanningMode = PanningMode.Both;
            canPlaceHouse = false;
            canPlaceRoad = false;
            //MessageBox.Show("Free Roam");
        }

        // When undo button is clicked
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {   int count = MainScatterview.Items.Count;
            if (count > 0)
            {
                redoList.Add((ScatterViewItem)MainScatterview.Items[count - 1]);
                MainScatterview.Items.RemoveAt(count-1);
            }
        }

        // When redo button is clicked
        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (redoList.Count > 0)
            {
                int count = redoList.Count;
                MainScatterview.Items.Add((ScatterViewItem)redoList[count-1]);
                redoList.Remove((ScatterViewItem)redoList[count-1]);
            }
            //MessageBox.Show("Redo");
        }

        // When clear button is clicked
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            redoList = new List<ScatterViewItem>();
            MainScatterview.Items.Clear();
        }

        //======================================================================
        //                          Helper Functions
        //======================================================================

        // Sets the sizes for the StackPanels and the buttons within it.
        private void InitializePanels()
        {
            Double WindowHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            Double WindowWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            // Setting sizes for StackPanels
            RightButtons.Height = WindowHeight / 10;
            LeftButtons.Height = WindowHeight / 10;

            SetButtonSize(HouseButton, WindowHeight / 10);
            SetButtonSize(RoadButton, WindowHeight / 10);
            SetButtonSize(FreeRoamButton, WindowHeight / 10);
            SetButtonSize(UndoButton, WindowHeight / 10);
            SetButtonSize(RedoButton, WindowHeight / 10);
            SetButtonSize(ClearButton, WindowHeight / 10);
        }

        // Sets the passed button's width and height to be equal to size.
        private void SetButtonSize(SurfaceButton button, Double size)
        {
            button.Height = size;
            button.Width = size;
        }

        // Sets the image of the house ScatterView based on which type of house is selected
        private ScatterViewItem SetSVHouseImage(ScatterViewItem sv, String type)
        {

            ScatterViewItem item = new ScatterViewItem();
            BitmapImage img = new BitmapImage();

            if (type.Equals("HouseEMI", StringComparison.Ordinal))
            {
                img = new BitmapImage(new Uri("Resources/iso_house_1.png", UriKind.Relative));

            } else if (type.Equals("BuildingEMI", StringComparison.Ordinal)) 
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