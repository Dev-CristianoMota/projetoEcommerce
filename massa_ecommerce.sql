-- ============================================================
-- Massa de testes — db_ecommerce
-- Execute DEPOIS de criar as tabelas.
--
-- ATENÇÃO: o bloco de limpeza abaixo apaga TODOS os dados das
-- tabelas, para o script poder ser executado quantas vezes precisar.
--
-- Senha de todos os usuários de teste: Senha@123 (guardada como SHA-256)
-- As datas são relativas a NOW(), então os cupons continuam
-- "vencidos" ou "válidos" mesmo se você rodar o script outro dia.
-- ============================================================

USE db_ecommerce;

SET FOREIGN_KEY_CHECKS = 0;
TRUNCATE TABLE item_pedido;
TRUNCATE TABLE pedido;
TRUNCATE TABLE cupom;
TRUNCATE TABLE produto;
TRUNCATE TABLE categoria;
TRUNCATE TABLE loja_virtual;
TRUNCATE TABLE usuario;
TRUNCATE TABLE tipo_usuario;
SET FOREIGN_KEY_CHECKS = 1;


INSERT INTO tipo_usuario (cd_tipo_usuario, nm_tipo_usuario) VALUES(1, 'Vendedor');
INSERT INTO tipo_usuario (cd_tipo_usuario, nm_tipo_usuario) VALUES(2, 'Comprador');
INSERT INTO tipo_usuario (cd_tipo_usuario, nm_tipo_usuario) VALUES(3, 'Admin');


INSERT INTO usuario (cd_usuario, nm_apelido, nm_usuario, nm_telefone, nm_email, nm_senha, cd_tipo_usuario, dt_criacao) VALUES
    (1, 'admin', 'Administrador do Sistema', '11998765432', 'admin@ecommerce.com', SHA2('Senha@123', 256), 3, NOW()),
    (2, 'carlos_tech', 'Carlos Almeida', '21987654321', 'carlos.almeida@email.com', SHA2('Senha@123', 256), 1, NOW()),
    (3, 'marina_vendas', 'Marina Costa', '31976543210', 'marina.costa@email.com', SHA2('Senha@123', 256), 1, NOW()),
    (4, 'joao_comprador', 'João Pereira', '41965432109', 'joao.pereira@email.com', SHA2('Senha@123', 256), 2, NOW());


INSERT INTO loja_virtual (cd_loja_virtual, cd_usuario, nm_loja_virtual, ds_loja_virtual, dt_criacao) VALUES
    (1, 2, 'TechCenter Eletrônicos', 'Notebooks, periféricos, celulares e eletrônicos em geral.', NOW()),
    (2, 3, 'Marina Variedades', 'Moda, casa, esporte, beleza e muito mais.', NOW()),
    (3, 3, 'Marina Outlet', 'Loja recém-criada, ainda sem produtos cadastrados.', NOW());


INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (1,  'Eletrônicos');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (2,  'Informática');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (3,  'Telefonia');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (4,  'Eletrodomésticos');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (5,  'Moda e Acessórios');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (6,  'Casa e Decoração');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (7,  'Beleza e Cuidados Pessoais');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (8,  'Alimentos e Bebidas');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (9,  'Esporte e Lazer');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (10, 'Livros e Papelaria');

