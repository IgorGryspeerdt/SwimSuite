# Architectuur

## Status en bron

Dit document beschrijft de huidige implementatie. [Vision](VISION.md) bepaalt de productrichting; onderwerpen die daaruit volgen maar nog niet in code bestaan, staan hier als **toekomstige richting**.

## Overzicht

SwimSuite is één ASP.NET Core MVC-webapplicatie op .NET 10. Razor Views vormen de gebruikersinterface. Controllers roepen domein- en toepassingsgerichte services aan; die services gebruiken `ApplicationDbContext` voor Entity Framework Core-toegang tot PostgreSQL. ASP.NET Core Identity levert de gebruikers- en UI-infrastructuur.

```text
Browser → Razor/MVC-controller → service → ApplicationDbContext (EF Core) → PostgreSQL
                         ↘ Identity Razor Pages → ApplicationDbContext
```

## Lagen en verantwoordelijkheden

- **Presentatie:** `Controllers/` bevat MVC-routes en vertaalt service-uitkomsten naar views, redirects, validatiefouten of `NotFound`. `Views/` bevat Razor-views; Bootstrap en client-side unobtrusive validation zijn lokaal beschikbaar.
- **Toepassing/businesslogica:** `Services/` bevat interface en implementatie per huidig domein (`Club`, training, trainers, traineraanwezigheid, officials en official duties). Services controleren onder meer of records tot dezelfde club behoren, normaliseren optionele tekst en bewaren wijzigingen.
- **Data/infrastructuur:** `Data/ApplicationDbContext.cs` erft van `IdentityDbContext`, configureert relaties en bewaart `DbSet`s. EF Core-migraties staan in `Data/Migrations/`.
- **Domein en viewmodellen:** `Models/` bevat zowel persistente entiteiten als input- en lijstviewmodellen. Dit is momenteel één map, geen afzonderlijk domeinproject.

## Projectstructuur

| Locatie | Huidige verantwoordelijkheid |
| --- | --- |
| `Program.cs` | compositieroot, configuratie, DI en HTTP-pipeline |
| `Controllers/` | MVC-endpoints voor clubs, training, trainers, aanwezigheid, officials |
| `Services/` | asynchrone use cases en EF Core-queries |
| `Models/` | entiteiten, data-annotations en Razor-viewmodellen |
| `Data/` | context en migraties |
| `Areas/Identity/` | minimale Identity-area-ondersteuning; Identity UI komt uit het pakket |
| `Views/` | Razor MVC-views en gedeelde layout/loginpartial |
| `wwwroot/` | statische bestanden, Bootstrap, jQuery en validatiescripts |
| `Dockerfile` | meerstaps Linux-containerbuild |

## Controllers en services

Controllers zijn bewust dun: ze valideren `ModelState`, zetten route-gebonden `clubId`/`trainingBlockId` op het model, vullen in enkele gevallen een keuzelijst, en delegeren de bewerking. Een uitzondering is `OfficialDutiesController`, die rechtstreeks `ApplicationDbContext` gebruikt om het bestaan van een club en de lijst actieve officials voor de view op te halen. Nieuwe businesslogica hoort in een service; dit bestaande uitzonderingsgeval is geen reden om dat patroon uit te breiden.

Services gebruiken primaire-constructorinjectie en asynchrone EF Core-calls met `CancellationToken`. Leesbewerkingen gebruiken meestal `AsNoTracking()`. De huidige services zijn `scoped` geregistreerd via hun interface.

## Dependency Injection

`Program.cs` registreert `ApplicationDbContext`, Default Identity en zes scoped serviceparen. Controllers ontvangen interfaces via de constructor. Er is geen apart repository-, mediator- of unit-of-work-patroon: `ApplicationDbContext` is de directe data-toegangsgrens in services.

## EF Core en database-toegang

