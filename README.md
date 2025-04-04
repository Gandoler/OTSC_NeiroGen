# 🧠 NeiroGenApi — Генерация поздравлений

## 📌 Назначение

NeiroGenApi — микросервис, который по `pozdrikId` отправляет запрос к внешнему AI-сервису для генерации поздравления и сохраняет его через прокси `DbProxy`.

---

## 🚀 Возможности

- Генерация поздравлений по ID.
- Работа с внешним AI API по токену (`apiKey`).
- Интеграция с `DBProxy` через HttpClient.
- Логирование действий через Serilog.
- Swagger UI с описанием эндпоинтов.

---

## 🛠️ Технологии

- ASP.NET Core
- Serilog
- HttpClientFactory
- Swagger (Swashbuckle)
- Clean Architecture (Domain, Infrastructure, API)

---

## 🔗 Зависимости

- **Внешний AI API** — для генерации поздравлений (работает по токену).
- **DBProxy** — для сохранения данных в БД.

---

## 🔐 Переменные окружения

| Переменная  | Назначение                               |
|-------------|-------------------------------------------|
| `ApiKey`    | Ключ доступа к AI-сервису                |
| `ApiUrl`    | URL внешнего сервиса генерации поздравлений |
| `DbProxy`   | URL прокси-сервиса к базе данных         |

---

## 🧪 Swagger

Swagger UI доступен по адресу:

```
http://localhost:{port}/swagger
```

---

## ⚙️ Запуск

### 🔧 Без Docker
```bash
dotnet run
```

### 🐳 С Docker

#### 🖼 Ручная сборка
```bash
DOCKER_BUILDKIT=1 docker build -t neirogenapi_image -f Dockerfile .
docker run -d -p 8084:8080 --name neirogen_container \
  -e ApiKey=your_api_key \
  -e ApiUrl=https://your.ai.api \
  -e DbProxy=http://dbproxy_url \
  neirogenapi_image
```

---

## 📂 Эндпоинты

### `GET /api/GenerateCon/{pozdrikId}`

Создаёт поздравление по ID и сохраняет его через DBProxy.

#### Пример запроса:
```
GET /api/GenerateCon/6
```

#### Пример ответа:
```json
{
  "message": "Поздравление успешно добавлено!"
}
```

#### Возможные ответы:
- `200 OK` — поздравление успешно добавлено.
- `400 Bad Request` — не удалось добавить поздравление.

---

## 📞 Поддержка

В случае вопросов или проблем — добро пожаловать в [GitHub Issues](https://github.com/Gandoler/NeiroGenApi/issues)
