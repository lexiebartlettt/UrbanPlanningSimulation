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
using System.Windows.Ink;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace Urban_Planning_Simulation
{
    /// Interaction logic for SurfaceWindow2.xaml
    public partial class SurfaceWindow2 : SurfaceWindow
    {
        // Constants for button type
        int ROAD_BUTTON = 1;
        int HOUSE_BUTTON = 2;
        int FREE_BUTTON = 3;

        // Flags for the current mode
        private Boolean canPlaceHouse;
        private Boolean canPlaceRoad;

        // Set default mode type
        private String houseType = "HouseEMI";

        // List for handling undo/redo
        private List<Object> redoList = new List<Object>();

        public SurfaceWindow2()
        {
            // Initialize the layout
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            // Start initial position and options for ScrollViewer
            MainPanel.ScrollToVerticalOffset(4000);
            MainPanel.ScrollToHorizontalOffset(4000);
            MainPanel.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            MainPanel.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            MainPanel.PanningMode = PanningMode.None;

            // Default to house mode
            HouseBorder.BorderThickness = new Thickness(5);
            canPlaceHouse = true;
            RoadCanvas.IsEnabled = false;

            // Initialize button panels
            InitializePanels();

            // Initialize InkCanvas Settings
            InitializeInkCanvas();

        }

        /// Occurs when the window is about to close. 
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// Adds handlers for window availability events.
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// Removes handlers for window availability events.
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// This is called when the user can interact with the application's window.
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// This is called when the user can see but not interact with the application's window.
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        //======================================================================
        //                       Canvas Functions
        //======================================================================

        // Handles event for completion of stroke on InkCanvas
        private void InkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            MainScatterview.Items.Add(e.Stroke);
        }

        // For mouse clicks
        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (canPlaceHouse)
            {
                RoadCanvas.IsEnabled = false;
                redoList = new List<Object>();
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
            }
            else if (canPlaceRoad)
            {
                RoadCanvas.IsEnabled = true;
            }
        }
        

        // For hold gestures
        private void gesturebox_HoldGesture(object sender, TouchEventArgs e)
        {
            if (canPlaceHouse)
            {
                redoList = new List<Object>();
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

        //======================================================================
        //                       TagVisualizer Functions
        //======================================================================

        // When tag is detected
        private void UrbanTagVisualizer_VisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            TagVisualization1 objectTag = (TagVisualization1)e.TagVisualization;
            switch (objectTag.VisualizedTag.Value)
            {
                case 0:
                    /*
                    ImageBrush imgBrush = new ImageBrush();
                    ScatterViewItem item = new ScatterViewItem();
                    imgBrush.ImageSource = new BitmapImage(new Uri("Resources/iso_house_1.png", UriKind.Relative));
                    item.Background = imgBrush;

                    Uri uri = new Uri("Resources/iso_house_1.png", UriKind.Relative);
                    BitmapImage source = new BitmapImage();
                    source.BeginInit();
                    source.UriSource = uri;
                    source.DecodePixelHeight = (int)item.ActualHeight;
                    source.DecodePixelWidth = (int)item.ActualWidth;
                    source.EndInit();
                    //ScatterViewItem item = new ScatterViewItem();
                    //BitmapImage img = new BitmapImage();
                    //img = new BitmapImage(new Uri("Resources/iso_house_1.png", UriKind.Relative));

                    objectTag.structure.Source = source;*/
                    objectTag.HouseModel.Content = "HOUSE ADDED";
                    objectTag.myHouse.Fill = SurfaceColors.Accent1Brush;
                    break;
                case 1:
                    objectTag.HouseModel.Content = "BUILDING ADDED";
                    objectTag.myHouse.Fill = SurfaceColors.Accent1Brush;
                    break;
                case 2:
                    objectTag.HouseModel.Content = "SKYSCRAPER ADDED";
                    objectTag.myHouse.Fill = SurfaceColors.Accent1Brush;
                    break;
                default:
                    objectTag.HouseModel.Content = "UNKNOWN MODEL";
                    objectTag.myHouse.Fill = SurfaceColors.Accent1Brush;
                    break;
            }
        }

        // When tag is removed, make scatterviewitem from last location
        private void UrbanTagVisualizer_VisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            TagVisualization1 objectTag = (TagVisualization1)e.TagVisualization;
            switch (objectTag.VisualizedTag.Value)
            {
                // house
                case 0:
                    Point p = objectTag.Center;
                    ScatterViewItem item = new ScatterViewItem();

                    item = SetSVHouseImage(item, "HouseEMI");
                    item.Center = p;
                    item.Orientation = 0;
                    MainScatterview.Items.Add(item);
                    break;
                // building
                case 1:
                    p = objectTag.Center;
                    item = new ScatterViewItem();

                    item = SetSVHouseImage(item, "BuildingEMI");
                    item.Center = p;
                    item.Orientation = 0;
                    MainScatterview.Items.Add(item);
                    break;
                // skyscraper
                case 2:
                    p = objectTag.Center;
                    item = new ScatterViewItem();

                    item = SetSVHouseImage(item, "SkyscraperEMI");
                    item.Center = p;
                    item.Orientation = 0;
                    MainScatterview.Items.Add(item);
                    break;
                default:
                    break;
            }
        }

        //======================================================================
        //                       Button Click Functions
        //======================================================================

        // When house button is clicked
        private void HouseButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonMode(HOUSE_BUTTON);

            // Set house flag and house type
            ElementMenuItem button = (ElementMenuItem) sender;
            houseType = button.Name;
        }

        // When road button is clicked
        private void RoadButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonMode(ROAD_BUTTON);
        }

        // When free roam button is clicked
        private void FreeRoamButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonMode(FREE_BUTTON);
        }

        // When undo button is clicked
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            int count = MainScatterview.Items.Count;

            if (count > 0)
            {
                Object mostRecentItem = MainScatterview.Items[count - 1];
                if (mostRecentItem.GetType() == typeof(Stroke)) 
                {
                    redoList.Add((Stroke) mostRecentItem);
                    RoadCanvas.Strokes.Remove((Stroke) mostRecentItem);
                } else if (mostRecentItem.GetType() == typeof(ScatterViewItem)) {
                    redoList.Add((ScatterViewItem) mostRecentItem);
                }
                MainScatterview.Items.RemoveAt(count-1);
            }
        }

        // When redo button is clicked
        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            int count = redoList.Count;
            if (count > 0)
            {
                Object mostRecentItem = redoList[count - 1];
                if (mostRecentItem.GetType() == typeof(Stroke)) 
                {
                    redoList.Remove((Stroke) mostRecentItem);
                    RoadCanvas.Strokes.Add((Stroke) mostRecentItem);
                    MainScatterview.Items.Add((Stroke) mostRecentItem);
                } else if (mostRecentItem.GetType() == typeof(ScatterViewItem)) {
                    redoList.Remove((ScatterViewItem) mostRecentItem);
                    MainScatterview.Items.Add((ScatterViewItem) mostRecentItem);
                }
            }
        }

        // When clear button is clicked
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            redoList = new List<Object>();
            MainScatterview.Items.Clear();
            RoadCanvas.Strokes.Clear();
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

        // Sets the settings for the road InkCanvas
        private void InitializeInkCanvas()
        {
            RoadCanvas.DefaultDrawingAttributes.Color = Colors.DarkGray;
            RoadCanvas.DefaultDrawingAttributes.FitToCurve = true;
            RoadCanvas.DefaultDrawingAttributes.Height = 30;
            RoadCanvas.DefaultDrawingAttributes.Width = 15;
            RoadCanvas.UsesTouchShape = false;
        }

        // Sets the passed button's width and height to be equal to size.
        private void SetButtonSize(SurfaceButton button, Double size)
        {
            button.Height = size;
            button.Width = size;
        }

        private void SetButtonMode(int button)
        {
            HouseBorder.BorderThickness = new Thickness(1);
            RoadBorder.BorderThickness = new Thickness(1);
            FreeRoamBorder.BorderThickness = new Thickness(1);
            MainPanel.PanningMode = PanningMode.Both;

            if (button == ROAD_BUTTON)
            {
                RoadBorder.BorderThickness = new Thickness(5);
                RoadCanvas.IsEnabled = true;
                canPlaceHouse = false;
                canPlaceRoad = true;
            } else if (button == HOUSE_BUTTON) {
                HouseBorder.BorderThickness = new Thickness(5);
                RoadCanvas.IsEnabled = false;
                canPlaceHouse = true;
                canPlaceRoad = false;
            } else if (button == FREE_BUTTON) {
                FreeRoamBorder.BorderThickness = new Thickness(5);
                RoadCanvas.IsEnabled = false;
                canPlaceHouse = false;
                canPlaceRoad = false;
            }

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