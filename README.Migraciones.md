# Conta360 – Migraciones de Base de Datos (EF Core)

Este proyecto aplica migraciones de Entity Framework Core automáticamente en entornos de desarrollo y ofrece control explícito en producción.

---

## 🛠️ Migraciones en Entorno de Desarrollo

Las migraciones se aplican automáticamente cuando levantas el backend con Docker o desde el entorno local, siempre que el entorno sea `Development`.

### ✅ Opción 1: Usando Docker Compose

El archivo `docker-compose.yml` incluye este comando en el servicio `validation-core`:

```bash
dotnet ef database update --project Conta360.Presentation.Api.csproj && dotnet watch --project Conta360.Presentation.Api.csproj run --urls 'http://+:80'
```

Esto asegura que las migraciones se apliquen al iniciar la API:

```bash
docker-compose up --build
```

La base de datos SQLite persistente se monta en:

```
/app/data/conta360.db
```

### ✅ Opción 2: Ejecutando localmente con .NET

Si ejecutas el backend desde Visual Studio, Rider o CLI:

```bash
dotnet run --project ./backend/Presentation/Conta360.Presentation.Api
```

...las migraciones también se aplicarán si:

- El entorno es `Development`, **o**
- La variable de entorno `EFCORE_AUTOMIGRATE=true` está definida.

---

## 🔒 Migraciones en Producción o CI/CD

En producción se recomienda evitar migraciones automáticas y aplicar cambios de esquema de forma controlada.

### 🚫 Para deshabilitar migraciones automáticas

No definas la variable `EFCORE_AUTOMIGRATE`, o asegúrate de que sea `false`.

### ✅ Para forzar migraciones manualmente

Define la variable de entorno:

```env
EFCORE_AUTOMIGRATE=true
```

Esto hará que las migraciones se ejecuten en tiempo de arranque, incluso si el entorno **no es** `Development`.

---

## 📌 Resumen de comportamiento

| Entorno     | Migraciones automáticas | Acción recomendada                       |
|-------------|--------------------------|-------------------------------------------|
| Desarrollo  | ✅ Sí                     | Usar `docker-compose` o `dotnet run`      |
| Producción  | ❌ No (por defecto)       | Aplicar migraciones manualmente o por CI  |

---

## 🧠 Notas adicionales

- La lógica de migración está en `Program.cs` y utiliza `IApplicationDbContext` como `DbContext`.
- En caso de fallo en las migraciones, la API no arrancará (comportamiento esperado y seguro).
- El log mostrará mensajes claros sobre éxito o error en migraciones.

---

> Si necesitas soporte para migraciones en PostgreSQL, actualiza la cadena de conexión y el parámetro `dbProvider` en `Program.cs` y `docker-compose.yml`.

  subvenciones-api:
    build:
      context: .
      dockerfile: ./src/SubvencionesApp/Dockerfile
    ports:
      - "5001:80"
    volumes:
      - ./src/SubvencionesApp:/src/SubvencionesApp
      - subvenciones-api-data:/app/data
    environment:
      - ConnectionStrings__POSTGRESQLConnection=Host=${POSTGRES_HOST_SUBVENCIONES};Port=${POSTGRES_PORT_SUBVENCIONES};Database=${POSTGRES_DB_SUBVENCIONES};Username=${POSTGRES_USER_SUBVENCIONES};Password=${POSTGRES_PASSWORD_SUBVENCIONES};Pooling=true;MinPoolSize=0;MaxPoolSize=100;CommandTimeout=60
    depends_on:
      postgres-db:
        condition: service_healthy
      subvenciones-migrations:
        condition: service_completed_successfully
    healthcheck:
      test: ["CMD", "dotnet", "--info"]
      interval: 30s
      timeout: 3s
      retries: 3
    networks:
      - conta360-network
    env_file:
      - .env

  subvenciones-migrations:
    build:
      context: .
      dockerfile: ./src/SubvencionesApp/Dockerfile.Migrations
    depends_on:
      postgres-db:
        condition: service_healthy
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
      - ConnectionStrings__POSTGRESQLConnection=Host=${POSTGRES_HOST_SUBVENCIONES};Port=${POSTGRES_PORT_SUBVENCIONES};Database=${POSTGRES_DB_SUBVENCIONES};Username=${POSTGRES_USER_SUBVENCIONES};Password=${POSTGRES_PASSWORD_SUBVENCIONES}
    networks:
      - conta360-network
    env_file:
      - .env
    restart: "no"