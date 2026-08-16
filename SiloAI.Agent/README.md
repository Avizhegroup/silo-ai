# SiloAI.Agent

## Overview

`SiloAI.Agent` is the AI agent library at the heart of the **Silo WMS** (Warehouse Management System), developed by **Tadbir Dade Avizhe** ([avizhegroup.com](https://avizhegroup.com)). It provides a reusable, instruction-driven conversational AI layer powered by **OpenAI GPT models** via the **GitHub AI Inference endpoint** and the **Microsoft Agents SDK**.

The library is consumed by the Blazor UI module (`Silo.Modules.Ai`) and the API layer (`Silo.Api`) to deliver:

- A **chat assistant** that answers questions about the Silo WMS in Persian (Farsi).
- A **report-building agent** that translates natural-language queries into SQL and renders results in the UI.
- An **OCR service** that reads Iranian national cards and vehicle licence plates from images.
- **Session-aware multi-turn conversations** that can be persisted to and restored from the database.

---

## Project Structure

```
SiloAI.Agent/
├── Chat/
│   ├── ChatAgentService.cs               ← Core service class
│   ├── chtbot-instructions-main.md       ← Base system prompt (always loaded)
│   ├── chtbot-instructions-add-product.md
│   ├── chtbot-instructions-exit-report-builder.md
│   ├── chtbot-instructions-inventory-conflict.md
│   ├── chtbot-instructions-location.md
│   ├── chtbot-instructions-nationalcard.md
│   ├── chtbot-instructions-plaque.md
│   ├── chtbot-instructions-product-report-builder.md
│   ├── chtbot-instructions-report-query.md
│   ├── chtbot-instructions-report.md
│   ├── chtbot-instructions-reports-truckcross.md
│   └── chtbot-instructions-truckcross.md
├── Tools/
│   └── SqlTextTools.cs                   ← Utility for extracting SQL blocks from AI responses
├── Knowledge/                            ← Static knowledge-base files (copied to output)
│   ├── wms-documentation.md
│   ├── product-specifications.md
│   ├── rfid-technology.md
│   ├── warehouse-procedures.md
│   ├── faq.md
│   └── system-config.json
├── Load_Agent_Diagram.md                 ← Mermaid flowchart of agent initialization
└── SiloAI.Agent.csproj
```

---

## NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| `OpenAI` | 2.8.0 | Official OpenAI .NET SDK — provides `ChatClient` and `ApiKeyCredential` |
| `Microsoft.Extensions.AI` | 10.3.0 | Core AI abstractions: `IChatClient`, `ChatMessage`, `AIContent`, `DataContent`, `TextContent` |
| `Microsoft.Extensions.AI.OpenAI` | 10.3.0 | Bridge that exposes the OpenAI `ChatClient` as `IChatClient` via `.AsIChatClient()` |
| `Microsoft.Agents.AI` | 1.0.0-preview | Microsoft Agents SDK — provides `AIAgent`, `ChatClientAgent`, `AgentSession`, `ChatClientAgentOptions` |

> **Central package management:** Versions are declared in `Directory.Packages.props` at the solution root. Do **not** specify versions in the `.csproj` file itself.

---

## Core Classes & Methods

### `ChatAgentService` (`Chat/ChatAgentService.cs`)

The main service class. Register it as **scoped** or **transient** in your DI container and call `InitChatAgent` before using any other method.

#### `InitChatAgent`

```csharp
public async Task InitChatAgent(List<string>? promptKeys = null)
```

Creates the `IChatClient` connected to the GitHub AI Inference endpoint and builds the `AIAgent` with the combined system instructions loaded from the `Chat/` folder.

| Parameter | Description |
|-----------|-------------|
| `promptKeys` | Optional list of instruction-file keys to load (e.g. `"add-product"`, `"truckcross"`). The file `chtbot-instructions-main.md` is **always** included regardless of this list. |

**Example**

```csharp
var service = new ChatAgentService();

// Load only main + product instructions
await service.InitChatAgent(new List<string> { "add-product" });

// Load agent mode (all agent keys)
await service.InitChatAgent(new List<string>
{
    "agent-general", "add-product", "report-builder",
    "exit-report-builder", "product-report-builder",
    "truckcross", "location", "inventory-conflict", "reports-truckcross"
});
```

---

#### `SendRequestAndGetResponse`

```csharp
public async Task<CopilotMessageDto> SendRequestAndGetResponse(CopilotMessageRequest query)
```

Stateless single-turn call. Sends the user message to the agent and returns the text response. No session is preserved between calls.

| Parameter | Type | Description |
|-----------|------|-------------|
| `query` | `CopilotMessageRequest` | Contains the user's `Text` and metadata |

**Returns** `CopilotMessageDto` with `ResponseText`.

---

#### `SendWithAgentSessionAsync`

```csharp
public async Task<(CopilotMessageDto Response, string SerializedSession)>
    SendWithAgentSessionAsync(string? sessionJson, CopilotMessageRequest query)
```

**Stateful multi-turn call.** Deserializes or creates an `AgentSession`, sends the message, and serializes the updated session back as a JSON string so it can be stored in the database and passed back on the next turn.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sessionJson` | `string?` | JSON of a previously serialized `AgentSession`. Pass `null` to start a new session. |
| `query` | `CopilotMessageRequest` | The user message. |

**Returns** a tuple of `(CopilotMessageDto, string)` — the AI response and the updated serialized session.

**Example**

```csharp
string? session = null; // first message

var (response1, session) = await service.SendWithAgentSessionAsync(null, new CopilotMessageRequest { Text = "سلام" });
// store session in DB …

var (response2, session) = await service.SendWithAgentSessionAsync(session, new CopilotMessageRequest { Text = "موجودی انبار قرنطینه چقدر است؟" });
```

---

#### `CreateNewSessionAsync`

```csharp
public async Task<AgentSession> CreateNewSessionAsync()
```

Creates a blank `AgentSession` without sending any message.

---

#### `SerializeSessionAsync`

```csharp
public async Task<string> SerializeSessionAsync(AgentSession session)
```

Serializes an `AgentSession` to a raw JSON string for persistence.

---

#### `SendImageAndGetTextAsync` (byte array overload)

```csharp
public async Task<string> SendImageAndGetTextAsync(
    byte[] imageData,
    string imageMediaType = "image/jpeg",
    string? promptKey = null)
```

Sends an image to the AI agent along with an instruction file and returns the extracted text. Used for OCR scenarios (national card, licence plate).

| Parameter | Description |
|-----------|-------------|
| `imageData` | Raw image bytes |
| `imageMediaType` | MIME type, e.g. `"image/jpeg"`, `"image/png"` |
| `promptKey` | Key of the instruction file to load (e.g. `"nationalcard"`, `"plaque"`) |

---

#### `SendImageAndGetTextAsync` (Stream overload)

```csharp
public async Task<string> SendImageAndGetTextAsync(
    Stream imageStream,
    string imageMediaType = "image/jpeg",
    string? promptText = null)
```

Stream-based variant — reads the stream into memory and delegates to the byte-array overload.

---

### `SqlTextTools` (`Tools/SqlTextTools.cs`)

A static utility class that parses AI-generated responses containing embedded SQL blocks delimited by `<<SQL … >>`.

#### `StripSqlBlocks`

```csharp
public static string StripSqlBlocks(string text, List<string>? collectedCommands = null)
```

Removes all `<<SQL … >>` blocks from the AI response text. If `collectedCommands` is provided, the extracted SQL body of each block is appended to the list.

| Parameter | Description |
|-----------|-------------|
| `text` | Raw AI response that may contain SQL blocks |
| `collectedCommands` | Optional list to collect extracted SQL command strings |

**Returns** the cleaned response text with SQL blocks removed.

**Example**

```csharp
var sqlCommands = new List<string>();
string cleanText = SqlTextTools.StripSqlBlocks(aiResponseText, sqlCommands);
// cleanText → human-readable reply
// sqlCommands → ["SELECT * FROM tbl_Tags WHERE ..."]
```

---

## Instruction File System

Instructions are plain Markdown files placed in the `Chat/` folder. They are **copied to the output directory** on build.

### File naming convention

```
chtbot-instructions-{key}.md
```

### Loading rules

| File | Loaded when |
|------|-------------|
| `chtbot-instructions-main.md` | **Always** — regardless of `promptKeys` |
| `chtbot-instructions-{key}.md` | Only when `key` appears in the `promptKeys` list passed to `InitChatAgent` |

All loaded files are concatenated with `=== filename ===` separators and passed as the `Instructions` field of `ChatClientAgentOptions`.

### Predefined keys

| Key | File | Purpose |
|-----|------|---------|
| *(always)* | `chtbot-instructions-main.md` | Base persona, red-lines, command block syntax |
| `add-product` | `chtbot-instructions-add-product.md` | Guidance for the Add Product form |
| `truckcross` | `chtbot-instructions-truckcross.md` | Truck traffic registration page |
| `location` | `chtbot-instructions-location.md` | Goods movement & location selection |
| `inventory-conflict` | `chtbot-instructions-inventory-conflict.md` | Inventory discrepancy handling |
| `nationalcard` | `chtbot-instructions-nationalcard.md` | Iranian national card OCR prompt |
| `plaque` | `chtbot-instructions-plaque.md` | Iranian vehicle licence plate OCR prompt |
| `report` | `chtbot-instructions-report.md` | Report mode instructions |
| `report-query` | `chtbot-instructions-report-query.md` | SQL schema context for report queries |
| `exit-report-builder` | `chtbot-instructions-exit-report-builder.md` | Exit report building |
| `product-report-builder` | `chtbot-instructions-product-report-builder.md` | Product report building |
| `reports-truckcross` | `chtbot-instructions-reports-truckcross.md` | Truck-cross reports |

### Adding a new instruction file

1. Create `Chat/chtbot-instructions-{yourkey}.md`.
2. Add the following entry to `SiloAI.Agent.csproj`:
   ```xml
   <None Update="Chat\chtbot-instructions-{yourkey}.md">
     <CopyToOutputDirectory>Always</CopyToOutputDirectory>
   </None>
   ```
3. Pass `"{yourkey}"` in the `promptKeys` list when calling `InitChatAgent`.

---

## AI Command Block Protocol

The main instruction file teaches the agent to embed structured command blocks at the end of its response:

```
<<SQL
SELECT * FROM tbl_Tags WHERE TagInDestinationId = 'WH001'
>>

<<API
GET /api/products?code=ABC123
>>
```

The UI layer uses `SqlTextTools.StripSqlBlocks` to extract SQL commands, execute them against the database, and render the results as an HTML table inline in the chat.

### Allowed commands

| Block type | Allowed operations |
|------------|--------------------|
| `<<SQL … >>` | `SELECT` only — no `INSERT`, `UPDATE`, `DELETE`, `EXECUTE`, `EXEC`, `DROP` |
| `<<API … >>` | REST calls: `GET`, `POST`, `PUT`, `DELETE` |
| `<<HTML … >>` | Static HTML + CSS (no JavaScript) |

---

## Data Transfer Objects (from `Silo.Application`)

| Class | Namespace | Description |
|-------|-----------|-------------|
| `CopilotMessageRequest` | `Silo.Application.Features` | Outbound request: `Username`, `Text`, `SiloChatId`, `IsUser`, `Datetime`, `SqlCommands`, `SqlCommandsResults` |
| `CopilotMessageDto` | `Silo.Application.Features` | Inbound response: `ResponseText` |
| `ChatHistory` | `Silo.Application.Features` | Persisted session record: `Id`, `Title`, `Messages`, `AgentSessionJson`, `CreatedDate`, `LastUpdated` |

---

## Agent Initialization Flowchart

```
InitializeChatWithMode
        │
        ▼
  ChatPageMode?
  ┌─────┴──────┐
Agent         Report
  │               │
  │  promptKeys   │  promptKeys
  │  (9 keys)     │  ["report","report-query"]
  └──────┬────────┘
         ▼
  AiAgent.InitChatAgent(promptKeys)
         │
         ▼
  Create OpenAI ChatClient
  Model: gpt-4.1
  Endpoint: https://models.github.ai/inference
         │
         ▼
  LoadInstructionsAsync(promptKeys)
         │
         ▼
  Scan ALL files in AppBaseDir/Chat/
         │
         ├── chtbot-instructions-main.md  → ✅ Always Include
         │
         └── other .md files
               │
               ├── filename matches chtbot-instructions-{key}.md
               │   for any key in promptKeys → ✅ Include
               └── no match → ⛔ Skip
         │
         ▼
  Join content with newlines
         │
         ▼
  ChatClientAgent created with Instructions
         │
         ▼
  ✅ Agent Ready
```

---

## How to Integrate into Another .NET Project

### 1. Add NuGet packages

In your `.csproj` (or via Central Package Management):

```xml
<ItemGroup>
  <PackageReference Include="OpenAI" Version="2.8.0" />
  <PackageReference Include="Microsoft.Extensions.AI.OpenAI" Version="10.3.0" />
  <PackageReference Include="Microsoft.Extensions.AI" Version="10.3.0" />
  <PackageReference Include="Microsoft.Agents.AI" Version="1.0.0-preview.260212.1" />
</ItemGroup>
```

### 2. Copy the `Chat/` folder

Copy the `Chat/` directory (with all `chtbot-instructions-*.md` files) into your project and mark every file as **Copy to Output Directory: Always**:

```xml
<ItemGroup>
  <None Update="Chat\chtbot-instructions-main.md">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </None>
  <!-- repeat for each instruction file -->
</ItemGroup>
```

### 3. Copy the service and tools

Copy `Chat/ChatAgentService.cs` and `Tools/SqlTextTools.cs` into your project. Adjust the namespace as needed.

### 4. Copy the required DTOs

You need `CopilotMessageRequest` and `CopilotMessageDto`. Either reference `Silo.Application` or create minimal equivalents:

```csharp
public class CopilotMessageRequest
{
    public string Text { get; set; }
    // add other fields as needed
}

public class CopilotMessageDto
{
    public string ResponseText { get; set; }
}
```

### 5. Register the service

```csharp
// Program.cs / Startup.cs
builder.Services.AddScoped<ChatAgentService>();
```

### 6. Configure the API key

Replace the hard-coded key in `ChatAgentService.InitChatAgent` with a value loaded from configuration:

```csharp
// appsettings.json
{
  "AiAgent": {
    "ApiKey": "<your-github-or-openai-api-key>",
    "Model": "gpt-4.1",
    "Endpoint": "https://models.github.ai/inference"
  }
}
```

```csharp
// Inject IConfiguration and read:
var apiKey  = configuration["AiAgent:ApiKey"];
var model   = configuration["AiAgent:Model"];
var endpoint = configuration["AiAgent:Endpoint"];

chatClient = new ChatClient(model,
    new ApiKeyCredential(apiKey),
    new OpenAIClientOptions { Endpoint = new Uri(endpoint) })
    .AsIChatClient();
```

### 7. Use the service

```csharp
// Stateless single turn
await chatAgentService.InitChatAgent(new List<string> { "add-product" });
var result = await chatAgentService.SendRequestAndGetResponse(
    new CopilotMessageRequest { Text = "فیلدهای اجباری فرم افزودن کالا کدامند؟" });
Console.WriteLine(result.ResponseText);

// Stateful multi-turn
await chatAgentService.InitChatAgent();
var (resp, session) = await chatAgentService.SendWithAgentSessionAsync(
    null,
    new CopilotMessageRequest { Text = "موجودی انبار امروز چقدر است؟" });
// persist `session` string to DB …
// on next request restore it:
var (resp2, session2) = await chatAgentService.SendWithAgentSessionAsync(
    session,
    new CopilotMessageRequest { Text = "جزئیات بیشتر بده" });

// OCR — Iranian national card
await chatAgentService.InitChatAgent(new List<string> { "nationalcard" });
string json = await chatAgentService.SendImageAndGetTextAsync(imageBytes, "image/jpeg", "nationalcard");

// Extract SQL commands from an AI response
var commands = new List<string>();
string cleanText = SqlTextTools.StripSqlBlocks(aiResponseText, commands);
// execute commands[0] against your database …
```

---

## Requirements Summary

| Requirement | Value |
|-------------|-------|
| .NET version | .NET 9+ |
| AI model | `gpt-4.1` (or any OpenAI-compatible model) |
| API endpoint | `https://models.github.ai/inference` (GitHub AI) or `https://api.openai.com/v1` (OpenAI) |
| Auth | GitHub PAT or OpenAI API key |
| Required files at runtime | All `Chat/chtbot-instructions-*.md` files next to the output assembly |

---

## Related Projects

| Project | Role |
|---------|------|
| `Silo.Modules.Ai` | Blazor UI module — renders the `ChatBot.razor` component |
| `Silo.Application` | Shared DTOs and MediatR queries used by the AI layer |
| `Silo.Api` | Registers `ChatAgentService` and exposes `ChatSessions` endpoints |
