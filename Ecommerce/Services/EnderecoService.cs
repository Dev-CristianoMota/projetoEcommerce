using Ecommerce.Data;
using Ecommerce.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services
{
    public class EnderecoService
    {
        private readonly EcommerceDbContext _context;

        public EnderecoService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<DTOEndereco>> ObterEnderecosPorClienteAsync(int cdCliente)
        {
            return await _context.Enderecos
                .Where(e => e.CdUsuario == cdCliente)
                .Select(e => new DTOEndereco
                {
                    CdEndereco = e.CdEndereco,
                    NmIdentificacao = e.NmIdentificacao,
                    NmDestinatario = e.NmDestinatario,
                    CEP = e.CEP,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero,
                    DsComplemento = e.DsComplemento,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    UF = e.UF,
                    IcEnderecoPrincipal = e.IcEnderecoPrincipal
                })
                .ToListAsync();
        }

        public const int MaxEnderecos = 4;

        public async Task<(bool Sucesso, string? Erro, DTOEndereco? Endereco)> CriarEnderecoAsync(DTOEndereco enderecoDto, int cdUsuario)
        {
            if (enderecoDto == null || cdUsuario <= 0)
            {
                return (false, "Dados inválidos.", null);
            }

            var total = await _context.Enderecos.CountAsync(e => e.CdUsuario == cdUsuario);

            if (total >= MaxEnderecos)
            {
                return (false, $"Você atingiu o limite de {MaxEnderecos} endereços.", null);
            }

            var endereco = new Models.Endereco
            {
                CdUsuario = cdUsuario,
                NmIdentificacao = enderecoDto.NmIdentificacao.Trim(),
                NmDestinatario = enderecoDto.NmDestinatario.Trim(),
                CEP = new string(enderecoDto.CEP.Where(char.IsDigit).ToArray()),
                Logradouro = enderecoDto.Logradouro.Trim(),
                Numero = enderecoDto.Numero.Trim(),
                DsComplemento = string.IsNullOrWhiteSpace(enderecoDto.DsComplemento)
                    ? null
                    : enderecoDto.DsComplemento.Trim(),
                Bairro = enderecoDto.Bairro.Trim(),
                Cidade = enderecoDto.Cidade.Trim(),
                UF = enderecoDto.UF.Trim().ToUpper(),
                IcEnderecoPrincipal = total == 0,
                IcEnderecoAtivo = true,
                DtCriacao = DateTime.UtcNow
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            return (true, null, new DTOEndereco
            {
                CdEndereco = endereco.CdEndereco,
                NmIdentificacao = endereco.NmIdentificacao,
                NmDestinatario = endereco.NmDestinatario,
                CEP = endereco.CEP,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                DsComplemento = endereco.DsComplemento,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                UF = endereco.UF,
                IcEnderecoPrincipal = endereco.IcEnderecoPrincipal
            });
        }

        public async Task<DTOEndereco?> AtualizarEnderecoAsync(int cdEndereco, int cdUsuario, DTOEndereco enderecoDto)
        {
            if (enderecoDto == null || cdEndereco <= 0 || cdUsuario <= 0)
            {
                return null;
            }

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.CdEndereco == cdEndereco && e.CdUsuario == cdUsuario);

            if (endereco == null)
            {
                return null;
            }

            endereco.NmIdentificacao = enderecoDto.NmIdentificacao.Trim();
            endereco.NmDestinatario = enderecoDto.NmDestinatario.Trim();
            endereco.CEP = enderecoDto.CEP.Trim();
            endereco.Logradouro = enderecoDto.Logradouro.Trim();
            endereco.Numero = enderecoDto.Numero.Trim();
            endereco.DsComplemento = string.IsNullOrWhiteSpace(enderecoDto.DsComplemento)
                ? null
                : enderecoDto.DsComplemento.Trim();
            endereco.Bairro = enderecoDto.Bairro.Trim();
            endereco.Cidade = enderecoDto.Cidade.Trim();
            endereco.UF = enderecoDto.UF.Trim().ToUpper();
            endereco.DtAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new DTOEndereco
            {
                CdEndereco = endereco.CdEndereco,
                NmIdentificacao = endereco.NmIdentificacao,
                NmDestinatario = endereco.NmDestinatario,
                CEP = endereco.CEP,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                DsComplemento = endereco.DsComplemento,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                UF = endereco.UF,
                IcEnderecoPrincipal = endereco.IcEnderecoPrincipal
            };
        }

        public async Task<bool> DeletarEnderecoAsync(int cdEndereco, int cdUsuario)
        {
            if (cdEndereco <= 0 || cdUsuario <= 0)
            {
                return false;
            }

            var enderecosUsuario = await _context.Enderecos
                .Where(e => e.CdUsuario == cdUsuario)
                .ToListAsync();

            var endereco = enderecosUsuario.FirstOrDefault(e => e.CdEndereco == cdEndereco);

            if (endereco == null)
            {
                return false;
            }

            _context.Enderecos.Remove(endereco);

            if (endereco.IcEnderecoPrincipal)
            {
                var novoPrincipal = enderecosUsuario
                    .Where(e => e.CdEndereco != cdEndereco)
                    .OrderBy(e => e.CdEndereco)
                    .FirstOrDefault();

                if (novoPrincipal != null)
                {
                    novoPrincipal.IcEnderecoPrincipal = true;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TornarEnderecoPrincipalAsync(int cdEndereco, int cdUsuario)
        {
            if (cdEndereco <= 0 || cdUsuario <= 0)
            {
                return false;
            }

            var enderecosUsuario = await _context.Enderecos
                .Where(e => e.CdUsuario == cdUsuario)
                .ToListAsync();

            if (!enderecosUsuario.Any(e => e.CdEndereco == cdEndereco))
            {
                return false;
            }

            foreach (var e in enderecosUsuario)
            {
                e.IcEnderecoPrincipal = e.CdEndereco == cdEndereco;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