INSERT INTO produto (cd_produto, cd_loja_virtual, cd_categoria, nm_produto, ds_produto, vl_produto, qt_estoque, dt_criacao) VALUES
    -- Loja 1: TechCenter Eletrônicos
    (1, 1, 2, 'Notebook Ultrafino 14"', 'Intel Core i5, 16 GB de RAM, SSD de 512 GB', 3899.90, 15, NOW()),
    (2, 1, 2, 'Mouse Sem Fio Ergonômico', 'Conexão 2.4 GHz, 6 botões, bateria de longa duração', 89.90, 120, NOW()),
    (3, 1, 2, 'Teclado Mecânico RGB', 'Switch azul, layout ABNT2, iluminação RGB', 249.90, 60, NOW()),
    (4, 1, 3, 'Smartphone 128 GB', 'Tela de 6,5", câmera tripla e 8 GB de RAM', 1799.00, 40, NOW()),
    (5, 1, 1, 'Fone de Ouvido Bluetooth', 'Cancelamento de ruído e até 30 h de bateria', 199.90, 80, NOW()),
    (6, 1, 3, 'Carregador USB-C 65W', 'Carregamento rápido para notebooks e celulares', 119.90, 100, NOW()),
    (7, 1, 1, 'Smart TV 50" 4K', 'Painel LED, HDR, Wi-Fi e aplicativos de streaming', 2599.00, 10, NOW()),
    (8, 1, 1, 'Caixa de Som Portátil', 'Resistente à água, Bluetooth e 12 h de bateria', 159.90, 55, NOW()),

    -- Loja 2: Marina Variedades
    (9, 2, 5, 'Camiseta Básica Algodão', 'Camiseta 100% algodão, disponível em várias cores', 39.90, 200, NOW()),
    (10, 2, 5, 'Calça Jeans Slim', 'Jeans com elastano e corte slim', 129.90, 90, NOW()),
    (11, 2, 7, 'Tênis Esportivo Running', 'Amortecimento reforçado para corrida', 299.90, 45, NOW()),
    (12, 2, 4, 'Conjunto de Panelas Antiaderentes', 'Kit com 5 peças e revestimento antiaderente', 349.90, 20, NOW()),
    (13, 2, 8, 'Perfume Floral 100 ml', 'Fragrância floral, eau de parfum', 219.90, 35, NOW()),
    (14, 2, 9, 'Blocos de Montar 500 peças', 'Indicado para crianças a partir de 6 anos', 119.90, 60, NOW()),
    (15, 2, 6, 'Livro Clean Code', 'Guia de boas práticas para escrever código limpo', 89.90, 30, NOW());

INSERT INTO cupom (cd_cupom, cd_promocional, sg_tipo_desconto, vl_desconto, dt_inicio_validade, dt_fim_validade) VALUES
    (1, 'BEMVINDO10',  'P', 10.00, NOW(),  NOW()),
    (2, 'DESCONTO30',  'F', 30.00, NOW(),  NOW()),
    (3, 'VERAO15',     'P', 15.00, NOW(),  NOW()),
    (4, 'PASCOA15',    'P', 15.00, NOW(), NOW()),
    (5, 'BLACKWEEK20', 'P', 20.00, NOW(), NOW());

INSERT INTO pedido (cd_pedido, cd_usuario, cd_loja_virtual, cd_cupom, sg_status_pedido,
                    vl_subtotal_pedido, vl_desconto_pedido, vl_total_pedido, dt_criacao_pedido) VALUES
    -- Sem cupom
    (1, 4, 1, NULL, 'T',  429.70,   0.00,  429.70, DATE_SUB(NOW(4), INTERVAL 45 DAY)),
    -- Cupom BEMVINDO10 (10% de 1918,90)
    (2, 4, 1, 1,    'E', 1918.90, 191.89, 1727.01, DATE_SUB(NOW(4), INTERVAL 10 DAY)),
    -- Cupom DESCONTO30 (R$ 30 fixos)
    (3, 4, 2, 2,    'P',  419.60,  30.00,  389.60, DATE_SUB(NOW(4), INTERVAL 2 DAY));

INSERT INTO item_pedido (cd_item_pedido, cd_pedido, cd_produto, qt_item, vl_preco_unitario) VALUES
    -- Pedido 1 (loja 1): 2 x mouse + 1 x teclado = 429,70
    (1, 1,  2, 2,   89.90),
    (2, 1,  3, 1,  249.90),
    -- Pedido 2 (loja 1): smartphone + carregador = 1918,90
    (3, 2,  4, 1, 1799.00),
    (4, 2,  6, 1,  119.90),
    -- Pedido 3 (loja 2): 3 x camiseta + 1 x tênis = 419,60
    (5, 3,  9, 3,   39.90),
    (6, 3, 11, 1,  299.90);

SELECT * FROM usuario;

SELECT * FROM produto p
	JOIN loja_virtual l ON (l.cd_loja_virtual = p.cd_loja_virtual)
		WHERE p.cd_loja_virtual = 1;
   
SELECT * FROM produto p
	JOIN loja_virtual l ON (l.cd_loja_virtual = p.cd_loja_virtual)
		WHERE p.cd_loja_virtual = 2;
    
SELECT * FROM produto p
	JOIN loja_virtual l ON (l.cd_loja_virtual = p.cd_loja_virtual)
		WHERE p.cd_loja_virtual = 3;
        
SELECT vl_total_pedido FROM pedido
	WHERE cd_pedido = 1;
    
SELECT vl_total_pedido FROM pedido
	WHERE cd_pedido = 2;

SELECT vl_total_pedido FROM pedido
	WHERE cd_pedido = 3;
    