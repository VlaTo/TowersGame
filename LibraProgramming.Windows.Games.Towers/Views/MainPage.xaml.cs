using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using LibraProgramming.Windows.Games.Towers.GameEngine;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace LibraProgramming.Windows.Games.Towers.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly GameplayController controller;

        public MainPage()
        {
            controller = ServiceLocator.Current.GetInstance<GameplayController>();
            InitializeComponent();
        }
        private void OnCanvasCreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            var scene = controller.ConfigureScene(new Size(sender.Width, sender.Height));
            args.TrackAsyncAction(scene.CreateResourcesAsync(sender, args.Reason).AsAsyncAction());
        }

        private void OnCanvasDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            controller.DrawScene(args.DrawingSession);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            controller.Initialize();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            controller.Shutdown();
        }

        private void OnCanvasUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            controller.Update(args.Timing);
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            controller.PointerMoved(e.GetCurrentPoint(sender as UIElement));
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            controller.PointerEntered();
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            controller.PointerExited();
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = controller.PointerPressed(sender as ICanvasResourceCreatorWithDpi, e.KeyModifiers, e.GetCurrentPoint(sender as UIElement));
        }
    }
}