EF Core gebruikt `Npgsql.EntityFrameworkCore.PostgreSQL` met de connection string `ConnectionStrings:DefaultConnection`. De standaardinstelling wijst naar `Host=localhost;Port=5432;Database=swimsuite`; credentials staan niet in de repository en moeten via configuratie worden aangeleverd. `ApplicationDbContext` bevat de Identity-tabellen plus de tabellen voor clubs, training, trainers, aanwezigheid, officials en diensten. Zie [Database](DATABASE.md) voor het volledige model.

## Identity, authenticatie en autorisatie

`AddDefaultIdentity<IdentityUser>` gebruikt EF-opslag en vereist bevestigde accounts bij aanmelden. Identity Razor Pages worden gemapt en de layout bevat registratie-, login-, logout- en manage-links. De bedrijfscontrollers behalve `HomeController` dragen `[Authorize]`.

De huidige code registreert **geen** rollen, policies, claims of gebruiker-naar-club-koppeling. Alle geautoriseerde functionaliteit is dus in de controllercode op hetzelfde niveau beschermd: een ingelogde gebruiker is het bedoelde criterium, niet een specifieke rol of club.

Let op een feitelijke implementatieafwijking: de HTTP-pipeline roept `UseAuthorization()` aan, maar niet `UseAuthentication()`. Dit document beschrijft die toestand, niet een gewenste toestand. De effectieve runtimewerking van aanmelden en `[Authorize]` moet vóór productie worden geverifieerd en dit moet door de projectowner als technisch herstelwerk worden beoordeeld.

## Razor/MVC

De standaardroute is `{controller=Home}/{action=Index}/{id?}`; clubgebonden controllers gebruiken attribuutroutes zoals `clubs/{clubId:guid}/training`. POST-acties gebruiken `[ValidateAntiForgeryToken]`. Views gebruiken viewmodellen en tag helpers; de bestaande formulieren laden `_ValidationScriptsPartial`.

## Configuratie, logging en foutafhandeling

Configuratie komt uit de normale ASP.NET Core-configuratiebronnen. `appsettings.json` bevat connection-stringhost/database en logniveaus; secrets horen niet daarin. In Development is het EF-migratie-foutendpoint actief. Buiten Development wordt `/Home/Error` en HSTS gebruikt. De code schrijft zelf nog geen toepassingslogs; het geconfigureerde loggingframework is wel beschikbaar.

## Docker

`Dockerfile` is opgezet als meerstaps Linux-containerbuild voor .NET 10 en exposeert poorten 8080 en 8081. De huidige instructies verwijzen echter naar `SwimSuite.csproj`, terwijl het aanwezige projectbestand `SwimSuite.Web.csproj` heet; een Docker-build is daarom niet als werkend geverifieerd. Er is geen `docker-compose`-bestand of geconfigureerde databasecontainer; dat is volgens README/visie **toekomstig**.

## Grenzen, principes en uitbreidbaarheid

- Club-scoping zit in de meeste servicequeries via `ClubId` en relaties; dit voorkomt in die use cases koppelingen tussen bestaande clubs.
- Dit is nog geen werkende multi-tenancygrens: routes accepteren een willekeurige club-GUID en er is geen koppeling tussen de ingelogde Identity-gebruiker en een club.
- Services, interface-registratie, viewmodellen en expliciete FK-configuratie zijn de huidige uitbreidingspunten.
- Houd controllerlogica klein, hergebruik de bestaande service- en viewmodelpatronen, en voeg geen lagen toe zonder concrete noodzaak.

## Toekomstige richting: SaaS en multi-tenancy

De visie en bestaande `ClubId`-velden ondersteunen een toekomstige SaaS-richting met meerdere clubs. Nog te besluiten en te implementeren zijn onder andere: tenantresolutie op basis van de gebruiker of host, een gebruiker-clubrelatie, autorisatie per club/rol, consistente queryfilters en beheer van clubinstellingen. Deze onderdelen zijn geen huidige functionaliteit.
