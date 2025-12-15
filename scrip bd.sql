CREATE DATABASE IF NOT EXISTS congreso_mercadotecnias; -- crea la base de datos si no existe

-- Usar la base de datos creada
USE congreso_mercadotecnias;

-- Crear la tabla de usuarios si no existe
CREATE TABLE IF NOT EXISTS usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo_registro VARCHAR(20) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido_paterno VARCHAR(100) NOT NULL,
    apellido_materno VARCHAR(100),
    carrera VARCHAR(100),
    semestre VARCHAR(10),
    id_institucional VARCHAR(50) UNIQUE,
    correo VARCHAR(100),
    contrasena VARCHAR(255) NOT NULL,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Crear la tabla de eventos si no existe
CREATE TABLE IF NOT EXISTS eventos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    lugar VARCHAR(100) NOT NULL,
    tipo_evento VARCHAR(50) NOT NULL,
    conferencista_responsable VARCHAR(100) NOT NULL,
    actividad TEXT NOT NULL,
    creado_en TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Crear la tabla de inscripciones si no existe
CREATE TABLE IF NOT EXISTS inscripciones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT NOT NULL,
    evento_id INT NOT NULL,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (evento_id) REFERENCES eventos(id)
);

-- Insertar cuatro usuarios alumnos en la tabla 'usuarios'

-- Alumno 1
INSERT INTO usuarios (tipo_registro, nombre, apellido_paterno, apellido_materno, carrera, semestre, id_institucional, correo, contrasena)
VALUES (
    'alumno',        -- tipo_registro
    'Ana',           -- nombre
    'García',        -- apellido_paterno
    'Pérez',         -- apellido_materno
    'Mercadotecnia', -- carrera
    '5',             -- semestre
    '785632',     -- id_institucional
    'al785632@edu.uaa.mx', -- correo
    '$2b$10$wLR1yfI2umHCZMNtt96ymO4JNY6DMaQ7ND4Kpv36Q6X9bo7wkdkSS'  -- contrasena (HASH ENCRIPTADO)
);

-- Alumno 2
INSERT INTO usuarios (tipo_registro, nombre, apellido_paterno, apellido_materno, carrera, semestre, id_institucional, correo, contrasena)
VALUES (
    'alumno',        -- tipo_registro
    'Luis',          -- nombre
    'Rodríguez',     -- apellido_paterno
    'Martínez',      -- apellido_materno
    'Comercio',      -- carrera
    '7',             -- semestre
    '548932',     -- id_institucional
    'al548932@edu.uaa.mx', -- correo
    '$2b$10$wLR1yfI2umHCZMNtt96ymO4JNY6DMaQ7ND4Kpv36Q6X9bo7wkdkSS'  -- contrasena (HASH ENCRIPTADO)
);

-- Alumno 3
INSERT INTO usuarios (tipo_registro, nombre, apellido_paterno, apellido_materno, carrera, semestre, id_institucional, correo, contrasena)
VALUES (
    'alumno',        -- tipo_registro
    'Sofía',         -- nombre
    'Hernández',     -- apellido_paterno
    'López',         -- apellido_materno
    'Administración',  -- carrera
    '3',             -- semestre
    '325478',     -- id_institucional
    'al325478@edu.uaa.mx', -- correo
    '$2b$10$wLR1yfI2umHCZMNtt96ymO4JNY6DMaQ7ND4Kpv36Q6X9bo7wkdkSS'  -- contrasena (HASH ENCRIPTADO)
);

-- Alumno 4
INSERT INTO usuarios (tipo_registro, nombre, apellido_paterno, apellido_materno, carrera, semestre, id_institucional, correo, contrasena)
VALUES (
    'alumno',        -- tipo_registro
    'Diego',         -- nombre
    'Sánchez',       -- apellido_paterno
    'Ramírez',       -- apellido_materno
    'Mercadotecnia',       -- carrera
    '1',             -- semestre
    '216578',     -- id_institucional
    'al216578@edu.uaa.mx', -- correo
    '$2b$10$wLR1yfI2umHCZMNtt96ymO4JNY6DMaQ7ND4Kpv36Q6X9bo7wkdkSS'  -- contrasena (HASH ENCRIPTADO)
);

select * from eventos; -- mostrar todos los eventos
select * from usuarios; -- mostrar todos los usuarios
select * from inscripciones; -- mostar todas las inscripciones