using child13.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Brushes = System.Windows.Media.Brushes;
using PointWPF = System.Windows.Point;

namespace child13.Views
{
    /// <summary>
    /// Interaction logic for CoordinateRecorder.xaml
    /// </summary>
    public partial class CoordinateRecorder : Window
    {
        private Coordinate _coordinate;
        private PointWPF _startPoint;
        private bool _isResizing;
        private bool _isDragging;

        public CoordinateRecorder(Coordinate coordinate)
        {
            InitializeComponent();
            _coordinate = coordinate;
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;
            this.MouseMove += Window_MouseMove;
            this.MouseLeftButtonUp += Window_MouseLeftButtonUp;
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            Background = Brushes.Transparent;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SelectionRectangle.Width = Width;
            SelectionRectangle.Height = Height;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == SelectionRectangle)
            {
                _isResizing = true;
                _startPoint = e.GetPosition(SelectionCanvas);
                Mouse.Capture(SelectionRectangle);
            }
            else
            {
                _isDragging = true;
                _startPoint = e.GetPosition(this);
                Mouse.Capture(this);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isResizing)
            {
                var currentPoint = e.GetPosition(SelectionCanvas);
                var x = Math.Min(currentPoint.X, _startPoint.X);
                var y = Math.Min(currentPoint.Y, _startPoint.Y);
                var width = Math.Abs(currentPoint.X - _startPoint.X);
                var height = Math.Abs(currentPoint.Y - _startPoint.Y);

                SelectionRectangle.Visibility = Visibility.Visible;
                Canvas.SetLeft(SelectionRectangle, x);
                Canvas.SetTop(SelectionRectangle, y);
                SelectionRectangle.Width = width;
                SelectionRectangle.Height = height;
            }
            else if (_isDragging)
            {
                var currentPoint = e.GetPosition(this);
                Left += currentPoint.X - _startPoint.X;
                Top += currentPoint.Y - _startPoint.Y;
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isResizing)
            {
                _isResizing = false;
                Mouse.Capture(null);
            }
            else if (_isDragging)
            {
                _isDragging = false;
                Mouse.Capture(null);
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionRectangle.Visibility == Visibility.Visible)
            {
                double x1 = Canvas.GetLeft(SelectionRectangle);
                double y1 = Canvas.GetTop(SelectionRectangle);
                double x2 = x1 + SelectionRectangle.Width;
                double y2 = y1 + SelectionRectangle.Height;

                _coordinate._SetCoordinate((int)x1, (int)x2, (int)y1, (int)y2);
            }

            Close();
        }
    }
}

