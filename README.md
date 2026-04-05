<img alt="AR Contenidos" src='https://firebasestorage.googleapis.com/v0/b/capacitechshop.appspot.com/o/IMAGENES_LOGO%2F768c4102b7f64dfa8d0570d79acccba2.jpeg?alt=media&token=4fb82046-4f6b-4cbe-807f-ffb9b0811911' width='48' height='48' style="object-fit:cover;border-radius:50%;border:1px solid #ddd;display:block;margin:auto;" />

# Sistema de Gestión Web

Aplicación web desarrollada con **ASP.NET Core / Blazor Server**, conectada a **SQL Server** mediante **Entity Framework Core**, siguiendo una arquitectura en capas orientada a mantenimiento, escalabilidad y buenas prácticas.

## Descripción

Este sistema fue construido para administrar distintos módulos del negocio, manteniendo separación de responsabilidades entre acceso a datos, lógica de negocio, configuración de dependencias y presentación.

Actualmente incluye funcionalidades como:

- Autenticación de **usuarios**.
- Control de acceso por **roles** y **permisos**.
- Gestión de **usuarios**.
- Gestión de **países**.
- Gestión de **estados**.
- Gestión de **proyectos**.
- Gestión de **archivos**.
- Recuperación de contraseña.
- Navegación **dinámica** por **menú** según **perfil**.

## Arquitectura

La solución sigue una arquitectura en capas con separación clara de responsabilidades:

- **DAL**: acceso a datos, contexto, entidades, repositorios y persistencia.
- **BLL**: servicios, reglas de negocio, validaciones y DTOs.
- **IOC**: configuración de inyección de dependencias.
- **App / Presentación**: interfaz de usuario con Blazor Server y Razor Components.

## Principios aplicados

- Inyección de dependencias.
- Repositorio genérico.
- Separación entre interfaz e implementación.
- Clases parciales y code-behind.
- Principios **SOLID**.
- Clean Code.
- Componentización de páginas y lógica reutilizable.
- Control de acceso por autenticación y permisos.

## Tecnologías utilizadas

- .NET
- ASP.NET Core
- Blazor Server
- Razor Components
- C#
- SQL Server
- Entity Framework Core
- Bootstrap

## Estructura general del proyecto

```text
Solución
│
├── Sistema.DAL
│   ├── Contexto
│   ├── Implementación
│   └── Interfaces
│
├── Sistema.Entidades
│   └── Clases de las entidades
│
├── Sistema.BLL
│   ├── Interfaces
│   ├── Implementación
│   ├── Services
│   ├── DTO
│   └── Clase para constantes
│
├── Sistema.IOC
│   └── Configuración de dependencias
│
└── Sistema.Presentacion
    ├── Components
    ├── Controller
    ├── Models
    ├── Pages
    ├── Shared
    ├── wwwroot
    └── Configuración UI
```

## Funcionalidades relevantes

### Seguridad
- Inicio de sesión con validación de credenciales.
- Recuperación de contraseña por correo.
- Restricción de páginas por autenticación.
- Restricción por rol/permisos aunque el usuario escriba la URL manualmente.

### UI / UX
- Menú lateral dinámico por rol.
- Tablas estilo tipo Excel para catálogos administrativos.
- Edición en línea por fila.
- Paginación en módulos con crecimiento de datos.
- Vista previa de imágenes en módulos que manejan archivos o iconos.

## Configuración inicial

### 1. Clonar el repositorio

```bash
git clone https://github.com/MemoRodz/AdmonArchivos.git
```

### 2. Abrir la solución

Abrir la solución en **Visual Studio**.

### 3. Configurar la cadena de conexión

Actualizar el archivo de configuración correspondiente, por ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVIDOR;Database=BASE_DATOS;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Restaurar paquetes

```bash
dotnet restore
```

### 5. Ejecutar migraciones o actualizar base de datos

Si aplica en tu proyecto:

```bash
dotnet ef database update
```

### 6. Ejecutar la aplicación

```bash
dotnet run
```

## Convenciones del proyecto

- Comentarios y documentación escritos en **Español MX**.
- Lógica de componentes separada en archivos `.razor` y `.razor.cs`.
- Uso de constantes para evitar valores en duro.
- Validaciones de acceso tanto en navegación como dentro de páginas sensibles.
- Estilo visual administrativo reutilizable desde `app.css`.

## Estado actual del proyecto

Entre los avances importantes se encuentran:

- Creación de usuarios con rol por defecto.
- Validación de acceso por permisos en páginas administrativas.
- Recuperación de contraseña funcionando con confirmación por correo.
- Menú dinámico mejorado.
- Tablas administrativas con estilo tipo Excel.
- Paginación aplicada en módulos específicos.

## Mejoras futuras

- Extender paginación a más módulos.
- Estandarizar plantillas CRUD reutilizables.
- Agregar búsqueda, ordenamiento y filtros.
- Mejorar componentes reutilizables para tablas.
- Refactorizar catálogos repetitivos hacia una base común.

## Autor

Proyecto desarrollado y mantenido por [MemoRodz](https://github.com/MemoRodz).