using Avalonia.Media;

namespace SC_App.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public IImageBrushSource Image {  get; set; }
        public ImageBrush Image { get; set; }
    }
}
