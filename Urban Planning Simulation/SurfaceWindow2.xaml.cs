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
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow2()
        {
            InitializeComponent();

            // Initialize Variables
            Double WindowHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            Double WindowWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            // Default to house mode
            HouseBorder.BorderThickness = new Thickness(5);

            // Setting sizes for StackPanels
            RightButtons.Height = WindowHeight / 10;
            LeftButtons.Height = WindowHeight / 10;

            // Setting Button Sizes
            SetButtonSize(HouseButton, WindowHeight / 10);
            SetButtonSize(RoadButton, WindowHeight / 10);
            SetButtonSize(FreeRoamButton, WindowHeight / 10);
            SetButtonSize(UndoButton, WindowHeight / 10);
            SetButtonSize(RedoButton, WindowHeight / 10);
            SetButtonSize(ClearButton, WindowHeight / 10);

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
            //MessageBox.Show("House");
           
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition(this);
            ScatterViewItem item = new ScatterViewItem();
            item.Center = mousePosition;
            MainScatterview.Items.Add(item);
            item.BringIntoView();
        }



        // When road button is clicked
        private void RoadButton_Click(object sender, RoutedEventArgs e)
        {
            // Change thickness to depict you are in that mode
            HouseBorder.BorderThickness = new Thickness(1);
            RoadBorder.BorderThickness = new Thickness(5);
            FreeRoamBorder.BorderThickness = new Thickness(1);
            //MessageBox.Show("Road");
        }

        // When free roam button is clicked
        private void FreeRoamButton_Click(object sender, RoutedEventArgs e)
        {
            // Change thickness to depict you are in that mode
            HouseBorder.BorderThickness = new Thickness(1);
            RoadBorder.BorderThickness = new Thickness(1);
            FreeRoamBorder.BorderThickness = new Thickness(5);
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

        // Sets the passed button's width and height to be equal to size
        private void SetButtonSize(SurfaceButton button, Double size)
        {
            button.Height = size;
            button.Width = size;
        }
    }
}