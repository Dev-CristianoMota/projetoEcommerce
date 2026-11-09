document.addEventListener("DOMContentLoaded", function () {

    const form      = document.getElementById("feedForm");
    const grid      = document.getElementById("feedGrid");
    const sentinela = document.getElementById("feedSentinela");
    const loader    = document.getElementById("feedLoader");
    const erro      = document.getElementById("feedErro");
    const fim       = document.getElementById("feedFim");
    const vazio     = document.getElementById("feedVazio");
    const totalEl   = document.getElementById("feedTotal");
    const totalTxt  = document.getElementById("feedTotalTexto");
    const ordem     = document.getElementById("feedOrdem");
    const filtros   = document.getElementById("feedFilters");
    const btnFiltros = document.getElementById("btnFiltros");

    const URL_FEED = location.pathname;

    let pagina = 1;                                   // última página já carregada
    let temMais = sentinela.dataset.temMais === "true";
    let carregando = false;
    let geracao = 0;                                  // invalida respostas antigas ao trocar filtros
    let controle = null;                              // AbortController da requisição atual

    // ---------- helpers ----------
    function parametros(numeroPagina) {
        const params = new URLSearchParams();
        for (const [chave, valor] of new FormData(form).entries()) {
            if (valor !== "") params.append(chave, valor);
        }
        if (numeroPagina) params.set("pagina", numeroPagina);
        return params;
    }

    function atualizarTotal(total) {
        totalEl.textContent = total.toLocaleString("pt-BR");
        totalTxt.textContent = total === 1 ? "produto" : "produtos";
    }

    function atualizarEstadoFinal() {
        const temCards = grid.children.length > 0;
        vazio.hidden = temCards;
        fim.hidden = temMais || !temCards;
    }

    function sentinelaVisivel() {
        return sentinela.getBoundingClientRect().top < window.innerHeight + 600;
    }

    // ---------- carregamento ----------
    async function carregar(reiniciar) {
        if (reiniciar) {
            geracao++;
            if (controle) controle.abort();
            pagina = 0;
            temMais = true;
            carregando = false;
            grid.innerHTML = "";
            vazio.hidden = true;
            fim.hidden = true;
        }

        if (carregando || !temMais) return;

        carregando = true;
        erro.hidden = true;
        loader.hidden = false;

        const minhaGeracao = geracao;
        controle = new AbortController();

        try {
            const resposta = await fetch(URL_FEED + "?" + parametros(pagina + 1), {
                headers: { "X-Requested-With": "XMLHttpRequest" },
                signal: controle.signal
            });

            if (!resposta.ok) throw new Error("HTTP " + resposta.status);

            const html = await resposta.text();
            if (minhaGeracao !== geracao) return;

            grid.insertAdjacentHTML("beforeend", html);
            pagina++;
            temMais = resposta.headers.get("X-Tem-Mais") === "true";

            const total = resposta.headers.get("X-Total");
            if (total !== null) atualizarTotal(parseInt(total, 10));

            atualizarEstadoFinal();
        } catch (e) {
            if (e.name === "AbortError" || minhaGeracao !== geracao) return;
            erro.hidden = false;
        } finally {
            if (minhaGeracao === geracao) {
                carregando = false;
                loader.hidden = true;

                // Se a tela for alta e o fim da lista ainda estiver visível, carrega a próxima
                if (temMais && erro.hidden && sentinelaVisivel()) carregar(false);
            }
        }
    }

    // ---------- scroll infinito ----------
    new IntersectionObserver(function (entradas) {
        if (entradas[0].isIntersecting) carregar(false);
    }, { rootMargin: "600px 0px" }).observe(sentinela);

    document.getElementById("feedTentar").addEventListener("click", function () {
        carregar(false);
    });

    // ---------- filtros ----------
    function aplicarFiltros() {
        // mantém a URL compartilhável (sem a página)
        const qs = parametros().toString();
        history.replaceState(null, "", qs ? "?" + qs : location.pathname);

        const topo = document.querySelector(".feed-layout").getBoundingClientRect().top + window.scrollY - 80;
        if (window.scrollY > topo) window.scrollTo({ top: Math.max(topo, 0), behavior: "smooth" });

        carregar(true);
    }

    let temporizador = null;

    form.addEventListener("change", aplicarFiltros);
    ordem.addEventListener("change", aplicarFiltros);

    form.querySelectorAll('input[type="number"]').forEach(function (campo) {
        campo.addEventListener("input", function () {
            clearTimeout(temporizador);
            temporizador = setTimeout(aplicarFiltros, 500);
        });
    });

    form.addEventListener("submit", function (e) {
        e.preventDefault();
        aplicarFiltros();
        filtros.classList.remove("is-open");
        btnFiltros.setAttribute("aria-expanded", "false");
    });

    document.getElementById("feedLimpar").addEventListener("click", function (e) {
        e.preventDefault();
        form.querySelectorAll('input[type="checkbox"]').forEach(function (c) { c.checked = false; });
        form.querySelectorAll('input[type="number"]').forEach(function (c) { c.value = ""; });
        ordem.value = "recentes";
        aplicarFiltros();
    });

    // ---------- filtros no celular ----------
    btnFiltros.addEventListener("click", function () {
        const aberto = filtros.classList.toggle("is-open");
        btnFiltros.setAttribute("aria-expanded", String(aberto));
    });

    atualizarEstadoFinal();
});