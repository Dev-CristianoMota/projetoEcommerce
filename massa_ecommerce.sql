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
 
 
INSERT INTO usuario (cd_usuario, nm_apelido, nm_usuario, nm_telefone, nm_email, nm_senha, cd_tipo_usuario, dt_criacao, dt_atualizacao, ic_usuario_ativo) VALUES
    (1, 'admin', 'Administrador do Sistema', '11998765432', 'admin@ecommerce.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 3, NOW(), NOW(), true),
    (2, 'carlos_tech', 'Carlos Almeida', '21987654321', 'carlos.almeida@email.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 1, NOW(), NOW(), true),
    (3, 'marina_vendas', 'Marina Costa', '31976543210', 'marina.costa@email.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 1, NOW(), NOW(), true),
    (4, 'joao_comprador', 'João Pereira', '41965432109', 'joao.pereira@email.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 2, NOW(), NOW(), true),
    (5, 'ana_compradora',   'Ana Souza',      '11912345678', 'ana.souza@email.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 2, NOW(), NOW(), true),
    (6, 'bruno_comprador',  'Bruno Martins',  '21923456789', 'bruno.martins@email.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 2, NOW(), NOW(), true),
    (7, 'carla_compradora', 'Carla Oliveira', '31934567890', 'carla.oliveira@email.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 2, NOW(), NOW(), true),
    (8, 'diego_comprador',  'Diego Santos',   '41945678901', 'diego.santos@email.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 2, NOW(), NOW(), true),
    (9, 'elisa_compradora', 'Elisa Ferreira', '51956789012', 'elisa.ferreira@email.com', 'AQAAAAIAAYagAAAAEFQInuSAsQk8Ti5CAIDPzVTPWLYDkGoGp3ep2Qq+qOsKtnTjii+d+5dzSnQATPQhOQ==', 2, NOW(), NOW(), true);
 
 
INSERT INTO loja_virtual (cd_loja_virtual, cd_usuario, nm_loja_virtual, ds_loja_virtual, dt_criacao) VALUES
    (1, 2, 'TechCenter Eletrônicos', 'Notebooks, periféricos, celulares e eletrônicos em geral.', NOW()),
    (2, 3, 'Marina Variedades', 'Moda, casa, esporte, beleza e muito mais.', NOW()),
    (3, 3, 'Marina Outlet', 'Loja recém-criada, ainda sem produtos cadastrados.', NOW());
 
 
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (1,  'Eletrônicos, Áudio e Vídeo');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (2,  'Computadores');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (3,  'Celulares e Telefones');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (4,  'Eletrodomésticos');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (5,  'Moda e Acessórios');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (6,  'Casa e Decoração');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (7,  'Beleza e Cuidados Pessoais');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (8,  'Alimentos e Bebidas');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (9,  'Esporte e Lazer');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (10, 'Livros e Papelaria');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (11, 'Veículos e Peças');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (12, 'Agro');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (13, 'Pet Shop');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (14, 'Antiguidades e Coleções');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (15, 'Bebês');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (16, 'Brinquedos');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (17, 'Câmeras e Fotografia');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (18, 'Construção');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (19, 'Ferramentas');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (20, 'Festas e Lembrancinhas');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (21, 'Games');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (22, 'Instrumentos Musicais');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (23, 'Joias e Relógios');
INSERT INTO categoria (cd_categoria, nm_categoria) VALUES (24, 'Saúde');
 
INSERT INTO produto (cd_produto, cd_loja_virtual, cd_categoria, nm_produto, ds_produto, vl_produto, qt_estoque, dt_criacao) VALUES
    (1, 1, 2, 'Notebook Ultrafino 14"', 'Intel Core i5, 16 GB de RAM, SSD de 512 GB', 3899.90, 15, NOW()),
    (2, 1, 2, 'Mouse Sem Fio Ergonômico', 'Conexão 2.4 GHz, 6 botões, bateria de longa duração', 89.90, 120, NOW()),
    (3, 1, 2, 'Teclado Mecânico RGB', 'Switch azul, layout ABNT2, iluminação RGB', 249.90, 60, NOW()),
    (4, 1, 3, 'Smartphone 128 GB', 'Tela de 6,5", câmera tripla e 8 GB de RAM', 1799.00, 40, NOW()),
    (5, 1, 1, 'Fone de Ouvido Bluetooth', 'Cancelamento de ruído e até 30 h de bateria', 199.90, 80, NOW()),
    (6, 1, 3, 'Carregador USB-C 65W', 'Carregamento rápido para notebooks e celulares', 119.90, 100, NOW()),
    (7, 1, 1, 'Smart TV 50" 4K', 'Painel LED, HDR, Wi-Fi e aplicativos de streaming', 2599.00, 10, NOW()),
    (8, 1, 1, 'Caixa de Som Portátil', 'Resistente à água, Bluetooth e 12 h de bateria', 159.90, 55, NOW()),
 
    (9, 2, 5, 'Camiseta Básica Algodão', 'Camiseta 100% algodão, disponível em várias cores', 39.90, 200, NOW()),
    (10, 2, 5, 'Calça Jeans Slim', 'Jeans com elastano e corte slim', 129.90, 90, NOW()),
    (11, 2, 7, 'Tênis Esportivo Running', 'Amortecimento reforçado para corrida', 299.90, 45, NOW()),
    (12, 2, 4, 'Conjunto de Panelas Antiaderentes', 'Kit com 5 peças e revestimento antiaderente', 349.90, 20, NOW()),
    (13, 2, 8, 'Perfume Floral 100 ml', 'Fragrância floral, eau de parfum', 219.90, 35, NOW()),
    (14, 2, 9, 'Blocos de Montar 500 peças', 'Indicado para crianças a partir de 6 anos', 119.90, 60, NOW()),
    (15, 2, 6, 'Livro Clean Code', 'Guia de boas práticas para escrever código limpo', 89.90, 30, NOW());
 
INSERT INTO cupom (cd_cupom, cd_promocional, sg_tipo_desconto, vl_desconto, dt_inicio_validade, dt_fim_validade) VALUES
    (1, 'BEMVINDO10',  'P', 10.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY)),
    (2, 'DESCONTO30',  'F', 30.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY)),
    (3, 'VERAO15',     'P', 15.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY)),
    (4, 'PASCOA15',    'P', 15.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY)),
    (5, 'BLACKWEEK20', 'P', 20.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY));
 
-- Pedidos (status: P=Pendente, A=Aprovado, E=Enviado, T=Entregue, C=Cancelado)
-- Valores corrigidos nos pedidos 6, 9, 19 e 21 (ver itens abaixo).
INSERT INTO pedido (cd_pedido, cd_usuario, cd_loja_virtual, cd_cupom, sg_status_pedido, vl_subtotal_pedido, vl_desconto_pedido, vl_total_pedido, dt_criacao_pedido) VALUES
    (1,  4, 1, NULL, 'T',  429.70,   0.00,  429.70, NOW()),
    (2,  4, 1, 1,    'E', 1918.90, 191.89, 1727.01, NOW()),
    (3,  4, 2, 2,    'P',  419.60,  30.00,  389.60, NOW()),
    (4,  4, 1, NULL, 'E', 3899.90,   0.00, 3899.90, NOW()),
    (5,  5, 1, 3,    'T', 3598.00, 539.70, 3058.30, NOW()),
    (6,  6, 2, NULL, 'P',  299.90,   0.00,  299.90, NOW()),
    (7,  7, 1, 1,    'E',  409.80,  40.98,  368.82, NOW()),
    (8,  8, 2, 5,    'T',  479.80,  95.96,  383.84, NOW()),
    (9,  9, 1, NULL, 'P', 2798.90,   0.00, 2798.90, NOW()),
    (10, 5, 2, 4,    'E',  349.90,  52.49,  297.41, NOW()),
    (11, 6, 1, NULL, 'T',  319.80,   0.00,  319.80, NOW()),
    (12, 7, 2, 1,    'P',  259.80,  25.98,  233.82, NOW()),
    (13, 8, 1, 2,    'E',  199.90,  30.00,  169.90, NOW()),
    (14, 9, 2, NULL, 'T',  429.70,   0.00,  429.70, NOW()),
    (15, 4, 1, 3,    'P',  209.80,  31.47,  178.33, NOW()),
    (16, 5, 2, NULL, 'E',  249.80,   0.00,  249.80, NOW()),
    (17, 6, 1, 5,    'T', 4099.80, 819.96, 3279.84, NOW()),
    (18, 7, 2, 4,    'P',  519.80,  77.97,  441.83, NOW()),
    (19, 8, 1, NULL, 'E',  249.90,   0.00,  249.90, NOW()),
    (20, 9, 2, 2,    'T',  419.60,  30.00,  389.60, NOW()),
    (21, 4, 1, NULL, 'P',  499.80,   0.00,  499.80, NOW()),
    (22, 5, 2, 3,    'E',  509.70,  76.46,  433.24, NOW()),
    (23, 6, 1, NULL, 'T', 2599.00,   0.00, 2599.00, NOW());
 
INSERT INTO item_pedido (cd_item_pedido, cd_pedido, cd_produto, qt_item, vl_preco_unitario) VALUES
    -- Pedido 1 (loja 1): 2 x 89.90 + 249.90 = 429.70
    (1,  1,  2, 2,   89.90),
    (2,  1,  3, 1,  249.90),
    -- Pedido 2 (loja 1): 1799.00 + 119.90 = 1918.90
    (3,  2,  4, 1, 1799.00),
    (4,  2,  6, 1,  119.90),
    -- Pedido 3 (loja 2): 3 x 39.90 + 299.90 = 419.60
    (5,  3,  9, 3,   39.90),
    (6,  3, 11, 1,  299.90),
    -- Pedido 4 (loja 1): 3899.90
    (7,  4,  1, 1, 3899.90),
    -- Pedido 5 (loja 1): 2 x 1799.00 = 3598.00
    (8,  5,  4, 2, 1799.00),
    -- Pedido 6 (loja 2): 299.90
    (9,  6, 11, 1,  299.90),
    -- Pedido 7 (loja 1): 249.90 + 159.90 = 409.80
    (10, 7,  3, 1,  249.90),
    (11, 7,  8, 1,  159.90),
    -- Pedido 8 (loja 2): 349.90 + 129.90 = 479.80
    (12, 8, 12, 1,  349.90),
    (13, 8, 10, 1,  129.90),
    -- Pedido 9 (loja 1): 2599.00 + 199.90 = 2798.90
    (14, 9,  7, 1, 2599.00),
    (15, 9,  5, 1,  199.90),
    -- Pedido 10 (loja 2): 349.90
    (16, 10, 12, 1, 349.90),
    -- Pedido 11 (loja 1): 2 x 159.90 = 319.80
    (17, 11, 8, 2,  159.90),
    -- Pedido 12 (loja 2): 2 x 129.90 = 259.80
    (18, 12, 10, 2, 129.90),
    -- Pedido 13 (loja 1): 199.90
    (19, 13, 5, 1,  199.90),
    -- Pedido 14 (loja 2): 219.90 + 119.90 + 89.90 = 429.70
    (20, 14, 13, 1, 219.90),
    (21, 14, 14, 1, 119.90),
    (22, 14, 15, 1,  89.90),
    -- Pedido 15 (loja 1): 89.90 + 119.90 = 209.80
    (23, 15, 2, 1,   89.90),
    (24, 15, 6, 1,  119.90),
    -- Pedido 16 (loja 2): 129.90 + 119.90 = 249.80
    (25, 16, 10, 1, 129.90),
    (26, 16, 14, 1, 119.90),
    -- Pedido 17 (loja 1): 3899.90 + 199.90 = 4099.80
    (27, 17, 1, 1, 3899.90),
    (28, 17, 5, 1,  199.90),
    -- Pedido 18 (loja 2): 219.90 + 299.90 = 519.80
    (29, 18, 13, 1, 219.90),
    (30, 18, 11, 1, 299.90),
    -- Pedido 19 (loja 1): 249.90
    (31, 19, 3, 1,  249.90),
    -- Pedido 20 (loja 2): 3 x 39.90 + 299.90 = 419.60
    (32, 20, 9, 3,   39.90),
    (33, 20, 11, 1, 299.90),
    -- Pedido 21 (loja 1): 2 x 249.90 = 499.80
    (34, 21, 3, 2,  249.90),
    -- Pedido 22 (loja 2): 349.90 + 119.90 + 39.90 = 509.70
    (35, 22, 12, 1, 349.90),
    (36, 22, 14, 1, 119.90),
    (37, 22, 9, 1,   39.90),
    -- Pedido 23 (loja 1): 2599.00
    (38, 23, 7, 1, 2599.00);

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
    
SELECT
    p.cd_produto,
    p.nm_produto,
    SUM(i.qt_item) AS qt_total_vendida,
    COUNT(DISTINCT i.cd_pedido) AS qtd_pedidos,
    SUM(i.qt_item * i.vl_preco_unitario) AS vl_total_vendido
FROM item_pedido i
JOIN pedido  pe ON pe.cd_pedido  = i.cd_pedido
JOIN produto p  ON p.cd_produto  = i.cd_produto
WHERE pe.sg_status_pedido <> 'C' 
GROUP BY p.cd_produto, p.nm_produto
ORDER BY qt_total_vendida DESC
LIMIT 5;
