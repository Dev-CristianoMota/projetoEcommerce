document.addEventListener('DOMContentLoaded', () => {
    const dialog = document.getElementById('dialogPrincipal');
    if (!dialog) return;

    const erro = document.getElementById('dialogErro');
    const btnSim = document.getElementById('btnSim');
    const btnNao = document.getElementById('btnNao');
    const url = dialog.dataset.url;
    let cdSelecionado = null;

    document.querySelectorAll('.address-set-primary').forEach(btn => {
        btn.addEventListener('click', () => {
            cdSelecionado = btn.dataset.cdEndereco;
            erro.hidden = true;
            dialog.showModal();
        });
    });

    btnNao.addEventListener('click', () => dialog.close());

    btnSim.addEventListener('click', async () => {
        btnSim.disabled = true;
        try {
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            const resposta = await fetch(url, {
                method: 'POST',
                body: new URLSearchParams({
                    cdEndereco: cdSelecionado,
                    __RequestVerificationToken: token
                })
            });

            if (resposta.ok) {
                location.reload();
                return;
            }

            const dados = await resposta.json().catch(() => null);
            erro.textContent = dados?.mensagem ?? 'Erro ao processar a solicitação.';
            erro.hidden = false;
        } catch {
            erro.textContent = 'Falha de conexão. Tente novamente.';
            erro.hidden = false;
        } finally {
            btnSim.disabled = false;
        }
    });
});


document.addEventListener('DOMContentLoaded', () => {
    const dialogEditar = document.getElementById('dialogEditar');
    if (!dialogEditar) return;

    const form = document.getElementById('formEditar');
    const erro = document.getElementById('dialogEditarErro');
    const btnSalvar = document.getElementById('btnEditarSalvar');
    const url = dialogEditar.dataset.url;

    const campos = {
        CdEndereco: 'cdEndereco',
        NmIdentificacao: 'identificacao',
        NmDestinatario: 'destinatario',
        CEP: 'cep',
        Logradouro: 'logradouro',
        Numero: 'numero',
        DsComplemento: 'complemento',
        Bairro: 'bairro',
        Cidade: 'cidade',
        UF: 'uf'
    };

    document.querySelectorAll('.address-edit').forEach(btn => {
        btn.addEventListener('click', () => {
            for (const [nome, chave] of Object.entries(campos)) {
                form.elements[nome].value = btn.dataset[chave] ?? '';
            }
            erro.hidden = true;
            dialogEditar.showModal();
        });
    });

    document.getElementById('btnEditarCancelar')
        .addEventListener('click', () => dialogEditar.close());

    dialogEditar.addEventListener('click', (e) => {
        if (e.target === dialogEditar) dialogEditar.close();
    });

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        if (!form.reportValidity()) return;

        btnSalvar.disabled = true;
        try {
            const dados = new FormData(form);
            dados.append(
                '__RequestVerificationToken',
                document.querySelector('input[name="__RequestVerificationToken"]').value
            );

            const resposta = await fetch(url, { method: 'POST', body: dados });

            if (resposta.ok) {
                location.reload();
                return;
            }

            const json = await resposta.json().catch(() => null);
            erro.textContent = json?.mensagem ?? 'Erro ao salvar o endereço.';
            erro.hidden = false;
        } catch {
            erro.textContent = 'Falha de conexão. Tente novamente.';
            erro.hidden = false;
        } finally {
            btnSalvar.disabled = false;
        }
    });
});


document.addEventListener('DOMContentLoaded', () => {
    const dialogExcluir = document.getElementById('dialogExcluir');
    if (!dialogExcluir) return;

    const erro = document.getElementById('dialogExcluirErro');
    const aviso = document.getElementById('dialogExcluirAviso');
    const btnSim = document.getElementById('btnExcluirSim');
    const btnNao = document.getElementById('btnExcluirNao');
    const url = dialogExcluir.dataset.url;
    let cdSelecionado = null;

    document.querySelectorAll('.address-delete').forEach(btn => {
        btn.addEventListener('click', () => {
            cdSelecionado = btn.dataset.cdEndereco;
            aviso.hidden = btn.dataset.principal !== 'true';
            erro.hidden = true;
            dialogExcluir.showModal();
        });
    });

    btnNao.addEventListener('click', () => dialogExcluir.close());

    dialogExcluir.addEventListener('click', (e) => {
        if (e.target === dialogExcluir) dialogExcluir.close();
    });

    btnSim.addEventListener('click', async () => {
        btnSim.disabled = true;
        try {
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            const resposta = await fetch(url, {
                method: 'POST',
                body: new URLSearchParams({
                    cdEndereco: cdSelecionado,
                    __RequestVerificationToken: token
                })
            });

            if (resposta.ok) {
                location.reload();
                return;
            }

            const dados = await resposta.json().catch(() => null);
            erro.textContent = dados?.mensagem ?? 'Erro ao excluir o endereço.';
            erro.hidden = false;
        } catch {
            erro.textContent = 'Falha de conexão. Tente novamente.';
            erro.hidden = false;
        } finally {
            btnSim.disabled = false;
        }
    });
});


