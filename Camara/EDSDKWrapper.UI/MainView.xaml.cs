
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

//using System.Windows.Forms;
//using System.Windows.Forms.Integration;

namespace EDSDKWrapper.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainView : Window
    {
        public System.Windows.Shapes.Rectangle rectSelectArea;

        public MainView()
        {
            //          Loaded += MainView_Loaded;
            InitializeComponent();
            rectSelectArea = new System.Windows.Shapes.Rectangle
            {
                Stroke = System.Windows.Media.Brushes.Lime,
                StrokeThickness = 1,
                Height = 600,
                Width = 510,
            };
            Properties.Settings.Default["ratio"] = "-600_90_510_600";
            //prueba = new System.Windows.Shapes.Rectangle {
            //    Stroke = System.Windows.Media.Brushes.Azure,
            //    StrokeThickness = 1,
            //    Height = 600,
            //    Width = 400,
            //}
            //;
            //cnvImage.Children.Add(prueba);
            Canvas.SetLeft(rectSelectArea, -600);
            Canvas.SetTop(rectSelectArea, 90);//90
            cnvImage.Children.Add(rectSelectArea);
        }
        private enum HitType
        {
            None, Body, UL, UR, LR, LL, L, R, T, B
        };

        // True if a drag is in progress.
        private bool DragInProgress = false;

        // The drag's last point.
        private System.Windows.Point LastPoint;

        // The part of the rectangle under the mouse.
        HitType MouseHitType = HitType.None;

        // Return a HitType value to indicate what is at the point.
        private HitType SetHitType(System.Windows.Shapes.Rectangle rect, System.Windows.Point point)
        {
            double left = Canvas.GetLeft(rectSelectArea);
            double top = Canvas.GetTop(rectSelectArea);
            double right = left + rectSelectArea.Width;
            double bottom = top + rectSelectArea.Height;
            if (point.X < left) return HitType.None;
            if (point.X > right) return HitType.None;
            if (point.Y < top) return HitType.None;
            if (point.Y > bottom) return HitType.None;

            const double GAP = 10;
            if (point.X - left < GAP)
            {
                // Left edge.
                if (point.Y - top < GAP) return HitType.UL;
                if (bottom - point.Y < GAP) return HitType.LL;
                return HitType.L;
            }
            if (right - point.X < GAP)
            {
                // Right edge.
                if (point.Y - top < GAP) return HitType.UR;
                if (bottom - point.Y < GAP) return HitType.LR;
                return HitType.R;
            }
            if (point.Y - top < GAP) return HitType.T;
            if (bottom - point.Y < GAP) return HitType.B;
            return HitType.Body;
        }

        // Set a mouse cursor appropriate for the current hit type.
        private void SetMouseCursor()
        {
            // See what cursor we should display.
            Cursor desired_cursor = Cursors.Arrow;
            switch (MouseHitType)
            {
                case HitType.None:
                    desired_cursor = Cursors.Arrow;
                    break;
                case HitType.Body:
                    desired_cursor = Cursors.ScrollAll;
                    break;
                case HitType.UL:
                case HitType.LR:
                    desired_cursor = Cursors.SizeNWSE;
                    break;
                case HitType.LL:
                case HitType.UR:
                    desired_cursor = Cursors.SizeNESW;
                    break;
                case HitType.T:
                case HitType.B:
                    desired_cursor = Cursors.SizeNS;
                    break;
                case HitType.L:
                case HitType.R:
                    desired_cursor = Cursors.SizeWE;
                    break;
            }

            // Display the desired cursor.
            if (Cursor != desired_cursor) Cursor = desired_cursor;
        }



        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseHitType = SetHitType(rectSelectArea, Mouse.GetPosition(cnvImage));
            //  SetMouseCursor();
            if (MouseHitType == HitType.None) return;

            LastPoint = Mouse.GetPosition(cnvImage);
            DragInProgress = true;
        }

        // If a drag is in progress, continue the drag.
        // Otherwise display the correct cursor.
        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!DragInProgress)
            {
                MouseHitType = SetHitType(rectSelectArea, Mouse.GetPosition(cnvImage));
                // SetMouseCursor();
            }
            else
            {

                // See how much the mouse has moved.
                System.Windows.Point point = Mouse.GetPosition(cnvImage);
                double offset_x = point.X - LastPoint.X;
                double offset_y = point.Y - LastPoint.Y;

                // Get the rectangle's current position.
                double new_x = Canvas.GetLeft(rectSelectArea);
                double new_y = Canvas.GetTop(rectSelectArea);
                double new_width = rectSelectArea.Width;
                double new_height = rectSelectArea.Height;
                if (new_y > 88)
                {
                    new_y = 88;
                }
                else if (new_y < 3)
                {
                    new_y = 3;
                }
                if (new_x > 538)
                {
                    new_x = 538;
                }
                else if (new_x < 10)
                {
                    new_x = 10;
                }



                // Update the rectangle.
                switch (MouseHitType)
                {
                    case HitType.Body:
                        new_x += offset_x;
                        new_y += offset_y;
                        break;
                    case HitType.UL:
                        new_x += offset_x;
                        new_y += offset_y;
                        new_width -= offset_x;
                        new_height -= offset_y;
                        break;
                    case HitType.UR:
                        new_y += offset_y;
                        new_width += offset_x;
                        new_height -= offset_y;
                        break;
                    case HitType.LR:
                        new_width += offset_x;
                        new_height += offset_y;
                        break;
                    case HitType.LL:
                        new_x += offset_x;
                        new_width -= offset_x;
                        new_height += offset_y;
                        break;
                    case HitType.L:
                        new_x += offset_x;
                        new_width -= offset_x;
                        break;
                    case HitType.R:
                        new_width += offset_x;
                        break;
                    case HitType.B:
                        new_height += offset_y;
                        break;
                    case HitType.T:
                        new_y += offset_y;
                        new_height -= offset_y;
                        break;
                }

                // Don't use negative width or height.
                if ((new_width > 0) && (new_height > 0))
                {
                    // Update the rectangle.
                    Canvas.SetLeft(rectSelectArea, new_x);
                    Canvas.SetTop(rectSelectArea, new_y);
                    //rectSelectArea.Width = new_width;
                    //rectSelectArea.Height = new_height;
                    Properties.Settings.Default["ratio"] = new_x.ToString() + "_" + new_y.ToString() + "_" + new_width + "_" + new_height;
                    // Save the mouse's new location.
                    LastPoint = point;
                }
            }
        }

        // Stop dragging.
        private void canvas1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DragInProgress = false;
        }

        private void btncerrar_Click(object sender, RoutedEventArgs e)
        {
            string RutaApp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var view = Window.GetWindow(this);
            if (File.Exists(System.IO.Path.Combine(RutaApp, "myImage.Jpeg")))
            {
                File.Delete(System.IO.Path.Combine(RutaApp, "myImage.Jpeg"));
            }
            view.Close();
        }

        private void inicia_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
