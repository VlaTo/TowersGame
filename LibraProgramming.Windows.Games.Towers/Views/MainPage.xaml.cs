using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using LibraProgramming.Windows.Games.Towers.Events;
using LibraProgramming.Windows.Games.Towers.GameEngine;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace LibraProgramming.Windows.Games.Towers.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly TowersGame game;

        public MainPage()
        {
            game = new TowersGame();

            InitializeComponent();
        }

        private void OnCanvasCreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            //var scene = controller.ConfigureScene(new Size(sender.Width, sender.Height));
            //args.TrackAsyncAction(scene.CreateResourcesAsync(sender, args.Reason).AsAsyncAction());
        }

        private void OnCanvasDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            //controller.DrawScene(args.DrawingSession);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            game.StartApplication();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            game.Dispose();
        }

        private void OnCanvasUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            game.Update(args.Timing.ElapsedTime);
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //controller.PointerMoved(e.GetCurrentPoint(sender as UIElement));
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //controller.PointerEntered();
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            //controller.PointerExited();
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //e.Handled = controller.PointerPressed(sender as ICanvasResourceCreatorWithDpi, e.KeyModifiers, e.GetCurrentPoint(sender as UIElement));
        }
    }
}