document.addEventListener('DOMContentLoaded', () => {
    const dialog = document.getElementById('dialogCadastrar');
    if (!dialog) return;

    const form = document.getElementById('formCadastrar');
    const campos = document.getElementById('camposEndereco');
    const inputCep = form.elements['CEP'];
    const btnBuscar = document.getElementById('btnBuscarCep');
    const btnSalvar = document.getElementById('btnCadastrarSalvar');
    const status = document.getElementById('cepStatus');
    const erro = document.getElementById('dialogCadastrarErro');
    const url = dialog.dataset.url;

    // campo do formulário -> chave da resposta do ViaCEP
    const camposViaCep = {
        Logradouro: 'logradouro',
        Bairro: 'bairro',
        Cidade: 'localidade',
        UF: 'uf'
    };

    let cepConsultado = null; // CEP (só dígitos) já consultado com sucesso

    const somenteDigitos = (v) => v.replace(/\D/g, '');

    function mostrarStatus(texto, tipo) {
        status.textContent = texto;
        status.className = `cep-status cep-status-${tipo}`;
        status.hidden = false;
    }

    function atualizarBotoes() {
        const cepCompleto = somenteDigitos(inputCep.value).length === 8;
        btnBuscar.disabled = !cepCompleto;

        btnSalvar.disabled = !(
            cepConsultado &&
            cepConsultado === somenteDigitos(inputCep.value) &&
            form.checkValidity()
        );
    }

    function limparEndereco() {
        campos.querySelectorAll('input').forEach(input => {
            input.value = '';
            input.readOnly = false;
        });
        campos.disabled = true;
        cepConsultado = null;
    }

    async function buscarCep() {
        const cep = somenteDigitos(inputCep.value);
        if (cep.length !== 8) return;

        btnBuscar.disabled = true;
        mostrarStatus('Buscando endereço...', 'info');

        try {
            const resposta = await fetch(`https://viacep.com.br/ws/${cep}/json/`, {
                signal: AbortSignal.timeout(8000)
            });
            if (!resposta.ok) throw new Error('Falha na consulta');

            const dados = await resposta.json();

            if (dados.erro) {
                limparEndereco();
                mostrarStatus('CEP não encontrado. Confira o número digitado.', 'erro');
                return;
            }

            for (const [nome, chave] of Object.entries(camposViaCep)) {
                const input = form.elements[nome];
                const valor = dados[chave] ?? '';
                input.value = valor;
                // o que o ViaCEP devolveu fica travado; o que veio vazio o usuário preenche
                input.readOnly = valor !== '';
            }

            campos.disabled = false;
            cepConsultado = cep;
            mostrarStatus('Endereço encontrado. Complete os campos restantes.', 'ok');
            form.elements['NmIdentificacao'].focus();
        } catch {
            limparEndereco();
            mostrarStatus('Não foi possível consultar o CEP. Tente novamente.', 'erro');
        } finally {
            atualizarBotoes();
        }
    }

    // Abrir o dialog (botão do cabeçalho e do estado vazio)
    document.querySelectorAll('.js-novo-endereco').forEach(btn => {
        btn.addEventListener('click', () => {
            form.reset();
            limparEndereco();
            status.hidden = true;
            erro.hidden = true;
            atualizarBotoes();
            dialog.showModal();
            inputCep.focus();
        });
    });

    // Máscara do CEP; se o usuário alterar o CEP depois da busca, o endereço é descartado
    inputCep.addEventListener('input', () => {
        const d = somenteDigitos(inputCep.value).slice(0, 8);
        inputCep.value = d.length > 5 ? `${d.slice(0, 5)}-${d.slice(5)}` : d;

        if (cepConsultado && cepConsultado !== d) {
            limparEndereco();
            status.hidden = true;
        }
        atualizarBotoes();
    });

    // Enter no campo CEP faz a busca em vez de tentar enviar o formulário
    inputCep.addEventListener('keydown', (e) => {
        if (e.key === 'Enter') {
            e.preventDefault();
            buscarCep();
        }
    });

    btnBuscar.addEventListener('click', buscarCep);

    // Libera o botão "Cadastrar" conforme os campos são preenchidos
    form.addEventListener('input', atualizarBotoes);

    document.getElementById('btnCadastrarCancelar')
        .addEventListener('click', () => dialog.close());

    dialog.addEventListener('click', (e) => {
        if (e.target === dialog) dialog.close();
    });

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        if (btnSalvar.disabled) return;

        btnSalvar.disabled = true;
        erro.hidden = true;

        try {
            const dados = new FormData(form);
            dados.append(
                '__RequestVerificationToken',
                document.querySelector('input[name="__RequestVerificationToken"]').value
            );

            const resposta = await fetch(url, { method: 'POST', body: dados });

            if (resposta.ok) {
                location.reload();
                return;
            }

            const json = await resposta.json().catch(() => null);
            erro.textContent = json?.mensagem ?? 'Erro ao cadastrar o endereço.';
            erro.hidden = false;
        } catch {
            erro.textContent = 'Falha de conexão. Tente novamente.';
            erro.hidden = false;
        } finally {
            atualizarBotoes();
        }
    });
});