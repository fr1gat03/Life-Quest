```mermaid

%%{init: {'theme': 'neutral'}}%%

flowchart TD

  subgraph Presentation ["Presentation Layer"]

    GameVM["GameViewModel"]

  end

  subgraph AppServices ["Application · Services"]

    QuestService["QuestService"]

  end

  subgraph Builder ["Application · Builder"]

    ChainBuilder["QuestHandlerChainBuilder"]

  end

  subgraph Chain ["Chain of Responsibility"]

    direction LR

    Base(["BaseQuestHandler"])

    VH["ValidationHandler"]

    EH["ExperienceHandler"]

    AMH["AiMotivationHandler"]

    PH["PersistenceHandler"]

    VH -->|"PassToNext"| EH

    EH -->|"PassToNext"| AMH

    AMH -->|"PassToNext"| PH

    Base -.->|"наслідує"| VH

    Base -.->|"наслідує"| EH

    Base -.->|"наслідує"| AMH

    Base -.->|"наслідує"| PH

  end

  subgraph Interfaces ["Interfaces"]

    IAiSvc(["IAiService"])

    IUserRepo(["IUserRepository"])

    IQuestRepo(["IQuestRepository"])

  end

  subgraph Domain ["Domain Layer"]

    User["User"]

    UserStats["UserStats"]

    QuestCollection["QuestCollection"]

    Quest["Quest"]

    User --> UserStats

    User --> QuestCollection

    QuestCollection -->|"містить"| Quest

  end

  subgraph Infra ["Infrastructure"]

    GeminiSvc["GeminiAiService"]

  end

  subgraph Storage ["Storage / External"]

    DB[("SQLite")]

    GeminiAPI(["Gemini API"])

  end

  GameVM -->|"CompleteQuestAsync()"| QuestService

  QuestService -->|"Build()"| ChainBuilder

  ChainBuilder -->|"Handle(context)"| Chain

  VH -->|"помилка"| Fail["QuestExecutionResult.Failure"]

  PH -->|"успіх"| Ok["QuestExecutionResult.Success"]

  AMH -->|"використовує"| IAiSvc

  PH -->|"використовує"| IUserRepo

  PH -->|"використовує"| IQuestRepo

  Chain -->|"використовує"| Domain

  IAiSvc -.->|"реалізує"| GeminiSvc

  GeminiSvc --> GeminiAPI

  IUserRepo -.->|"зберігає"| DB

  IQuestRepo -.->|"зберігає"| DB

```