IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

-- Crear la tabla Clientes
CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Contrasena] nvarchar(max) NOT NULL,
    [Estado] bit NOT NULL,
    [Nombre] nvarchar(max) NOT NULL,
    [Genero] nvarchar(max) NOT NULL,
    [Edad] int NOT NULL,
    [Identificacion] nvarchar(max) NOT NULL,
    [Direccion] nvarchar(max) NOT NULL,
    [Telefono] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);
GO

-- Crear la tabla Cuentas
CREATE TABLE [Cuentas] (
    [Id] int NOT NULL IDENTITY,
    [NumeroCuenta] nvarchar(max) NOT NULL,
    [TipoCuenta] nvarchar(max) NOT NULL,
    [SaldoInicial] decimal(18,2) NOT NULL,
    [Estado] bit NOT NULL,
    [ClienteId] int NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Cuentas] PRIMARY KEY ([Id])
);
GO

-- Crear la tabla Movimientos
CREATE TABLE [Movimientos] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [TipoMovimiento] nvarchar(max) NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [Saldo] decimal(18,2) NOT NULL,
    [CuentaId] int NOT NULL,
    CONSTRAINT [PK_Movimientos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Movimientos_Cuentas_CuentaId] FOREIGN KEY ([CuentaId]) REFERENCES [Cuentas] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Movimientos_CuentaId] ON [Movimientos] ([CuentaId]);
GO

-- Registrar las migraciones en __EFMigrationsHistory
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES 
    (N'20240903071628_InitialCreate', N'9.0.0-preview.7.24405.3'), 
    (N'20240903071740_InitialCreate', N'9.0.0-preview.7.24405.3'), 
    (N'20240903081651_AddClienteIdToCuenta', N'9.0.0-preview.7.24405.3'),
    (N'20240903090146_MovimientosNotRequired', N'9.0.0-preview.7.24405.3'),
    (N'20240903103745_CuentaRequired', N'9.0.0-preview.7.24405.3'),
    (N'20240903104205_CuentaRequired1', N'9.0.0-preview.7.24405.3'),
    (N'20240903104247_CuentaRequired2', N'9.0.0-preview.7.24405.3');
GO

COMMIT;
GO
