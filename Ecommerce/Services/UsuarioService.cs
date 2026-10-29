using Ecommerce.Data;
using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
public record ResultadoEdicao(bool Sucesso, string? Erro = null, DTOUsuarioLogado? Usuario = null);

public class UsuarioService
{
    private readonly EcommerceDbContext _context;
    private readonly PasswordHasher<Usuario> _hasher = new();
    private const int TipoVendedor = 1;   // cd_tipo_usuario = 1 ('Vendedor')
    private const int TipoComprador = 2;  // cd_tipo_usuario = 2 ('Comprador')

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

    public async Task<DadosUsuarioViewModel?> ListarDadosAsync(int cdUsuario)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .Where(u => u.CdUsuario == cdUsuario && u.IcUsuarioAtivo)
            .Select(u => new DadosUsuarioViewModel
            {
                NmUsuario = u.NmUsuario,
                NmApelido = u.NmApelido,
                NmEmail = u.NmEmail,
                NmTelefone = u.NmTelefone,
                NmTipoUsuario = u.CdTipoUsuarioNavigation.NmTipoUsuario,
                DtCriacao = u.DtCriacao
            })
            .FirstOrDefaultAsync();
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

    public async Task<ResultadoEdicao> CadastrarAsync(CadastroViewModel dados)
    {
        // Segurança: o cadastro público só aceita Vendedor ou Comprador (nunca Admin),
        // mesmo que alguém altere o valor do formulário no navegador.
        if (dados.CdTipoUsuario is not (TipoVendedor or TipoComprador))
            return new ResultadoEdicao(false, "Tipo de conta inválido.");

        var apelido = dados.NmApelido.Trim();
        var nome = dados.NmUsuario.Trim();
        var email = dados.NmEmail.Trim();
        var telefone = new string(dados.NmTelefone.Where(char.IsDigit).ToArray());

        if (telefone.Length is < 10 or > 11)
            return new ResultadoEdicao(false, "Telefone deve ter DDD + número (10 ou 11 dígitos).");

        if (await _context.Usuarios.AnyAsync(u => u.NmApelido == apelido))
            return new ResultadoEdicao(false, "Este apelido já está em uso.");

        if (await _context.Usuarios.AnyAsync(u => u.NmEmail == email))
            return new ResultadoEdicao(false, "Este e-mail já está em uso.");

        if (await _context.Usuarios.AnyAsync(u => u.NmTelefone == telefone))
            return new ResultadoEdicao(false, "Este telefone já está em uso.");

        var novo = await CriarUsuarioAsync(new Usuario
        {
            NmApelido = apelido,
            NmUsuario = nome,
            NmEmail = email,
            NmTelefone = telefone,
            NmSenha = dados.Senha,
            CdTipoUsuario = dados.CdTipoUsuario
        });

        var criado = await _context.Usuarios
            .AsNoTracking()
            .Include(u => u.CdTipoUsuarioNavigation)
            .FirstAsync(u => u.CdUsuario == novo.CdUsuario);

        return new ResultadoEdicao(true, null, new DTOUsuarioLogado
        {
            CdUsuario = criado.CdUsuario,
            NmApelido = criado.NmApelido,
            NmUsuario = criado.NmUsuario,
            NmEmail = criado.NmEmail,
            CdTipoUsuario = criado.CdTipoUsuario,
            NmTipoUsuario = criado.CdTipoUsuarioNavigation.NmTipoUsuario
        });
    }

    public async Task<ResultadoEdicao> EditarAsync(int cdUsuario, DadosUsuarioViewModel dados)
    {
        if (dados is null)
            return new ResultadoEdicao(false, "Dados inválidos.");

        var usuario = await _context.Usuarios
            .Include(u => u.CdTipoUsuarioNavigation)
            .FirstOrDefaultAsync(u => u.CdUsuario == cdUsuario && u.IcUsuarioAtivo);

        if (usuario is null)
            return new ResultadoEdicao(false, "Usuário não encontrado.");

        var apelido = dados.NmApelido?.Trim() ?? "";
        var nome = dados.NmUsuario?.Trim() ?? "";
        var email = dados.NmEmail?.Trim() ?? "";
        var telefone = new string((dados.NmTelefone ?? "").Where(char.IsDigit).ToArray());

        if (apelido.Length == 0 || nome.Length == 0 || email.Length == 0)
            return new ResultadoEdicao(false, "Preencha apelido, nome e e-mail.");

        if (telefone.Length is < 10 or > 11)
            return new ResultadoEdicao(false, "Telefone deve ter DDD + número (10 ou 11 dígitos).");

        if (await _context.Usuarios.AnyAsync(u => u.CdUsuario != cdUsuario && u.NmApelido == apelido))
            return new ResultadoEdicao(false, "Este apelido já está em uso.");

        if (await _context.Usuarios.AnyAsync(u => u.CdUsuario != cdUsuario && u.NmEmail == email))
            return new ResultadoEdicao(false, "Este e-mail já está em uso.");

        if (await _context.Usuarios.AnyAsync(u => u.CdUsuario != cdUsuario && u.NmTelefone == telefone))
            return new ResultadoEdicao(false, "Este telefone já está em uso.");

        usuario.NmApelido = apelido;
        usuario.NmUsuario = nome;
        usuario.NmEmail = email;
        usuario.NmTelefone = telefone;
        usuario.DtAtualizacao = DateTime.Now;

        await _context.SaveChangesAsync();

        return new ResultadoEdicao(true, null, new DTOUsuarioLogado
        {
            CdUsuario = usuario.CdUsuario,
            NmApelido = usuario.NmApelido,
            NmUsuario = usuario.NmUsuario,
            NmEmail = usuario.NmEmail,
            CdTipoUsuario = usuario.CdTipoUsuario,
            NmTipoUsuario = usuario.CdTipoUsuarioNavigation.NmTipoUsuario
        });
    }
}