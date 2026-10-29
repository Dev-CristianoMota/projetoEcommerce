-- ============================================================
-- Massa de testes — db_ecommerce
-- Execute DEPOIS de criar as tabelas.
--
-- ATENÇÃO: o bloco de limpeza abaixo apaga TODOS os dados das
-- tabelas, para o script poder ser executado quantas vezes precisar.
--
-- Senha de todos os usuários de teste: Senha@123 (guardada como hash do ASP.NET Core Identity)
-- As datas são relativas a NOW(), então os cupons continuam
-- "vencidos" ou "válidos" mesmo se você rodar o script outro dia.
-- ============================================================

USE db_ecommerce;
 
SET FOREIGN_KEY_CHECKS = 0;
TRUNCATE TABLE item_pedido;
TRUNCATE TABLE pedido;
TRUNCATE TABLE endereco;
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


-- Endereços
-- O admin (usuário 1) não tem endereço de propósito, para testar usuário sem endereço.
-- O João (usuário 4) tem 3: Casa (principal), Trabalho e um apartamento antigo
-- desativado, que continua vinculado ao pedido 1 (histórico preservado).
INSERT INTO endereco (cd_endereco, cd_usuario, nm_identificacao, nm_destinatario, nm_cep, nm_logradouro, nm_numero, ds_complemento, nm_bairro, nm_cidade, sg_uf, ic_endereco_principal, ic_endereco_ativo, dt_criacao, dt_atualizacao) VALUES
    (1,  4, 'Casa', 'João Pereira',   '80010000', 'Rua XV de Novembro', '1200', 'Apto 52', 'Centro', 'Curitiba', 'PR', TRUE,  TRUE,  NOW(), NULL),
    (2,  4, 'Trabalho', 'João Pereira',   '80420010', 'Avenida Sete de Setembro', '3500', 'Sala 810', 'Batel', 'Curitiba', 'PR', FALSE, TRUE,  NOW(), NULL),
    (3,  4, 'Apartamento', 'João Pereira',   '80215901', 'Rua Padre Anchieta', '450',  NULL, 'Bigorrilho', 'Curitiba', 'PR', FALSE, FALSE, NOW(), NOW()),
	(4,  5, 'Casa', 'Ana Souza',      '01310100', 'Avenida Paulista', '1578', 'Bloco B, apto 91', 'Bela Vista', 'São Paulo', 'SP', TRUE,  TRUE,  NOW(), NULL),
    (5,  6, 'Casa', 'Bruno Martins',  '22040020', 'Rua Barata Ribeiro', '320',  'Apto 704', 'Copacabana', 'Rio de Janeiro', 'RJ', TRUE,  TRUE,  NOW(), NULL),
    (6,  7, 'Casa', 'Carla Oliveira', '30130010', 'Avenida Afonso Pena', '1500', NULL, 'Centro', 'Belo Horizonte', 'MG', TRUE,  TRUE,  NOW(), NULL),
    (7,  8, 'Casa', 'Diego Santos',   '80250104', 'Rua Marechal Deodoro', '630',  'Conjunto 12', 'Centro', 'Curitiba', 'PR', TRUE,  TRUE,  NOW(), NULL),
    (8,  9, 'Casa', 'Elisa Ferreira', '90010150', 'Rua dos Andradas', '1234', 'Apto 301', 'Centro Histórico', 'Porto Alegre',   'RS', TRUE,  TRUE,  NOW(), NULL),
    (9,  2, 'Apartamento', 'Carlos Almeida', '20040020', 'Rua da Assembleia', '10', '1201', 'Centro', 'Rio de Janeiro', 'RJ', TRUE,  TRUE,  NOW(), NULL),
    (10, 3, 'Casa', 'Marina Costa',   '30140071', 'Rua da Bahia', '1148', 'Sala 5', 'Lourdes', 'Belo Horizonte', 'MG', TRUE,  TRUE,  NOW(), NULL);
 
 
INSERT INTO loja_virtual (cd_loja_virtual, cd_usuario, nm_loja_virtual, ds_loja_virtual, dt_criacao) VALUES
    (1, 2, 'TechCenter Eletrônicos', 'Notebooks, periféricos, celulares e eletrônicos em geral.', NOW()),
    (2, 3, 'Marina Variedades', 'Moda, casa, esporte, beleza e muito mais.', NOW()),
    (3, 3, 'Marina Outlet', 'Loja recém-criada, ainda sem produtos cadastrados.', NOW()),
    (4, 3, 'Casa & Conforto', 'Eletrodomésticos, decoração, construção e ferramentas para o seu lar.', NOW()),
    (5, 2, 'Garagem Automotiva', 'Peças, acessórios e produtos de limpeza para o seu veículo.', NOW()),
    (6, 3, 'PetCampo', 'Tudo para pets, além de produtos para o campo e alimentos selecionados.', NOW()),
    (7, 2, 'GameZone Studio', 'Games, instrumentos musicais, câmeras e áudio para criadores.', NOW()),
    (8, 3, 'Mundo Kids', 'Bebês, brinquedos e artigos de festa para a criançada.', NOW());
 
 
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
 
