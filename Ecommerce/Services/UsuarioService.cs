using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
public class UsuarioService
{
    private readonly EcommerceDbContext _context;
    private readonly PasswordHasher<Usuario> _hasher = new();

    public UsuarioService(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> ListarUsuariosAsync()
    {
        return await _context.Usuarios
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Usuario?> BuscarPorIdAsync(int id)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.CdUsuario == id);
    }

    public async Task<Usuario?> LoginAsync(string email, string senha)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NmEmail == email.Trim().ToLower() && u.IcUsuarioAtivo);

        if (usuario is null) return null;

        var resultado = _hasher.VerifyHashedPassword(usuario, usuario.NmSenha, senha);
        return resultado == PasswordVerificationResult.Failed ? null : usuario;
    }

    public async Task<Usuario> CriarUsuarioAsync(Usuario usuario)
    {
        usuario.NmSenha = _hasher.HashPassword(usuario, usuario.NmSenha);
        usuario.DtCriacao = DateTime.Now;
        usuario.DtAtualizacao = DateTime.Now;
        usuario.IcUsuarioAtivo = true;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
}