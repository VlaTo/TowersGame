using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICreateResources
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        Task CreateAsync(ICanvasResourceCreatorWithDpi creator);
    }
}