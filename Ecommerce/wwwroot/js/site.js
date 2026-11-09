document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("searchForm");
    if (!form) return;

    const input = document.getElementById("searchInput");
    const lista = document.getElementById("searchSugestoes");
    const urlSugestoes = form.dataset.urlSugestoes;
    const urlDetalhes = form.dataset.urlDetalhes;

    let temporizador = null;
    let controle = null;
    let ativo = -1;

    function fechar() {
        lista.hidden = true;
        lista.innerHTML = "";
        ativo = -1;
        input.setAttribute("aria-expanded", "false");
    }

    function links() {
        return Array.from(lista.querySelectorAll("a"));
    }

    function destacar(indice) {
        const itens = links();
        itens.forEach(a => a.classList.remove("is-active"));
        ativo = indice;
        if (indice >= 0 && itens[indice]) {
            itens[indice].classList.add("is-active");
            itens[indice].scrollIntoView({ block: "nearest" });
        }
    }

    function renderizar(produtos, termo) {
        lista.innerHTML = "";
        ativo = -1;

        if (produtos.length === 0) {
            const vazio = document.createElement("li");
            vazio.className = "search-suggestion-empty";
            vazio.textContent = 'Nenhum produto encontrado com o nome "' + termo + '"';
            lista.appendChild(vazio);
        } else {
            produtos.forEach(function (p) {
                const li = document.createElement("li");
                li.setAttribute("role", "option");

                const a = document.createElement("a");
                a.href = urlDetalhes + "?id=" + p.cdProduto;

                if (p.nmUrlImagem) {
                    const img = document.createElement("img");
                    img.src = p.nmUrlImagem;
                    img.alt = "";
                    a.appendChild(img);
                }

                const nome = document.createElement("span");
                nome.className = "suggestion-name";
                nome.textContent = p.nmProduto;

                const preco = document.createElement("span");
                preco.className = "suggestion-price";
                preco.textContent = p.vlProduto.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

                a.append(nome, preco);
                li.appendChild(a);
                lista.appendChild(li);
            });
        }

        lista.hidden = false;
        input.setAttribute("aria-expanded", "true");
    }

    async function buscar(termo) {
        if (controle) controle.abort();
        controle = new AbortController();

        try {
            const resposta = await fetch(urlSugestoes + "?termo=" + encodeURIComponent(termo), {
                signal: controle.signal
            });
            if (!resposta.ok) throw new Error("HTTP " + resposta.status);

            // ignora se o usuário já digitou outra coisa
            if (input.value.trim() !== termo) return;
            renderizar(await resposta.json(), termo);
        } catch (e) {
            if (e.name !== "AbortError") fechar();
        }
    }

    input.addEventListener("input", function () {
        clearTimeout(temporizador);
        const termo = input.value.trim();

        if (termo.length < 2) {
            if (controle) controle.abort();
            fechar();
            return;
        }
        temporizador = setTimeout(function () { buscar(termo); }, 250);
    });

    input.addEventListener("keydown", function (e) {
        const itens = links();

        if (e.key === "ArrowDown" && itens.length) {
            e.preventDefault();
            destacar((ativo + 1) % itens.length);
        } else if (e.key === "ArrowUp" && itens.length) {
            e.preventDefault();
            destacar(ativo <= 0 ? itens.length - 1 : ativo - 1);
        } else if (e.key === "Enter" && ativo >= 0 && itens[ativo]) {
            e.preventDefault();
            window.location.href = itens[ativo].href;
        } else if (e.key === "Escape") {
            fechar();
        }
    });

    input.addEventListener("focus", function () {
        if (lista.children.length) lista.hidden = false;
    });

    document.addEventListener("click", function (e) {
        if (!form.contains(e.target)) fechar();
    });

    form.addEventListener("submit", function (e) {
        if (input.value.trim() === "") e.preventDefault();
    });
});