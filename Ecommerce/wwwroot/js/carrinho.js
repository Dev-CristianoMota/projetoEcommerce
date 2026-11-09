const carrinhoDrawer = document.getElementById('carrinhoSidebar');
const carrinhoOverlay = document.getElementById('carrinhoOverlay');
const carrinhoConteudo = document.getElementById('carrinhoConteudo');
const carrinhoPagina = document.getElementById('carrinhoPagina'); // só existe em /Carrinho

let carrinhoAlterado = false;

function abrirCarrinho() {
    carrinhoDrawer.classList.add('is-open');
    carrinhoOverlay.classList.add('is-open');
    carrinhoDrawer.setAttribute('aria-hidden', 'false');
    document.body.classList.add('cart-open');
}

function fecharCarrinho() {
    carrinhoDrawer.classList.remove('is-open');
    carrinhoOverlay.classList.remove('is-open');
    carrinhoDrawer.setAttribute('aria-hidden', 'true');
    document.body.classList.remove('cart-open');

    // Estando na página do carrinho, atualiza a lista se algo mudou pela sidebar
    if (carrinhoPagina && carrinhoAlterado) location.reload();
}

async function chamarCarrinho(acao, dados) {
    const returnUrl = location.pathname + location.search;
    let resposta;

    if (acao === 'Sidebar') {
        resposta = await fetch(`/Carrinho/Sidebar?returnUrl=${encodeURIComponent(returnUrl)}`);
    } else {
        carrinhoAlterado = true;
        dados.append('returnUrl', returnUrl);
        resposta = await fetch(`/Carrinho/${acao}`, { method: 'POST', body: dados });
    }

    carrinhoConteudo.innerHTML = await resposta.text();
    abrirCarrinho();
}

async function atualizarPagina(acao, dados) {
    dados.append('origem', 'pagina');
    dados.append('returnUrl', location.pathname + location.search);

    const resposta = await fetch(`/Carrinho/${acao}`, { method: 'POST', body: dados });
    if (resposta.ok && carrinhoPagina) {
        carrinhoPagina.innerHTML = await resposta.text();
    }
}

document.addEventListener('click', (e) => {
    if (e.target.closest('[data-carrinho-fechar]') || e.target === carrinhoOverlay) {
        fecharCarrinho();
        return;
    }

    const botao = e.target.closest('[data-carrinho-acao]');
    if (!botao || botao.disabled) return;

    const dados = new FormData();
    dados.append('cdProduto', botao.dataset.cdProduto ?? 0);
    if (botao.dataset.quantidade) dados.append('quantidade', botao.dataset.quantidade);

    if (botao.dataset.carrinhoOrigem === 'pagina') {
        atualizarPagina(botao.dataset.carrinhoAcao, dados);
    } else {
        chamarCarrinho(botao.dataset.carrinhoAcao, dados);
    }
});

document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') fecharCarrinho();
});

    document.addEventListener('submit', async (e) => {
    const form = e.target.closest('[data-cupom-form]');
    if (!form) return;

    e.preventDefault();

    const resposta = await fetch('/Carrinho/AplicarCupom', {
        method: 'POST',
        body: new FormData(form)
    });

    if (resposta.ok && carrinhoPagina) {
        carrinhoPagina.innerHTML = await resposta.text();
    }
});