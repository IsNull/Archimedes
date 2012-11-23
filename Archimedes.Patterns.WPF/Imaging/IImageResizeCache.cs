using System;
namespace Archimedes.Patterns.WPF.Imaging
{
    public interface ISizedImageCache<T>
        where T : System.Windows.Media.ImageSource
    {
        void Clear();
        bool Contains(object imageID, System.Windows.Size size);
        T Retrive(object imageID, System.Windows.Size size);
        void Store(object imageID, System.Windows.Size size, T imgsrc);
    }
}
