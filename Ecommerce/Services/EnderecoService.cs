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

        public async Task<DTOEndereco> CriarEnderecoAsync(DTOEndereco enderecoDto, int cdUsuario)
        {
            if (cdUsuario <= 0)
            {
                return null;
            }

            var endereco = new Models.Endereco
            {
                CdUsuario = cdUsuario,
                NmIdentificacao = enderecoDto.NmIdentificacao,
                NmDestinatario = enderecoDto.NmDestinatario,
                CEP = enderecoDto.CEP,
                Logradouro = enderecoDto.Logradouro,
                Numero = enderecoDto.Numero,
                DsComplemento = enderecoDto.DsComplemento,
                Bairro = enderecoDto.Bairro,
                Cidade = enderecoDto.Cidade,
                UF = enderecoDto.UF,
                IcEnderecoPrincipal = enderecoDto.IcEnderecoPrincipal,
                DtCriacao = DateTime.UtcNow
            };

            _context.Enderecos.Add(endereco);
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

        public async Task<DTOEndereco?> AtualizarEnderecoAsync(int cdEndereco, DTOEndereco enderecoDto)
        {
            if (enderecoDto == null)
            {
                return null;
            }

            if (cdEndereco <= 0)
            {
                return null;
            }

            var endereco = await _context.Enderecos.FindAsync(cdEndereco);
            if (endereco == null)
            {
                return null;
            }
            endereco.NmIdentificacao = enderecoDto.NmIdentificacao;
            endereco.NmDestinatario = enderecoDto.NmDestinatario;
            endereco.CEP = enderecoDto.CEP;
            endereco.Logradouro = enderecoDto.Logradouro;
            endereco.Numero = enderecoDto.Numero;
            endereco.DsComplemento = enderecoDto.DsComplemento;
            endereco.Bairro = enderecoDto.Bairro;
            endereco.Cidade = enderecoDto.Cidade;
            endereco.UF = enderecoDto.UF;
            endereco.IcEnderecoPrincipal = enderecoDto.IcEnderecoPrincipal;
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

        public async Task<bool> DeletarEnderecoAsync(int cdEndereco)
        {
            if (cdEndereco <= 0)
            {
                return false;
            }

            var endereco = await _context.Enderecos.FindAsync(cdEndereco);
            if (endereco == null)
            {
                return false;
            }
            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
