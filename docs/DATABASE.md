# Database

## Huidige technologie en context

SwimSuite gebruikt PostgreSQL via EF Core en de Npgsql-provider. `ApplicationDbContext` erft van `IdentityDbContext`; daardoor omvat de database zowel ASP.NET Core Identity-tabellen als de applicatietabellen hieronder. Alle applicatie-entiteiten gebruiken een `Guid` als primaire sleutel. EF Core-migraties staan in `Data/Migrations/`; er is geen seed data in de code.

## Development seed data

`Data/DevelopmentDataSeeder.cs` fills a fixed, fictional Northstar Aquatics dataset only when the application runs in the Development environment. It runs at application startup after `builder.Build()`, uses stable IDs and existing unique relationships, and is therefore safe to run repeatedly. It never runs outside Development and does not apply migrations.

The dataset includes a club, three training groups, active and inactive trainers and officials, training blocks, trainer attendance, and several official duties. Existing `Notes` fields demonstrate a changed location, a cancelled training, an absent trainer, and a trainer replacement. Swimmers, competitions, swimmer attendance, and reimbursements are not seeded because they do not exist in the current model.

The seeder can also create two confirmed fictional Identity users without roles. Set a local password without committing it to the repository, then start the application:

```powershell
dotnet user-secrets set "DevelopmentSeed:Password" "<choose-a-local-test-password>"
dotnet run
```

Without `DevelopmentSeed:Password`, only the domain dataset is seeded. Roles, claims, and permissions are not seeded because the current application does not register or use them.

## Applicatie-entiteiten

| Entiteit / tabel | Primaire sleutel | Belangrijkste velden en relaties |
| --- | --- | --- |
| `Club` / `Clubs` | `Id` | Naam (verplicht, max. 160), optionele registratie-, contact- en adresgegevens, `CreatedAtUtc`; één-op-veel naar groups, blocks, trainers, officials en duties. |
| `TrainingGroup` / `TrainingGroups` | `Id` | `ClubId` FK, naam verplicht/max. 120, optionele omschrijving/max. 400; één-op-veel naar blocks. |
| `TrainingBlock` / `TrainingBlocks` | `Id` | `ClubId` en `TrainingGroupId` FKs; datum, begin/eindtijd, optionele locatie/max. 160 en notities/max. 400; één-op-veel naar traineraanwezigheid. |
| `Trainer` / `Trainers` | `Id` | `ClubId` FK, verplichte voor- en achternaam (elk max. 120), optionele e-mail/max. 160 en telefoon/max. 80, `IsActive`, `CreatedAtUtc`; één-op-veel naar aanwezigheid. |
| `TrainerAttendance` / `TrainerAttendances` | `Id` | `ClubId`, `TrainingBlockId`, `TrainerId` FKs, `IsPresent`, optionele notities/max. 400, `CreatedAtUtc`. |
| `Official` / `Officials` | `Id` | `ClubId` FK, verplichte voor- en achternaam, optionele e-mail, telefoon en licentienummer/max. 80, `IsActive`, `CreatedAtUtc`; één-op-veel naar duties. |
| `OfficialDuty` / `OfficialDuties` | `Id` | `ClubId` en `OfficialId` FKs, datum, verplichte wedstrijdnaam/max. 160 en rol/max. 120, optionele locatie/max. 160/notities/max. 400, `CreatedAtUtc`. |

De Identity-tabellen omvatten onder andere `AspNetUsers`, `AspNetRoles`, claims, logins, tokens en user-role-koppelingen. De runtime is geconfigureerd met `IdentityUser`; de roltabellen bestaan door het Identity-migratieschema, maar de applicatie registreert of gebruikt geen rollen. Zie [Permissions](permissions.md).

## Relaties, foreign keys en deletegedrag

| Relatie | Deletegedrag |
| --- | --- |
| Club → TrainingGroup, TrainingBlock, Trainer, Official, OfficialDuty, TrainerAttendance | Cascade |
| TrainingGroup → TrainingBlock | Restrict |
| TrainingBlock → TrainerAttendance | Cascade |
| Trainer → TrainerAttendance | Restrict |
| Official → OfficialDuty | Restrict |

`TrainerAttendance` heeft een unieke index op `(TrainingBlockId, TrainerId)`: er kan dus maximaal één aanwezigheidsrecord per trainer per trainingsblok bestaan. EF Core maakt tevens indexen voor de meeste foreign keys. De Indexen `UserNameIndex` en `RoleNameIndex` zijn in de laatste migratie uniek zonder een null-filter; dat is de gemigreerde toestand.

## Conventies en regels in data-toegang

Entiteiten gebruiken PascalCase CLR-eigenschappen en EF Core-conventietabelnamen (meervoudige `DbSet`-namen). Verplichte tekst en maximale lengtes komen uit data annotations. `DateOnly`/`TimeOnly` modelleren trainings- en dienstdatum/tijd; `CreatedAtUtc` krijgt in de CLR-modellen een `DateTime.UtcNow`-default. Services trimmen verplichte invoer en zetten lege optionele tekst om naar `null`.

Een `ClubId`-kolom bestaat op alle huidige niet-root bedrijfsentiteiten. Services toetsen bij creatie of een gerelateerde entiteit tot dezelfde club behoort. De database heeft echter geen samengestelde constraint die bijvoorbeeld afdwingt dat `TrainingBlock.ClubId` gelijk is aan die van de `TrainingGroup`; deze consistentie wordt in de huidige services bewaakt.

## Migrations

De aanwezige migraties maken achtereenvolgens Identity, clubs, trainingsplanning, trainers, traineraanwezigheid en officials/diensten aan. Maak een schemawijziging via EF Core-migraties, beoordeel de gegenereerde migratie en commit die samen met de model- en contextwijzigingen. Gebruik geen handmatige productiedatabasewijziging als vervanging voor een migratie.

Voorbeeld voor de huidige projectstructuur:

```powershell
dotnet ef migrations add <Beschrijving>
dotnet ef database update
```

Voer dit pas uit nadat model, contextconfiguratie en de impact op bestaande data zijn beoordeeld. De laatste migratie staat in namespace `SwimSuite.Web.Data.Migrations`, terwijl oudere migraties `SwimSuite.Data.Migrations` gebruiken; EF kan de migratie nog vinden via attributen/snapshot, maar nieuwe migraties moeten de bestaande contextstructuur volgen en hun namespace bewust worden gecontroleerd.

## Richtlijnen voor toekomstige wijzigingen

1. Breid een bestaand aggregate alleen uit wanneer de relatie en eigenaar duidelijk zijn.
2. Voeg `ClubId` toe waar een nieuwe bedrijfsentiteit clubgebonden is, maar presenteer dit niet als volledige tenantisolatie.
3. Configureer vereiste FK-relaties, deletegedrag, indexen en unieke regels expliciet wanneer conventies onvoldoende zijn.
4. Verplaats businessvalidatie niet uitsluitend naar de UI; handhaaf kritieke regels ook in de service en, waar passend, als databaseconstraint.
5. Werk [business rules](business-rules.md), domeindocumentatie en deze pagina bij met iedere schemawijziging.
