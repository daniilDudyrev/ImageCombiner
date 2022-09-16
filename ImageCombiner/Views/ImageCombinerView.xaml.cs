using ImageCombiner.ViewModels;
using System.Windows;

namespace ImageCombiner.Views
{
    public partial class ImageCombinerView : Window
    {
        public ImageCombinerViewModel ViewModel { get; set; }
        public ImageCombinerView()
        {
            ViewModel = new ImageCombinerViewModel();
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void countOfHorizontal_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ViewModel.SliderValueChangedHorizontal();

        private void countOfVertical_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ViewModel.SliderValueChangedVertical();
    }
}
