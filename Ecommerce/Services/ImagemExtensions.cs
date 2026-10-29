namespace Ecommerce.Services;
public static class ImagemExtensions
{
    public static string? Miniatura(this string? url, int largura = 300, int altura = 300)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;
        return url.Replace("/upload/", $"/upload/w_{largura},h_{altura},c_fill,q_auto,f_auto/");
    }
}