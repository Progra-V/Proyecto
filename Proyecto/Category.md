CREATE TABLE categorias
(
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    departamento_id BIGINT NOT NULL,

    nombre VARCHAR(100) NOT NULL,

    descripcion TEXT,

    activo BOOLEAN NOT NULL DEFAULT TRUE,

    fecha_creacion TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    fecha_actualizacion TIMESTAMPTZ,

    CONSTRAINT fk_categoria_departamento
        FOREIGN KEY (departamento_id)
        REFERENCES departamentos(id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);