# Diagrams wich we used for project:

## Component Diagram
Як наш код розбитий на модулі.

```mermaid

flowchart TD
    subgraph UI [Presentation Layer]
        AvaloniaApp[Avalonia UI Application]
    end
    
    subgraph AppLayer [Application Layer]
        Handlers[Chain of Responsibility Handlers]
        Interfaces[IRepositories / IAiService]
    end
    
    subgraph Core [Domain Layer]
        Entities[User, Quest, Reward Entities]
        Values[HP, XP Value Objects]
    end
    
    subgraph Infra [Infrastructure Layer]
        EF[Entity Framework Core]
        OpenAI[HTTP REST Client]
    end

    AvaloniaApp -->|Викликає команди| AppLayer
    Infra -.->|Реалізує інтерфейси| AppLayer
    AppLayer -->|Маніпулює| Core
    Infra -->|Зберігає стан| Core
    
    EF --> SQLite[(SQLite Local DB)]
    OpenAI --> WebAPI((AI API))
```
## Deployment Diagram
На якій платформі та в якому середовищі все це буде працювати.

```mermaid

flowchart LR
    subgraph UserPC [ПК Гравця - Windows OS]
        subgraph AppEnv [.NET 10 Runtime]
            AppExe[Life Quest.exe\nAvalonia Client]
        end
        DB[(SQLite)]
    end
    
    subgraph Cloud [Хмара]
        AIServer((Сервери AI))
    end

    AppExe <-->|Entity Framework\nЛокальне читання/запис| DB
    AppExe <-->|HTTP / JSON\nАсинхронні запити| AIServer
```

## Use Case Diagram

```mermaid
---
config:
  layout: elk
---
graph TB
    User["Користувач (Гравець)"]
    AI["AI Service"]
    
    User ~~~ AI

    User -->|"s5"| UC1["Створити квест"]
    User -->|"s6"| UC2["Виконати квест"]
    User -->|"s7"| UC3["Отримати нагороду"]
    
    UC1 -.->|extend| UC5["Декомпозиція цілі"]
    UC2 -.->|include| UC4["Згенерувати мотивацію"]
    
    AI --> UC4
    AI --> UC5


```


## Class Diargam (Domain)

```mermaid
---
config:
  layout: elk
---
classDiagram
    class User {
        -int Id
        -string Username
        -HealthPoints HP
        -ExperiencePoints XP
        -Currency Gold
        +TakeDamage(int amount) void
        +AddExperience(int amount) void
    }

    class Quest {
        -int Id
        -string Title
        -bool IsCompleted
        +Complete() void
    }

    class Reward {
        -string Name
        -int Cost
        +Purchase(User user) void
    }

    User *-- Quest : має
    User *-- Reward : купує
```

## Activity Diagram
Показує як працює наш патерн Chain of Responsibility.

```mermaid

flowchart TD
    Start([Початок: Гравець натискає 'Виконати квест']) --> Validate[ValidationHandler: Перевірка статусу квесту]
    
    Validate -->|Невалідно/Шахрайство| Error([Відмова: Квест не оновлено])
    Validate -->|Валідно| CalcXP[ExperienceHandler: Нарахування XP та Золота]
    
    CalcXP --> CheckLvl[LevelUpHandler: Перевірка XP гравця]
    
    CheckLvl -->|Достатньо XP| LevelUp[Підвищення рівня гравця]
    CheckLvl -->|Недостатньо XP| CheckAI[AiMotivationHandler: Генерація повідомлення]
    LevelUp --> CheckAI
    
    CheckAI --> Save[PersistenceHandler: Збереження змін у БД]
    Save --> End([Кінець: Оновлення інтерфейсу])
```
## State Machine Diagram
Життєвий цикл одного об'єкта: сутність Quest.

```mermaid

stateDiagram-v2
    [*] --> Draft : Створено (Гравець налаштовує)
    Draft --> Active : Підтверджено (Взято в роботу)
    
    Active --> Completed : Виконано успішно
    Active --> Failed : Пропущено (для щоденних) / Провалено
    
    Completed --> Archived : Переміщено в історію
    Failed --> Archived : Переміщено в історію
    
    Archived --> [*]
```