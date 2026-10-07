DROP SCHEMA IF EXISTS db_ecommerce;
CREATE DATABASE IF NOT EXISTS db_ecommerce
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;
USE db_ecommerce;

CREATE TABLE tipo_usuario 
(
    cd_tipo_usuario  INT NOT NULL AUTO_INCREMENT,
    nm_tipo_usuario  VARCHAR(50)  NOT NULL,
    
    CONSTRAINT pk_tipo_usuario PRIMARY KEY (cd_tipo_usuario),
    CONSTRAINT uq_tipo_usuario_nome UNIQUE (nm_tipo_usuario)
) ENGINE=InnoDB;


CREATE TABLE usuario 
(
    cd_usuario  INT NOT NULL AUTO_INCREMENT,
    nm_apelido  VARCHAR(50) NOT NULL,
    nm_usuario  VARCHAR(150) NOT NULL,
    nm_telefone VARCHAR(11) NULL,
    nm_email  VARCHAR(180) NOT NULL,
    nm_senha  VARCHAR(255) NOT NULL,
    cd_tipo_usuario  INT NOT NULL,
    dt_criacao  DATETIME NOT NULL,
    dt_atualizacao DATETIME,
    ic_usuario_ativo BOOL NOT NULL,

    CONSTRAINT pk_usuario PRIMARY KEY (cd_usuario),
    CONSTRAINT uq_usuario_apelido UNIQUE (nm_apelido),
    CONSTRAINT uq_usuario_email   UNIQUE (nm_email),
    CONSTRAINT uq_usuario_telefone   UNIQUE (nm_telefone),
    
    CONSTRAINT fk_usuario_tipo_usuario
        FOREIGN KEY (cd_tipo_usuario) REFERENCES tipo_usuario (cd_tipo_usuario)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB;


CREATE TABLE loja_virtual 
(
    cd_loja_virtual  INT NOT NULL AUTO_INCREMENT,
    cd_usuario  INT NOT NULL,
    nm_loja_virtual  VARCHAR(200)  NOT NULL,
    ds_loja_virtual  TEXT NULL,
    ic_loja_ativa  BOOLEAN NOT NULL DEFAULT TRUE,
    dt_criacao  DATETIME NOT NULL,

    CONSTRAINT pk_loja_virtual PRIMARY KEY (cd_loja_virtual),
    CONSTRAINT uq_loja_virtual_nome UNIQUE (nm_loja_virtual),
    
    CONSTRAINT fk_loja_virtual_usuario
        FOREIGN KEY (cd_usuario) REFERENCES usuario (cd_usuario)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB;


CREATE TABLE categoria
(
    cd_categoria  INT  NOT NULL AUTO_INCREMENT,
    nm_categoria  VARCHAR(200) NOT NULL,

    CONSTRAINT  pk_categoria PRIMARY KEY (cd_categoria),
    CONSTRAINT uq_categoria_nome UNIQUE (nm_categoria)
) ENGINE=InnoDB;


CREATE TABLE produto
(
    cd_produto  INT NOT NULL AUTO_INCREMENT,
    cd_loja_virtual  INT NOT NULL,
    cd_categoria  INT NOT NULL,
    nm_produto  VARCHAR(200) NOT NULL,
    ds_produto  TEXT NULL,
    vl_produto  DECIMAL(10,2) NOT NULL,
    qt_estoque  INT NOT NULL DEFAULT 0,
    ic_produto_ativo  BOOLEAN NOT NULL DEFAULT TRUE,
    dt_criacao  DATETIME NOT NULL,
    dt_atualizacao  DATETIME NULL,

    CONSTRAINT  pk_produto PRIMARY KEY (cd_produto),
    CONSTRAINT ck_produto_vl_produto CHECK (vl_produto >= 0),
    CONSTRAINT ck_produto_qt_estoque CHECK (qt_estoque >= 0),

    CONSTRAINT fk_produto_loja_virtual
        FOREIGN KEY (cd_loja_virtual) REFERENCES loja_virtual (cd_loja_virtual)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_produto_categoria
        FOREIGN KEY (cd_categoria) REFERENCES categoria (cd_categoria)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB;


CREATE TABLE cupom 
(
    cd_cupom  INT NOT NULL AUTO_INCREMENT,
    cd_promocional  VARCHAR(45) NOT NULL,
    sg_tipo_desconto  CHAR(1) NOT NULL,
    vl_desconto  DECIMAL(10,2) NOT NULL,
    dt_inicio_validade  DATETIME NOT NULL,
    dt_fim_validade  DATETIME NOT NULL,
    ic_cupom_ativo  BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT pk_cupom PRIMARY KEY (cd_cupom),
    CONSTRAINT uq_cupom_promocional UNIQUE (cd_promocional),
    CONSTRAINT ck_cupom_tipo_desconto CHECK (sg_tipo_desconto IN ('P', 'F')),
    CONSTRAINT ck_cupom_vl_desconto CHECK (vl_desconto > 0),
    CONSTRAINT ck_cupom_percentual_max CHECK (sg_tipo_desconto = 'F' OR vl_desconto <= 100),
    CONSTRAINT ck_cupom_validade CHECK (dt_fim_validade > dt_inicio_validade)
) ENGINE=InnoDB;


CREATE TABLE pedido 
(
    cd_pedido  INT NOT NULL AUTO_INCREMENT,
    cd_usuario  INT NOT NULL,
    cd_loja_virtual  INT NOT NULL,
    cd_cupom  INT NULL,
    sg_status_pedido  CHAR(1) NOT NULL DEFAULT 'P',
    vl_subtotal_pedido  DECIMAL(10,2) NOT NULL DEFAULT 0,
    vl_desconto_pedido  DECIMAL(10,2) NOT NULL DEFAULT 0,
    vl_total_pedido  DECIMAL(10,2) NOT NULL DEFAULT 0,
    dt_criacao_pedido  DATETIME NOT NULL,
    dt_atualizacao_pedido  DATETIME NULL,

    CONSTRAINT pk_pedido PRIMARY KEY (cd_pedido),
    CONSTRAINT ck_pedido_status CHECK (sg_status_pedido IN ('P', 'A', 'E', 'T', 'C')),
    CONSTRAINT ck_pedido_valores CHECK (vl_subtotal_pedido >= 0 AND vl_desconto_pedido >= 0 AND vl_total_pedido >= 0),

    CONSTRAINT fk_pedido_usuario
        FOREIGN KEY (cd_usuario) REFERENCES usuario (cd_usuario)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_pedido_loja_virtual
        FOREIGN KEY (cd_loja_virtual) REFERENCES loja_virtual (cd_loja_virtual)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_pedido_cupom
        FOREIGN KEY (cd_cupom) REFERENCES cupom (cd_cupom)
        ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB;


CREATE TABLE item_pedido
(
    cd_item_pedido  INT NOT NULL AUTO_INCREMENT,
    cd_pedido  INT NOT NULL,
    cd_produto  INT NOT NULL,
    qt_item  INT NOT NULL,
    vl_preco_unitario  DECIMAL(10,2) NOT NULL,

    CONSTRAINT pk_item_pedido PRIMARY KEY (cd_item_pedido),
    CONSTRAINT uq_item_pedido_produto UNIQUE (cd_pedido, cd_produto),
    CONSTRAINT ck_item_pedido_qt CHECK (qt_item > 0),
    CONSTRAINT ck_item_pedido_preco CHECK (vl_preco_unitario >= 0),

    CONSTRAINT fk_item_pedido_pedido
        FOREIGN KEY (cd_pedido) REFERENCES pedido (cd_pedido)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_item_pedido_produto
        FOREIGN KEY (cd_produto) REFERENCES produto (cd_produto)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB;
