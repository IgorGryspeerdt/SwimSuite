# Coding standards

## Status

Dit document onderscheidt **bestaande conventies** (zichtbaar in de code) van **aanbevolen conventies** voor consistente uitbreiding. Een aanbeveling is geen claim dat de hele codebase die al volgt.

## Bestaande conventies

- C# met nullable reference types en implicit usings is ingeschakeld; namespaces zijn file-scoped en heten `SwimSuite.<map>`.
- Klassen, publieke methoden en properties gebruiken PascalCase; parameters en lokale variabelen camelCase. Asynchrone methoden eindigen op `Async`.
- Services hebben een interface `I…Service`, een concrete `…Service` en primaire-constructorinjectie van `ApplicationDbContext`; registratie is scoped in `Program.cs`.
- Controllers gebruiken constructorinjectie, asynchrone acties en `CancellationToken`; POST-acties hebben anti-forgeryvalidatie. De meeste clubgebonden routes dragen GUID-routeconstraints.
- Persistente modellen en invoer-/lijstviewmodellen staan nu samen in `Models/`. Data annotations verzorgen model- en clientvalidatie.
- EF Core-leesqueries gebruiken meestal `AsNoTracking()`, filteren clubgebonden records op `ClubId`, sorteren expliciet en gebruiken `Include` wanneer de view een navigatie nodig heeft.
- Services trimmen verplichte tekst en normaliseren lege optionele tekst naar `null`.

## Aanbevolen conventies voor nieuwe code

### Klassen, services en controllers

- Houd controllers bij HTTP/MVC-zaken: modelbinding, `ModelState`, viewselectie, routewaarden en HTTP-uitkomsten. Plaats use-case- en consistentielogica in een service.
- Voeg een service-interface en scoped DI-registratie toe wanneer een nieuwe use case business- of data-toegang bevat. Vermijd een extra repositorylaag zolang die geen concreet probleem oplost.
- Geef asynchrone service- en EF-calls een doorgegeven `CancellationToken`. Gebruik `Task`/`Task<T>`, niet `async void`.
- Gebruik specifieke viewmodellen voor invoer of samengestelde views; bind niet onnodig rechtstreeks aan een entiteit.
- Houd methoden klein en één verantwoordelijk; kies namen die de actie en het resultaat uitdrukken, zoals `GetListAsync` of `CreateAsync`.

### EF Core en database

- Filter elke clubgebonden query expliciet op de vertrouwde clubcontext/`ClubId`; valideer ook dat gerelateerde IDs bij die club horen.
- Gebruik `AsNoTracking()` voor pure reads. Gebruik `Include` alleen voor benodigde navigaties en projecteer waar dat de view eenvoudiger maakt.
- Voeg of wijzig entiteit, `DbSet`, Fluent-configuratie, migratie en documentatie als één samenhangende wijziging. Beoordeel constraints en deletegedrag vóór het maken van de migratie.
- Vermijd N+1-queries, onbegrensde dataophalingen in nieuwe schermen en raw SQL zonder een aantoonbare reden.

### Validatie, fouten en logging

- Combineer data annotations voor invoerstructuur met servicevalidatie voor relaties en businessregels. Vertrouw nooit op verborgen `ClubId`-velden; zet routewaarden in de controller zoals de bestaande code doet.
- Geef onvindbare of cross-club records een passende bestaande uitkomst (`NotFound` of een modelvalidatiefout) en lek geen gegevens uit een andere club.
- De code bevat nog geen toepassingslogging. Introduceer bij nieuwe relevante fout- of auditpaden `ILogger<T>` met zinvolle, niet-gevoelige context; log geen wachtwoorden, tokens of persoonsgegevens zonder noodzaak.
- Gebruik uitzonderingen niet voor gewone validatie. Behoud de centrale productie-foutafhandeling in `Program.cs`.

### Razor/MVC en documentatie

- Gebruik tag helpers, viewmodellen, anti-forgery op muterende formulieren en `_ValidationScriptsPartial` wanneer clientvalidatie past.
- Houd views presentatief; plaats query- en businesslogica niet in Razor.
- Voeg comments alleen toe voor niet-evidente motivatie of beperking. Werk XML-documentatie niet mechanisch overal bij; houd [architectuur](ARCHITECTURE.md), [database](DATABASE.md), business rules en het relevante domeindocument actueel wanneer gedrag verandert.

### SOLID, configuratie en nullable

- Respecteer single responsibility en dependency inversion via de bestaande serviceinterfaces, zonder interfaces voor triviale helpers te creëren.
- Gebruik constructorinjectie; haal geen services uit `IServiceProvider` in reguliere applicatiecode.
- Behandel nullable navigaties en optionele invoer expliciet. Initialiseer niet-null collecties en strings zoals de bestaande modellen doen; gebruik geen null-forgiving operator om waarschuwingen te verbergen.
- Lees instellingen via `IConfiguration`/Options waar een nieuwe samenhangende configuratiesectie dat rechtvaardigt. Houd secrets buiten `appsettings*.json`.

## Migrations en controles

Gebruik `dotnet ef migrations add <Beschrijving>` en controleer de migratie vóór `dotnet ef database update`. Voer daarna ten minste `dotnet build` uit en test de betrokken routes/validatie. Er zijn momenteel geen geautomatiseerde testprojecten in de repository; voeg tests niet alleen om de documentatieplicht toe, maar behandel nieuwe businesslogica als kandidaat voor gerichte tests.
