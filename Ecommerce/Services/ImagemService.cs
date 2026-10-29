using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class ImagemService
{
    private readonly Cloudinary _cloudinary;
    private static readonly string[] _extPermitidas = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long TamanhoMaximo = 5 * 1024 * 1024; // 5 MB

    public ImagemService(Cloudinary cloudinary) => _cloudinary = cloudinary;

    public async Task<(string Url, string PublicId)> EnviarAsync(IFormFile arquivo, string pasta = "produtos")
    {
        if (arquivo == null || arquivo.Length == 0)
            throw new ArgumentException("Nenhum arquivo enviado.");

        if (arquivo.Length > TamanhoMaximo)
            throw new ArgumentException("A imagem deve ter no máximo 5 MB.");

        var ext = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        if (!_extPermitidas.Contains(ext))
            throw new ArgumentException("Formato inválido. Use JPG, PNG ou WEBP.");

        await using var stream = arquivo.OpenReadStream();

        var parametros = new ImageUploadParams
        {
            File = new FileDescription(arquivo.FileName, stream),
            Folder = pasta,
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false
        };

        var resultado = await _cloudinary.UploadAsync(parametros);

        if (resultado.Error != null)
            throw new InvalidOperationException($"Falha no upload: {resultado.Error.Message}");

        return (resultado.SecureUrl.ToString(), resultado.PublicId);
    }

    public async Task ExcluirAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId)) return;
        await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}

