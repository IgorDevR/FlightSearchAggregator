# API Агрегатор Поиска Авиаперелетов

## Описание

API агрегатор поиска авиаперелетов предназначен для агрегации данных о доступных авиаперелетах из различных источников, предоставления информации о перелетах по запросу клиента, фильтрации и сортировки результатов поиска, а также бронирования выбранных рейсов.

## Основные функции

### Поиск перелетов
- Агрегация данных о перелетах из различных тестовых источников.
- Фильтрация по дате, цене, количеству пересадок и авиакомпании.
- Сортировка по цене, дате, количеству пересадок и длительности перелета.

### Бронирование рейсов
- Возможность бронирования выбранного рейса через API.

### Кэширование запросов
- Механизм кэширования запросов для снижения нагрузки на сервера.

### Логирование запросов
- Логирование всех запросов к API для последующего анализа.

### Аутентификация и авторизация
- Реализация механизмов аутентификации и авторизации для доступа к методам API.

## Развертывание

API может быть развернуто на облачных платформах, таких как AWS или Azure, для обеспечения масштабируемости и высокой доступности.

## Запуск проекта

1. Клонируйте репозиторий с GitHub.
2. Откройте решение в Visual Studio или другой IDE.
3. Восстановите зависимости проекта.
4. Запустите проект через IDE.

## Архитектура приложения

### Модели
- `Flight.cs`: Структура данных для полета.
- `Booking.cs`: Структура данных для бронирования.

### Сервисы
#### Booking
- `BookingService.cs`: Управление бронированиями.
- `BookingServiceFactory.cs`: Фабрика для создания экземпляров сервиса бронирования.

#### Providers
- `FlyQuestService.cs`: Сервис для работы с поставщиком FlyQuest.
- `SkyTrailsService.cs`: Сервис для работы с поставщиком SkyTrails.

### Auth
- `AuthService.cs`: Сервис аутентификации пользователей.
- `UserController.cs`: Контроллер для регистрации и входа пользователей.

### Контроллеры
- `FlightsController.cs`: Операции связанные с полетами.
- `BookingFlightsController.cs`: Операции связанные с бронированием полетов.

### Контексты
- `DbContext.cs`: Имитация контекста базы данных.

### DTOs (Data Transfer Objects)
- `BookingRequest.cs`: DTO для запроса бронирования.
- `FlightSearchParams.cs`: DTO для параметров поиска
- `Providers.cs`: DTO, связанные с поставщиками услуг полетов.

### Примеры
- `BookingRequestExample.cs`: Код, демонстрирующий создание запроса на бронирование.

### Вспомогательные файлы
- `AppSettings.cs`: Класс, отвечающий за настройки приложения.
- `appsettings.json`: Файл конфигурации, содержащий настройки приложения.
- `FlightSearchAggregator.http`: Файл с HTTP запросами для агрегирования данных о поиске полетов.

### Запуск приложения
- `Program.cs`: Точка входа приложения, запускающая веб-сервер и инициализирующая сервисы.
