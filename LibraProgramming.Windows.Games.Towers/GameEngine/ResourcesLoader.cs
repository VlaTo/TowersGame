using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ResourcesLoader : ICreateResources, IResourceProvider<CanvasBitmap>, IResourceProvider<ICanvasBrush>
    {
        private readonly IDictionary<string, ICanvasBrush> brushes;
        private CanvasBitmap bitmap;

        CanvasBitmap IResourceProvider<CanvasBitmap>.this[string key]
        {
            get
            {
                if (null == key)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (0 == key.Length)
                {
                    throw new ArgumentException("", nameof(key));
                }

                return bitmap;
            }
        }

        ICanvasBrush IResourceProvider<ICanvasBrush>.this[string key]
        {
            get
            {
                if (null == key)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (0 == key.Length)
                {
                    throw new ArgumentException("", nameof(key));
                }

                return brushes[key];
            }
        }

        public ResourcesLoader()
        {
            brushes = new Dictionary<string, ICanvasBrush>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public async Task CreateAsync(ICanvasResourceCreatorWithDpi creator)
        {
            bitmap = await CanvasBitmap.LoadAsync(creator, new Uri("ms-appx:///Assets/Terrain.png"));

            brushes.Add("cadetblue", new CanvasSolidColorBrush(creator, Colors.CadetBlue));
            brushes.Add("green", new CanvasSolidColorBrush(creator, Colors.GreenYellow));
            brushes.Add("yellow", new CanvasSolidColorBrush(creator, Colors.Yellow));
            brushes.Add("red", new CanvasSolidColorBrush(creator, Colors.Red));
            brushes.Add("grey", new CanvasSolidColorBrush(creator, Colors.DarkGray));
            brushes.Add("white", new CanvasSolidColorBrush(creator, Colors.White));
        }
    }
}