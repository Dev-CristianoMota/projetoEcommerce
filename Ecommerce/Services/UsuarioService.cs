using Ecommerce.Data;
using Ecommerce.DTO;
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

    public async Task<DTOUsuarioLogado?> LoginAsync(string identificador, string senha)
    {
        if (string.IsNullOrWhiteSpace(identificador) || string.IsNullOrEmpty(senha))
            return null;

        var valor = identificador.Trim();

        var usuario = await _context.Usuarios
            .AsNoTracking()
            .Include(u => u.CdTipoUsuarioNavigation)
            .FirstOrDefaultAsync(u =>
                (u.NmEmail == valor || u.NmApelido == valor) && u.IcUsuarioAtivo);

        if (usuario is null) return null;

        var resultado = _hasher.VerifyHashedPassword(usuario, usuario.NmSenha, senha);
        if (resultado == PasswordVerificationResult.Failed) return null;

        return new DTOUsuarioLogado
        {
            CdUsuario = usuario.CdUsuario,
            NmApelido = usuario.NmApelido,
            NmUsuario = usuario.NmUsuario,
            NmEmail = usuario.NmEmail,
            CdTipoUsuario = usuario.CdTipoUsuario,
            NmTipoUsuario = usuario.CdTipoUsuarioNavigation.NmTipoUsuario
        };
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