INSERT INTO produto (cd_produto, cd_loja_virtual, cd_categoria, nm_produto, ds_produto, vl_produto, qt_estoque, dt_criacao, nm_url_imagem, cd_public_id_imagem) VALUES
    (1, 1, 2, 'Notebook Ultrafino 14"', 'Intel Core i5, 16 GB de RAM, SSD de 512 GB', 3899.90, 15, NOW(),'https://res.cloudinary.com/nfky6va1/image/upload/v1793285671/produtos/nk5h9o5conqmnrmz5zhh.webp', 'produtos/nk5h9o5conqmnrmz5zhh'),
    (2, 1, 2, 'Mouse Sem Fio Ergonômico', 'Conexão 2.4 GHz, 6 botões, bateria de longa duração', 89.90, 120, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293630/produtos/nttsdv2he7qwxeom9ugb.webp', 'produtos/nttsdv2he7qwxeom9ugb'),
    (3, 1, 2, 'Teclado Mecânico RGB', 'Switch azul, layout ABNT2, iluminação RGB', 249.90, 60, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293637/produtos/tyhhz4fnnbhgu9rix0ou.webp', 'produtos/tyhhz4fnnbhgu9rix0ou'),
    (4, 1, 3, 'Smartphone 128 GB', 'Tela de 6,5", câmera tripla e 8 GB de RAM', 1799.00, 40, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293643/produtos/shgx4eemmsmbqkuqiiss.webp', 'produtos/shgx4eemmsmbqkuqiiss'),
    (5, 1, 1, 'Fone de Ouvido Bluetooth', 'Cancelamento de ruído e até 30 h de bateria', 199.90, 80, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293649/produtos/hx7g0hoadgmfg9ipo6d1.webp', 'produtos/hx7g0hoadgmfg9ipo6d1'),
    (6, 1, 3, 'Carregador USB-C 65W', 'Carregamento rápido para notebooks e celulares', 119.90, 100, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793294451/produtos/y8jne6hr9yzsk8ahxyul.webp', 'produtos/y8jne6hr9yzsk8ahxyul'),
    (7, 1, 1, 'Smart TV 50" 4K', 'Painel LED, HDR, Wi-Fi e aplicativos de streaming', 2599.00, 10, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293871/produtos/q0iukhnqmgzchunsywou.webp', 'produtos/q0iukhnqmgzchunsywou'),
    (8, 1, 1, 'Caixa de Som Portátil', 'Resistente à água, Bluetooth e 12 h de bateria', 159.90, 55, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293878/produtos/u1b2uhjfzb0k6h1nmkqd.webp', 'produtos/u1b2uhjfzb0k6h1nmkqd'),
    (9, 2, 5, 'Camiseta Básica Algodão', 'Camiseta 100% algodão, disponível em várias cores', 39.90, 200, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793294435/produtos/xybg5qbwzcvrdfxq5r5g.webp', 'produtos/xybg5qbwzcvrdfxq5r5g'),
    (10, 2, 5, 'Calça Jeans Slim', 'Jeans com elastano e corte slim', 129.90, 90, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293890/produtos/seickjco6r2oqrevxkeb.webp', 'produtos/z0by56or3qjhmxjdyept'),
    (11, 2, 9, 'Tênis Esportivo Running', 'Amortecimento reforçado para corrida', 299.90, 45, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293899/produtos/zzngwpfa0a5lxk5th1qf.webp', 'produtos/zzngwpfa0a5lxk5th1qf'),
    (12, 2, 4, 'Conjunto de Panelas Antiaderentes', 'Kit com 5 peças e revestimento antiaderente', 349.90, 20, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293908/produtos/i8huzltwo0ag7891ngne.webp', 'produtos/i8huzltwo0ag7891ngne'),
    (13, 2, 7, 'Perfume Floral 100 ml', 'Fragrância floral, eau de parfum', 219.90, 35, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293914/produtos/xsaf2c7pwe6fedew3vy9.webp', 'produtos/xsaf2c7pwe6fedew3vy9'),
    (14, 2, 16, 'Blocos de Montar 500 peças', 'Indicado para crianças a partir de 6 anos', 119.90, 60, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293921/produtos/iay7qcvb1m9asmcprepy.webp', 'produtos/iay7qcvb1m9asmcprepy'),
    (15, 2, 10, 'Livro Clean Code', 'Guia de boas práticas para escrever código limpo', 89.90, 30, NOW(), 'https://res.cloudinary.com/nfky6va1/image/upload/v1793293927/produtos/hjwq4ea4r6dlsfu4rvhm.webp', 'produtos/hjwq4ea4r6dlsfu4rvhm'),
    
    (16, 1, 1,  'Soundbar 2.1 Bluetooth 120W', 'Subwoofer sem fio, HDMI ARC e entrada óptica', 699.90, 25, DATE_SUB(NOW(), INTERVAL 3 DAY), NULL, NULL),
    (17, 7, 1,  'Fone de Ouvido Gamer 7.1', 'Som surround virtual, microfone removível e almofadas macias', 229.90, 70, DATE_SUB(NOW(), INTERVAL 12 DAY), NULL, NULL),
    (18, 1, 2,  'Monitor 24" Full HD', 'Painel IPS, 75 Hz, entradas HDMI e VGA', 749.00, 30, DATE_SUB(NOW(), INTERVAL 20 DAY), NULL, NULL),
    (19, 1, 2,  'Webcam Full HD 1080p', 'Microfone embutido e correção automática de luz', 189.90, 0, DATE_SUB(NOW(), INTERVAL 45 DAY), NULL, NULL),
    (20, 1, 3,  'Smartphone 256 GB 5G', 'Tela AMOLED de 6,7", câmera de 108 MP e 12 GB de RAM', 2899.00, 18, DATE_SUB(NOW(), INTERVAL 2 DAY), NULL, NULL),
    (21, 1, 3,  'Capa Anti-impacto para Smartphone', 'Silicone reforçado com bordas elevadas', 39.90, 300, DATE_SUB(NOW(), INTERVAL 60 DAY), NULL, NULL),
    (22, 4, 4,  'Air Fryer 5 L Digital', 'Cesto antiaderente, 8 predefinições e desligamento automático', 399.90, 35, DATE_SUB(NOW(), INTERVAL 5 DAY), NULL, NULL),
    (23, 4, 4,  'Aspirador de Pó Vertical Sem Fio', 'Bateria com 40 minutos de autonomia e filtro HEPA', 549.90, 22, DATE_SUB(NOW(), INTERVAL 15 DAY), NULL, NULL),
    (24, 2, 4,  'Liquidificador 1200W', 'Jarra de vidro de 2 L e 5 velocidades', 189.90, 50, DATE_SUB(NOW(), INTERVAL 30 DAY), NULL, NULL),
    (25, 2, 5,  'Bolsa Tote Couro Sintético', 'Alça dupla, forro interno e bolso com zíper', 159.90, 40, DATE_SUB(NOW(), INTERVAL 8 DAY), NULL, NULL),
    (26, 2, 5,  'Jaqueta Corta-Vento Unissex', 'Tecido impermeável leve com capuz ajustável', 219.90, 65, DATE_SUB(NOW(), INTERVAL 25 DAY), NULL, NULL),
    (27, 4, 6,  'Luminária de Mesa LED', 'Três níveis de brilho e braço articulado', 99.90, 85, DATE_SUB(NOW(), INTERVAL 6 DAY), NULL, NULL),
    (28, 4, 6,  'Conjunto de Almofadas Decorativas 4 un.', 'Capas com zíper em tecido macio', 129.90, 60, DATE_SUB(NOW(), INTERVAL 40 DAY), NULL, NULL),
    (29, 2, 6,  'Vaso Decorativo de Cerâmica', 'Acabamento fosco, ideal para plantas pequenas', 69.90, 75, DATE_SUB(NOW(), INTERVAL 18 DAY), NULL, NULL),
    (30, 2, 7,  'Kit Skincare Hidratante Facial', 'Sabonete, tônico e creme com ácido hialurônico', 179.90, 40, DATE_SUB(NOW(), INTERVAL 4 DAY), NULL, NULL),
    (31, 2, 7,  'Secador de Cabelo 2200W', 'Íons, três temperaturas e bocal concentrador', 249.90, 28, DATE_SUB(NOW(), INTERVAL 35 DAY), NULL, NULL),
    (32, 6, 8,  'Café Especial em Grãos 1 kg', 'Notas de chocolate e caramelo, torra média', 79.90, 100, DATE_SUB(NOW(), INTERVAL 7 DAY), NULL, NULL),
    (33, 2, 8,  'Kit Barras de Cereal Integral 24 un.', 'Sem conservantes, sabores variados', 54.90, 150, DATE_SUB(NOW(), INTERVAL 55 DAY), NULL, NULL),
    (34, 2, 9,  'Bicicleta Aro 29 com 21 Marchas', 'Quadro de alumínio e freios a disco', 1899.00, 8, DATE_SUB(NOW(), INTERVAL 10 DAY), NULL, NULL),
    (35, 2, 9,  'Kit Halteres Ajustáveis 20 kg', 'Anilhas emborrachadas e barra com trava', 289.90, 0, DATE_SUB(NOW(), INTERVAL 70 DAY), NULL, NULL),
    (36, 2, 10, 'Caderno Universitário 10 Matérias', 'Capa dura e 160 folhas pautadas', 34.90, 220, DATE_SUB(NOW(), INTERVAL 14 DAY), NULL, NULL),
    (37, 2, 10, 'Livro O Programador Pragmático', 'Dicas práticas para evoluir na carreira de desenvolvedor', 99.90, 25, DATE_SUB(NOW(), INTERVAL 22 DAY), NULL, NULL),
    (38, 5, 11, 'Pneu Aro 15 185/65', 'Alta durabilidade e baixo ruído', 389.90, 48, DATE_SUB(NOW(), INTERVAL 9 DAY), NULL, NULL),
    (39, 5, 11, 'Bateria Automotiva 60 Ah', 'Livre de manutenção e 18 meses de garantia', 479.90, 20, DATE_SUB(NOW(), INTERVAL 28 DAY), NULL, NULL),
    (40, 5, 11, 'Kit de Limpeza Automotiva Completo', 'Shampoo, cera, flanelas e pretinho para pneus', 119.90, 90, DATE_SUB(NOW(), INTERVAL 45 DAY), NULL, NULL),
    (41, 6, 12, 'Sal Mineral para Bovinos 25 kg', 'Suplemento com fósforo e microminerais', 139.90, 60, DATE_SUB(NOW(), INTERVAL 33 DAY), NULL, NULL),
    (42, 6, 12, 'Pulverizador Costal Manual 20 L', 'Tanque resistente e lança regulável', 229.90, 25, DATE_SUB(NOW(), INTERVAL 50 DAY), NULL, NULL),
    (43, 6, 13, 'Ração Premium para Cães Adultos 10 kg', 'Frango e arroz, sem corantes artificiais', 189.90, 70, DATE_SUB(NOW(), INTERVAL 1 DAY), NULL, NULL),
    (44, 6, 13, 'Caminha Pet Confortável Tamanho M', 'Capa removível e base antiderrapante', 129.90, 40, DATE_SUB(NOW(), INTERVAL 21 DAY), NULL, NULL),
    (45, 6, 13, 'Arranhador para Gatos com Torre', 'Sisal natural e plataformas acolchoadas', 159.90, 30, DATE_SUB(NOW(), INTERVAL 65 DAY), NULL, NULL),
    (46, 2, 14, 'Rádio Retrô Decorativo Restaurado', 'Peça de colecionador com Bluetooth adaptado', 449.00, 3, DATE_SUB(NOW(), INTERVAL 90 DAY), NULL, NULL),
    (47, 8, 15, 'Carrinho de Bebê Dobrável', 'Reclinável, com capota e cinto de cinco pontos', 899.90, 12, DATE_SUB(NOW(), INTERVAL 11 DAY), NULL, NULL),
    (48, 8, 15, 'Kit de Mamadeiras Anticólica 3 un.', 'Bico de silicone e válvula antivácuo', 89.90, 80, DATE_SUB(NOW(), INTERVAL 27 DAY), NULL, NULL),
    (49, 8, 16, 'Pista de Carrinhos com Looping', 'Acompanha 4 carrinhos e 120 peças', 169.90, 38, DATE_SUB(NOW(), INTERVAL 16 DAY), NULL, NULL),
    (50, 8, 16, 'Boneca Interativa com Acessórios', 'Fala frases e acompanha mamadeira', 139.90, 45, DATE_SUB(NOW(), INTERVAL 38 DAY), NULL, NULL),
    (51, 7, 17, 'Câmera Digital Mirrorless 24 MP', 'Vídeo 4K, Wi-Fi e lente 16-50 mm', 4899.00, 6, DATE_SUB(NOW(), INTERVAL 13 DAY), NULL, NULL),
    (52, 1, 17, 'Tripé Flexível para Câmera e Celular', 'Pernas articuladas e suporte com rosca universal', 69.90, 110, DATE_SUB(NOW(), INTERVAL 42 DAY), NULL, NULL),
    (53, 4, 18, 'Furadeira de Impacto 650W', 'Velocidade variável e mandril de 13 mm', 249.90, 33, DATE_SUB(NOW(), INTERVAL 19 DAY), NULL, NULL),
    (54, 4, 18, 'Tinta Acrílica Fosca Branca 18 L', 'Alta cobertura e ótimo rendimento', 329.90, 24, DATE_SUB(NOW(), INTERVAL 52 DAY), NULL, NULL),
    (55, 5, 19, 'Jogo de Chaves Combinadas 12 Peças', 'Aço cromo-vanádio com estojo', 109.90, 55, DATE_SUB(NOW(), INTERVAL 31 DAY), NULL, NULL),
    (56, 4, 19, 'Caixa de Ferramentas com 129 Peças', 'Maleta resistente com soquetes e alicates', 299.90, 28, DATE_SUB(NOW(), INTERVAL 26 DAY), NULL, NULL),
    (57, 8, 20, 'Kit Decoração de Aniversário', 'Balões, faixa, toalha e pratos descartáveis para 12 pessoas', 74.90, 95, DATE_SUB(NOW(), INTERVAL 17 DAY), NULL, NULL),
    (58, 7, 21, 'Console Portátil Retrô 5000 Jogos', 'Tela de 4,3", saída para TV e dois controles', 349.90, 42, DATE_SUB(NOW(), INTERVAL 23 DAY), NULL, NULL),
    (59, 7, 21, 'Controle Gamer Sem Fio', 'Bluetooth, vibração e bateria recarregável', 219.90, 65, DATE_SUB(NOW(), INTERVAL 4 DAY), NULL, NULL),
    (60, 7, 22, 'Violão Acústico de Aço', 'Tampo de spruce, braço de mogno e afinador integrado', 549.90, 15, DATE_SUB(NOW(), INTERVAL 36 DAY), NULL, NULL),
    (61, 7, 22, 'Teclado Musical 61 Teclas', 'Sensível ao toque, 300 timbres e entrada USB', 899.90, 11, DATE_SUB(NOW(), INTERVAL 48 DAY), NULL, NULL),
    (62, 2, 23, 'Relógio Analógico Masculino', 'Caixa de aço inox e resistência à água de 50 m', 329.90, 26, DATE_SUB(NOW(), INTERVAL 29 DAY), NULL, NULL),
    (63, 2, 23, 'Colar com Pingente em Prata 925', 'Corrente veneziana de 45 cm', 189.90, 0, DATE_SUB(NOW(), INTERVAL 75 DAY), NULL, NULL),
    (64, 2, 24, 'Termômetro Digital Infravermelho', 'Medição sem contato em 1 segundo', 99.90, 85, DATE_SUB(NOW(), INTERVAL 2 DAY), NULL, NULL),
    (65, 2, 24, 'Medidor de Pressão Arterial de Pulso', 'Memória para 90 leituras e alerta de arritmia', 159.90, 40, DATE_SUB(NOW(), INTERVAL 44 DAY), NULL, NULL);
 
INSERT INTO cupom (cd_cupom, cd_promocional, sg_tipo_desconto, vl_desconto, dt_inicio_validade, dt_fim_validade) VALUES
    (1, 'BEMVINDO10',  'P', 10.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY)),
    (2, 'DESCONTO30',  'F', 30.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY)),
    (3, 'VERAO15',     'P', 15.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY)),
    (4, 'PASCOA15',    'P', 15.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY)),
    (5, 'BLACKWEEK20', 'P', 20.00, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY));
 
-- Pedidos (status: P=Pendente, A=Aprovado, E=Enviado, T=Entregue, C=Cancelado)
INSERT INTO pedido (cd_pedido, cd_usuario, cd_loja_virtual, cd_cupom, cd_endereco_entrega, sg_status_pedido, vl_subtotal_pedido, vl_desconto_pedido, vl_total_pedido, dt_criacao_pedido) VALUES
    (1,  4, 1, NULL, 3, 'T',  429.70,   0.00,  429.70, NOW()),
    (2,  4, 1, 1,    1, 'E', 1918.90, 191.89, 1727.01, NOW()),
    (3,  4, 2, 2,    1, 'P',  419.60,  30.00,  389.60, NOW()),
    (4,  4, 1, NULL, 1, 'E', 3899.90,   0.00, 3899.90, NOW()),
    (5,  5, 1, 3,    4, 'T', 3598.00, 539.70, 3058.30, NOW()),
    (6,  6, 2, NULL, 5, 'P',  299.90,   0.00,  299.90, NOW()),
    (7,  7, 1, 1,    6, 'E',  409.80,  40.98,  368.82, NOW()),
    (8,  8, 2, 5,    7, 'T',  479.80,  95.96,  383.84, NOW()),
    (9,  9, 1, NULL, 8, 'P', 2798.90,   0.00, 2798.90, NOW()),
    (10, 5, 2, 4,    4, 'E',  349.90,  52.49,  297.41, NOW()),
    (11, 6, 1, NULL, 5, 'T',  319.80,   0.00,  319.80, NOW()),
    (12, 7, 2, 1,    6, 'P',  259.80,  25.98,  233.82, NOW()),
    (13, 8, 1, 2,    7, 'E',  199.90,  30.00,  169.90, NOW()),
    (14, 9, 2, NULL, 8, 'T',  429.70,   0.00,  429.70, NOW()),
    (15, 4, 1, 3,    1, 'P',  209.80,  31.47,  178.33, NOW()),
    (16, 5, 2, NULL, 4, 'E',  249.80,   0.00,  249.80, NOW()),
    (17, 6, 1, 5,    5, 'T', 4099.80, 819.96, 3279.84, NOW()),
    (18, 7, 2, 4,    6, 'P',  519.80,  77.97,  441.83, NOW()),
    (19, 8, 1, NULL, 7, 'E',  249.90,   0.00,  249.90, NOW()),
    (20, 9, 2, 2,    8, 'T',  419.60,  30.00,  389.60, NOW()),
    (21, 4, 1, NULL, 1, 'P',  499.80,   0.00,  499.80, NOW()),
    (22, 5, 2, 3,    4, 'E',  509.70,  76.46,  433.24, NOW()),
    (23, 6, 1, NULL, 5, 'T', 2599.00,   0.00, 2599.00, NOW()),
    
    (24, 4, 1, NULL, 1, 'P',  409.70,   0.00,  409.70, NOW()),
    (25, 4, 2, 1,    2, 'P',  399.70,  39.97,  359.73, NOW()),
    (26, 4, 1, 2,    1, 'P', 3158.60,  30.00, 3128.60, NOW()),
    (27, 4, 2, NULL, 2, 'P',  319.40,   0.00,  319.40, NOW()),

    (28, 4, 1, 3,    1, 'A', 3989.80, 598.47, 3391.33, NOW()),
    (29, 4, 2, NULL, 2, 'A',  639.50,   0.00,  639.50, NOW()),
    (30, 4, 1, 5,    1, 'A', 2368.70, 473.74, 1894.96, NOW()),
    (31, 4, 2, 2,    2, 'A',  569.80,  30.00,  539.80, NOW()),
    (32, 4, 1, NULL, 1, 'A',  239.80,   0.00,  239.80, NOW()),
    (33, 4, 2, 4,    2, 'A',  609.20,  91.38,  517.82, NOW()),
    (34, 4, 1, 1,    1, 'A',  829.40,  82.94,  746.46, NOW()),

    (35, 4, 2, NULL, 2, 'E',  729.70,   0.00,  729.70, NOW()),
    (36, 4, 1, 1,    2, 'E', 2848.90, 284.89, 2564.01, NOW()),
    (37, 4, 2, 2,    1, 'E',  819.50,  30.00,  789.50, NOW()),
    (38, 4, 1, NULL, 2, 'E', 4219.70,   0.00, 4219.70, NOW()),
    (39, 4, 2, 5,    1, 'E',  769.50, 153.90,  615.60, NOW()),

    (40, 4, 1, NULL, 3, 'T',  359.60,   0.00,  359.60, NOW()),
    (41, 4, 2, 4,    1, 'T',  689.60, 103.44,  586.16, NOW()),
    (42, 4, 1, 1,    3, 'T', 2078.80, 207.88, 1870.92, NOW()),
    (43, 4, 2, NULL, 2, 'T',  539.10,   0.00,  539.10, NOW()),
    (44, 4, 1, 2,    1, 'T',  699.70,  30.00,  669.70, NOW()),
    (45, 4, 2, 1,    2, 'T',  869.70,  86.97,  782.73, NOW()),

    (46, 4, 1, NULL, 1, 'C', 2599.00,   0.00, 2599.00, NOW()),
    (47, 4, 2, 2,    2, 'C',  299.60,  30.00,  269.60, NOW()),
    (48, 4, 1, 5,    1, 'C', 3899.90, 779.98, 3119.92, NOW()),
    (49, 4, 2, NULL, 2, 'C',  699.50,   0.00,  699.50, NOW()),
    (50, 4, 1, 4,    1, 'C', 1029.40, 154.41,  874.99, NOW()),
    (51, 4, 2, 1,    2, 'C',  949.70,  94.97,  854.73, NOW()),
    (52, 4, 1, NULL, 1, 'C',  719.50,   0.00,  719.50, NOW());
 
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
    (38, 23, 7, 1, 2599.00),
    
    -- Pedido 24 (loja 1): 2 x 159.90 + 89.90 = 409.70
    (39, 24, 8, 2,  159.90),
    (40, 24, 2, 1,   89.90),
    -- Pedido 25 (loja 2): 219.90 + 2 x 89.90 = 399.70
    (41, 25, 13, 1, 219.90),
    (42, 25, 15, 2,  89.90),
    -- Pedido 26 (loja 1): 2599.00 + 3 x 119.90 + 199.90 = 3158.60
    (43, 26, 7, 1, 2599.00),
    (44, 26, 6, 3,  119.90),
    (45, 26, 5, 1,  199.90),
    -- Pedido 27 (loja 2): 5 x 39.90 + 119.90 = 319.40
    (46, 27, 9, 5,   39.90),
    (47, 27, 14, 1, 119.90),
 
    -- Pedido 28 (loja 1): 3899.90 + 89.90 = 3989.80
    (48, 28, 1, 1, 3899.90),
    (49, 28, 2, 1,   89.90),
    -- Pedido 29 (loja 2): 2 x 129.90 + 299.90 + 2 x 39.90 = 639.50
    (50, 29, 10, 2, 129.90),
    (51, 29, 11, 1, 299.90),
    (52, 29, 9, 2,   39.90),
    -- Pedido 30 (loja 1): 1799.00 + 2 x 159.90 + 249.90 = 2368.70
    (53, 30, 4, 1, 1799.00),
    (54, 30, 8, 2,  159.90),
    (55, 30, 3, 1,  249.90),
    -- Pedido 31 (loja 2): 349.90 + 219.90 = 569.80
    (56, 31, 12, 1, 349.90),
    (57, 31, 13, 1, 219.90),
    -- Pedido 32 (loja 1): 2 x 119.90 = 239.80
    (58, 32, 6, 2,  119.90),
    -- Pedido 33 (loja 2): 3 x 119.90 + 89.90 + 4 x 39.90 = 609.20
    (59, 33, 14, 3, 119.90),
    (60, 33, 15, 1,  89.90),
    (61, 33, 9, 4,   39.90),
    -- Pedido 34 (loja 1): 2 x 199.90 + 3 x 89.90 + 159.90 = 829.40
    (62, 34, 5, 2,  199.90),
    (63, 34, 2, 3,   89.90),
    (64, 34, 8, 1,  159.90),
 
    -- Pedido 35 (loja 2): 2 x 299.90 + 129.90 = 729.70
    (65, 35, 11, 2, 299.90),
    (66, 35, 10, 1, 129.90),
    -- Pedido 36 (loja 1): 2599.00 + 249.90 = 2848.90
    (67, 36, 7, 1, 2599.00),
    (68, 36, 3, 1,  249.90),
    -- Pedido 37 (loja 2): 2 x 349.90 + 3 x 39.90 = 819.50
    (69, 37, 12, 2, 349.90),
    (70, 37, 9, 3,   39.90),
    -- Pedido 38 (loja 1): 3899.90 + 199.90 + 119.90 = 4219.70
    (71, 38, 1, 1, 3899.90),
    (72, 38, 5, 1,  199.90),
    (73, 38, 6, 1,  119.90),
    -- Pedido 39 (loja 2): 2 x 219.90 + 2 x 119.90 + 89.90 = 769.50
    (74, 39, 13, 2, 219.90),
    (75, 39, 14, 2, 119.90),
    (76, 39, 15, 1,  89.90),
 
    -- Pedido 40 (loja 1): 4 x 89.90 = 359.60
    (77, 40, 2, 4,   89.90),
    -- Pedido 41 (loja 2): 3 x 129.90 + 299.90 = 689.60
    (78, 41, 10, 3, 129.90),
    (79, 41, 11, 1, 299.90),
    -- Pedido 42 (loja 1): 1799.00 + 119.90 + 159.90 = 2078.80
    (80, 42, 4, 1, 1799.00),
    (81, 42, 6, 1,  119.90),
    (82, 42, 8, 1,  159.90),
    -- Pedido 43 (loja 2): 6 x 39.90 + 119.90 + 2 x 89.90 = 539.10
    (83, 43, 9, 6,   39.90),
    (84, 43, 14, 1, 119.90),
    (85, 43, 15, 2,  89.90),
    -- Pedido 44 (loja 1): 2 x 249.90 + 199.90 = 699.70
    (86, 44, 3, 2,  249.90),
    (87, 44, 5, 1,  199.90),
    -- Pedido 45 (loja 2): 349.90 + 219.90 + 299.90 = 869.70
    (88, 45, 12, 1, 349.90),
    (89, 45, 13, 1, 219.90),
    (90, 45, 11, 1, 299.90),
 
    -- Pedido 46 (loja 1): 2599.00
    (91, 46, 7, 1, 2599.00),
    -- Pedido 47 (loja 2): 2 x 39.90 + 129.90 + 89.90 = 299.60
    (92, 47, 9, 2,   39.90),
    (93, 47, 10, 1, 129.90),
    (94, 47, 15, 1,  89.90),
    -- Pedido 48 (loja 1): 3899.90
    (95, 48, 1, 1, 3899.90),
    -- Pedido 49 (loja 2): 4 x 119.90 + 219.90 = 699.50
    (96, 49, 14, 4, 119.90),
    (97, 49, 13, 1, 219.90),
    -- Pedido 50 (loja 1): 2 x 89.90 + 249.90 + 3 x 199.90 = 1029.40
    (98, 50, 2, 2,   89.90),
    (99, 50, 3, 1,  249.90),
    (100, 50, 5, 3, 199.90),
    -- Pedido 51 (loja 2): 2 x 299.90 + 349.90 = 949.70
    (101, 51, 11, 2, 299.90),
    (102, 51, 12, 1, 349.90),
    -- Pedido 52 (loja 1): 3 x 159.90 + 2 x 119.90 = 719.50
    (103, 52, 8, 3, 159.90),
    (104, 52, 6, 2, 119.90);

SELECT * FROM usuario;

SELECT * FROM endereco;

-- Pedidos com o endereço de entrega
SELECT pe.cd_pedido, u.nm_usuario, e.nm_identificacao, e.nm_logradouro, e.nm_numero, e.nm_cidade, e.sg_uf
FROM pedido pe
JOIN usuario u  ON u.cd_usuario  = pe.cd_usuario
JOIN endereco e ON e.cd_endereco = pe.cd_endereco_entrega
ORDER BY pe.cd_pedido;

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

SELECT COUNT(*) FROM pedido WHERE cd_usuario = 4;

SELECT
    cd_pedido,
    cd_usuario,
    dt_criacao_pedido,
    sg_status_pedido
FROM pedido
WHERE cd_usuario = 4
ORDER BY dt_criacao_pedido DESC, cd_pedido DESC;

SELECT
        p.cd_pedido,
        p.cd_usuario,
        p.dt_criacao_pedido,
        p.sg_status_pedido,

        lv.cd_loja_virtual,
        lv.nm_loja_virtual,

        p.vl_subtotal_pedido,
        p.vl_desconto_pedido,
        p.vl_total_pedido,

        c.cd_promocional,

        ip.cd_produto,
        pr.nm_produto,
        ip.qt_item,
        ip.vl_preco_unitario,

        (ip.qt_item * ip.vl_preco_unitario) AS vl_total_item

    FROM pedido p

    INNER JOIN loja_virtual lv
        ON lv.cd_loja_virtual = p.cd_loja_virtual

    LEFT JOIN cupom c
        ON c.cd_cupom = p.cd_cupom

    LEFT JOIN item_pedido ip
        ON ip.cd_pedido = p.cd_pedido

    LEFT JOIN produto pr
        ON pr.cd_produto = ip.cd_produto

    WHERE p.cd_pedido = (
        SELECT p2.cd_pedido
        FROM pedido p2
        WHERE p2.cd_usuario = 4
        ORDER BY
            p2.dt_criacao_pedido DESC,
            p2.cd_pedido DESC
        LIMIT 1
    )

    ORDER BY ip.cd_produto;
    
    
    SELECT
    p.cd_produto,
    p.nm_produto,
    p.vl_produto,
    l.nm_loja_virtual,
    SUM(i.qt_item)              AS qt_total_vendida,
    COUNT(DISTINCT i.cd_pedido) AS qt_pedidos,
    SUM(i.qt_item * i.vl_preco_unitario) AS vl_total_vendido
FROM item_pedido i
JOIN pedido       pe ON pe.cd_pedido       = i.cd_pedido
JOIN produto      p  ON p.cd_produto       = i.cd_produto
JOIN loja_virtual l  ON l.cd_loja_virtual  = p.cd_loja_virtual
WHERE pe.sg_status_pedido <> 'C'
  AND p.ic_produto_ativo = TRUE
  AND l.ic_loja_ativa    = TRUE
GROUP BY p.cd_produto, p.nm_produto, p.vl_produto, l.nm_loja_virtual
ORDER BY qt_total_vendida DESC, p.cd_produto
LIMIT 15;


SELECT
    p.cd_produto,
    p.nm_produto,
    p.vl_produto,
    p.dt_criacao,
    l.nm_loja_virtual
FROM produto p
INNER JOIN loja_virtual l
    ON l.cd_loja_virtual = p.cd_loja_virtual
WHERE p.ic_produto_ativo = TRUE
  AND l.ic_loja_ativa = TRUE
ORDER BY
    p.dt_criacao DESC,
    p.cd_produto DESC
LIMIT 15;

SELECT cd_produto, nm_url_imagem, cd_public_id_imagem FROM produto where cd_produto = 9;
