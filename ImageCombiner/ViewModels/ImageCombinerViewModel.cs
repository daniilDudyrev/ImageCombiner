using ImageCombiner.Commands;
using ImageCombiner.Views;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageCombiner.ViewModels
{
    public class ImageCombinerViewModel
    {
        #region publicFields
        public ActionCommand SelectImageCommand => new(SelectImage);
        public ActionCommand MergeImageCommand => new(MergeImages);
        #endregion

        #region privateFields
        private List<BitmapFrame> _bitmapFrames;
        private IEnumerable<string>? _fileNames;
        private int _imageWidth;
        private int _imageHeight;
        private int _sliderValueHorizontal;
        private int _sliderValueVertical;
        #endregion
        public ImageCombinerViewModel()
        {
            _bitmapFrames = new List<BitmapFrame>();
        }

        #region private func
        private void SelectImage()
        {
            using var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = false,
                Multiselect = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                _fileNames = dialog.FileNames;
            if (_fileNames == null)
                return;
            _fileNames.ToList();
            foreach (var file in _fileNames)
            {
                _bitmapFrames.Add(BitmapDecoder.Create(new Uri($@"{file}"), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First());
                _imageWidth = BitmapDecoder.Create(new Uri($@"{file}"), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First().PixelWidth;
                _imageHeight = BitmapDecoder.Create(new Uri($@"{file}"), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First().PixelHeight;
            }
        }

        private void MergeImages()
        {
            if (_bitmapFrames.Count == 0)
            {
                MessageBox.Show("Choose Images", "Error", MessageBoxButton.OK);
                return;
            }
            if (_sliderValueHorizontal == 0 || _sliderValueVertical == 0)
            {
                MessageBox.Show("Choose the horizontal and vertical size", "Error", MessageBoxButton.OK);
                return;
            }
            var startWidth = 0;
            var startHeight = 0;
            var drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                /*foreach (var frame in _bitmapFrames)
                {
                    if (i <= _sliderValueHorizontal)
                    {
                        drawingContext.DrawImage(frame, new Rect(startWidth, 0, _imageWidth, _imageHeight));
                        startWidth += _imageWidth;
                        if (i == _sliderValueHorizontal)
                            startWidth = 0;
                    }
                    if (i > _sliderValueHorizontal && i <= _sliderValueHorizontal * 2)
                    {
                        drawingContext.DrawImage(frame, new Rect(startWidth, _imageHeight, _imageWidth, _imageHeight));
                        startWidth += _imageWidth;
                        if (i == _sliderValueHorizontal * 2)
                            startWidth = 0;
                    }
                    if (i > _sliderValueHorizontal * 2 && i <= _sliderValueHorizontal * 3)
                    {
                        drawingContext.DrawImage(frame, new Rect(startWidth, _imageHeight * 2, _imageWidth, _imageHeight));
                        startWidth += _imageWidth;
                        if (i == _sliderValueHorizontal * 3)
                            startWidth = 0;
                    }
                    if (i > _sliderValueHorizontal * 3)
                    {
                        drawingContext.DrawImage(frame, new Rect(startWidth, _imageHeight * 3, _imageWidth, _imageHeight));
                        startWidth += _imageWidth;
                        if (i == _sliderValueHorizontal * 3)
                            return;
                    }
                    i++;
                }*/

                int l;
                for (var j=0;j< _bitmapFrames.Count;)
                {
                    for(var k=0;k< _sliderValueVertical; k++)
                    {
                        for(l=0;l<_sliderValueHorizontal;l++)
                        {
                            drawingContext.DrawImage(_bitmapFrames[j], new Rect(startWidth, startHeight, _imageWidth, _imageHeight));
                            startWidth += _imageWidth;
                            j++;
                        }
                        startHeight += _imageHeight;
                        startWidth = 0;
                    }
                }
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap(_imageWidth * _sliderValueHorizontal, _imageHeight * _sliderValueHorizontal, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using Stream stream = File.Create("final.png");
            encoder.Save(stream);
            MessageBox.Show("Your file has been saved");
        }
        #endregion
        public void SliderValueChangedHorizontal()
        {
            var window = Application.Current.Windows.OfType<ImageCombinerView>().Single();
            _sliderValueHorizontal = (int)window.countOfHorizontal.Value;
            window.textBlockHorizontal.Text = _sliderValueHorizontal.ToString();
        }
        public void SliderValueChangedVertical()
        {
            var window = Application.Current.Windows.OfType<ImageCombinerView>().Single();
            _sliderValueVertical = (int)window.countOfVertical.Value;
            window.textBlockVertical.Text = _sliderValueVertical.ToString();
        }
    }
}
