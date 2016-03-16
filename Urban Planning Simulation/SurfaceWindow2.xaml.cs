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

        static int DEFAULT_HOUSE = 1;

        private Boolean canPlaceHouse;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow2()
        {
            InitializeComponent();

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
            // Change thickness to depict you are in that mode
            HouseBorder.BorderThickness = new Thickness(5);
            RoadBorder.BorderThickness = new Thickness(1);
            FreeRoamBorder.BorderThickness = new Thickness(1);
            MainPanel.PanningMode = PanningMode.None;
            canPlaceHouse = true;
            //MessageBox.Show("House");
           
        }

        // For mouse clicks
        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (canPlaceHouse)
            {
                e.Handled = true;
                MainPanel.UpdateLayout();
                Point mousePosition = e.GetPosition(this);
                mousePosition.X += MainPanel.HorizontalOffset;
                mousePosition.Y += MainPanel.VerticalOffset;

                // Setting the ScatterView image background
                ScatterViewItem item = new ScatterViewItem();
                item = SetSVHouseImage(DEFAULT_HOUSE);

                item.Center = mousePosition;
                item.Orientation = 0;
                MainScatterview.Items.Add(item);
                //item.BringIntoView();
            }
        }

        // For hold gestures
        private void gesturebox_HoldGesture(object sender, TouchEventArgs e)
        {
            e.Handled = true;
            MainPanel.UpdateLayout();
            Point p = e.TouchDevice.GetPosition(this);
            p.X += MainPanel.HorizontalOffset;
            p.Y += MainPanel.VerticalOffset;

            // Setting the ScatterView image background
            ScatterViewItem item = new ScatterViewItem();
            item = SetSVHouseImage(DEFAULT_HOUSE);

            item.Center = p;
            item.Orientation = 0;
            MainScatterview.Items.Add(item);
            //item.BringIntoView();
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
            //MessageBox.Show("Road");
        }

        // When free roam button is clicked
        private void FreeRoamButton_Click(object sender, RoutedEventArgs e)
        {
            // Change thickness to depict you are in that mode
            HouseBorder.BorderThickness = new Thickness(1);
            RoadBorder.BorderThickness = new Thickness(1);
            FreeRoamBorder.BorderThickness = new Thickness(5);
            MainPanel.PanningMode = PanningMode.Both;
            canPlaceHouse = false;
            //MessageBox.Show("Free Roam");
        }

        // When undo button is clicked
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {   int count = MainScatterview.Items.Count;
            if (count > 0)
            {
                MainScatterview.Items.RemoveAt(count-1);
            }
        }

        // When redo button is clicked
        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Redo");
        }

        // When clear button is clicked
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
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
        private ScatterViewItem SetSVHouseImage(int type)
        {
            ScatterViewItem item = new ScatterViewItem();

            if (type == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("Resources/iso_house_1.png", UriKind.Relative));
                ImageBrush imgBrush = new ImageBrush();
                imgBrush.ImageSource = img;
                item.Background = imgBrush;
            }

            return item;
        }
        
    }
}