document.addEventListener("DOMContentLoaded", function () {
    function inicializarCarrossel(
        carouselId,
        prevId,
        nextId,
        indicadorId
    ) {
        const carousel = document.getElementById(carouselId);

        if (!carousel) return;

        const track = carousel.querySelector(".carousel-track");
        const paginas = carousel.querySelectorAll(".carousel-page");
        const btnPrev = document.getElementById(prevId);
        const btnNext = document.getElementById(nextId);
        const indicador = document.getElementById(indicadorId);

        if (!track || paginas.length === 0 || !btnPrev ||
            !btnNext || !indicador) {
            return;
        }

        let atual = 0;
        const total = paginas.length;

        function atualizar() {
            track.style.transform =
                `translateX(-${atual * 100}%)`;

            btnPrev.disabled = atual === 0;
            btnNext.disabled = atual === total - 1;

            indicador.textContent = `${atual + 1} / ${total}`;
        }

        btnPrev.addEventListener("click", function () {
            if (atual > 0) {
                atual--;
                atualizar();
            }
        });

        btnNext.addEventListener("click", function () {
            if (atual < total - 1) {
                atual++;
                atualizar();
            }
        });

        atualizar();
    }

    // Carrossel dos produtos mais vendidos
    inicializarCarrossel(
        "produtosCarousel",
        "btnPrev",
        "btnNext",
        "carouselIndicador"
    );

    // Carrossel dos produtos mais recentes
    inicializarCarrossel(
        "produtosRecentesCarousel",
        "recentesPrev",
        "recentesNext",
        "recentesIndicador"
    );

});

document.addEventListener("DOMContentLoaded", function () {

    const hero = document.getElementById("heroCarousel");
    if (!hero) return;

    const slides   = hero.querySelectorAll(".hero-slide");
    const total    = slides.length;
    const btnPrev  = document.getElementById("heroPrev");
    const btnNext  = document.getElementById("heroNext");
    const numero   = document.getElementById("heroAtual");
    const barra    = document.getElementById("heroProgresso");
    const controles = hero.querySelector(".hero-pagination");

    if (total === 0) return;

    if (total === 1) {
        controles.style.display = "none";
        return;
    }

    const DURACAO = 10000; // tempo de cada banner (ms)

    let atual = 0;
    let decorrido = 0;
    let ultimo = performance.now();
    let pausado = false;

    function mostrar(indice) {
        atual = (indice + total) % total;

        slides.forEach(function (slide, i) {
            const ativo = i === atual;
            slide.classList.toggle("is-active", ativo);
            slide.setAttribute("aria-hidden", String(!ativo));
        });

        numero.textContent = String(atual + 1).padStart(2, "0");
        decorrido = 0; // reinicia os 15s ao trocar (manual ou automático)
    }

    function tick(agora) {
        // limita o intervalo para não "pular" ao voltar de outra aba
        const dt = Math.min(agora - ultimo, 100);
        ultimo = agora;

        if (!pausado && !document.hidden) {
            decorrido += dt;

            if (decorrido >= DURACAO) {
                mostrar(atual + 1);
            }
        }

        barra.style.transform = "scaleX(" + Math.min(decorrido / DURACAO, 1) + ")";
        requestAnimationFrame(tick);
    }

    btnPrev.addEventListener("click", function () { mostrar(atual - 1); });
    btnNext.addEventListener("click", function () { mostrar(atual + 1); });

    // pausa enquanto o mouse ou o foco estiverem no banner
    hero.addEventListener("mouseenter", function () { pausado = true; });
    hero.addEventListener("mouseleave", function () { pausado = false; });
    hero.addEventListener("focusin",    function () { pausado = true; });
    hero.addEventListener("focusout",   function () { pausado = false; });

    // setas do teclado
    hero.addEventListener("keydown", function (e) {
        if (e.key === "ArrowLeft")  mostrar(atual - 1);
        if (e.key === "ArrowRight") mostrar(atual + 1);
    });

    // arrastar no celular
    let inicioX = 0;

    hero.addEventListener("touchstart", function (e) {
        inicioX = e.changedTouches[0].clientX;
    }, { passive: true });

    hero.addEventListener("touchend", function (e) {
        const diff = e.changedTouches[0].clientX - inicioX;

        if (Math.abs(diff) > 50) {
            mostrar(diff < 0 ? atual + 1 : atual - 1);
        }
    }, { passive: true });

    requestAnimationFrame(tick);
});