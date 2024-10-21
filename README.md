Разработано с использованием C# .NET 8 (8.0.403), ASP.NET Core, Entity Framework Core (EF Core) и SQLite. API предоставляет конечные точки для типичных операций CRUD (создание, чтение, обновление, удаление) для продуктов и категорий продуктов.

## Требования
- **Версия .NET SDK**: 8.0.0 или новее
- **Nuget gакеты**:
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.Sqlite`
  - `Microsoft.EntityFrameworkCore.Design`
  - `AutoMapper.Extensions.Microsoft.DependencyInjection`
  - `FluentValidation.AspNetCore`
  - `Swashbuckle.AspNetCore`
  - `Swashbuckle.AspNetCore.Annotations/>`

### Список необходимых NuGet пакетов:
```xml
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.10" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="6.9.0" />
```

## Инструкции по установке и запуску

### 1. Клонирование репозитория

```bash
git clone https://github.com/yourusername/ProductManagementAPI.git
cd ProductManagementAPI
```

### 2. Установка необходимых пакетов

```bash
dotnet restore
```

### 3. Запуск приложения

Чтобы запустить приложение в дев моде (Swagger + дев БД с тестовыми сущностями), используйте команду:

```bash
dotnet run --launch-profile "https"
```

Чтобы запустить приложение в дев моде (Swagger + дев БД с тестовыми сущностями), используйте команду:

```bash
dotnet run --launch-profile "https_prod"
```

Приложение будет доступно по адресу: [https://localhost:7028](https://localhost:7028)

Swagger: [https://localhost:7028/swagger/index.html](https://localhost:7028/swagger/index.html)

## Эндпоинты

### Продукты

- `GET /api/products`: Получить список всех продуктов
- `GET /api/products/{id}`: Получить продукт по ID
- `POST /api/products`: Создать новый продукт
- `PUT /api/products/{id}`: Обновить существующий продукт
- `DELETE /api/products/{id}`: Удалить продукт по ID

### Категории продуктов

- `GET /api/productcategories`: Получить список всех категорий продуктов
- `GET /api/productcategories/{id}`: Получить категорию продукта по ID
- `POST /api/productcategories`: Создать новую категорию продукта
- `PUT /api/productcategories/{id}`: Обновить существующую категорию продукта
- `DELETE /api/productcategories/{id}`: Удалить категорию продукта по ID
