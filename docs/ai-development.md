# AI-assisted development

## Doel en bronvolgorde

Gebruik `/docs` als werkcontext, maar verifieer elke wijziging in de code. Lees bij een nieuwe sessie in deze volgorde:

1. [Vision](VISION.md)
2. [Architecture](ARCHITECTURE.md)
3. [Coding standards](coding-standards.md)
4. [Business rules](business-rules.md) en [Permissions](permissions.md)
5. Het relevante document in [domain](domain/)
6. [Database](DATABASE.md), relevante migraties en modellen
7. [Roadmap](ROADMAP.md) en de relevante ADR in [adr](adr/)

## Workflow voor nieuwe functionaliteit

1. Bepaal of de vraag huidige functionaliteit uitbreidt of een toekomstige richting uit de visie betreft.
2. Lees de relevante documenten en inspecteer vervolgens controllers, services, modellen, views, context en migraties die het gedrag werkelijk bepalen.
3. Hergebruik bestaande service-, viewmodel-, route- en validatiepatronen. Analyseer gevolgen voor clubscoping, relaties, autorisatie, UI en bestaande data.
4. Implementeer alleen wat door de opdracht en huidige context wordt gedragen. Houd controllers dun en plaats use-case- of data-integriteitslogica in een service.
5. Bij een schemawijziging: wijzig model en contextconfiguratie, maak en beoordeel een EF Core-migratie, en pas documenten met entiteiten/regels aan.
6. Bouw en test de betrokken flows. Er is nu geen testproject; voeg of voer passende tests uit voor nieuwe logica wanneer de taak dat vraagt.
7. Werk documentatie en eventueel ADR bij zodra architectuur, database, businessregels of een geplande richting materieel verandert.

## Architectuurregels

- Injecteer services via interfaces en registreer ze scoped in `Program.cs`. Gebruik `ApplicationDbContext` in services; voeg geen repository of nieuwe technologie toe zonder concrete noodzaak.
- Controllers verwerken HTTP, modelvalidatie en view/redirect-resultaten. Gebruik routewaarden als gezaghebbend voor `ClubId`; vertrouw niet op een verborgen formulierveld.
- Gebruik asynchrone EF Core-calls met `CancellationToken`, `AsNoTracking()` voor reads en expliciete clubfilters voor clubgebonden data. Verifieer dat gerelateerde IDs bij dezelfde club horen.
- Voeg een entiteit samen met zijn `DbSet`, relaties/deletegedrag, migratie en relevante viewmodellen toe. Beoordeel indexes en unieke constraints expliciet.
- Bescherm bestaand gedrag: respecteer huidige FK-deletegedrag en de unieke attendance-regel. Gebruik data annotations plus servicevalidatie voor nieuwe regels.
- Controleer vóór aanpassing van authenticatie/autorisation de actuele beperkingen in [Permissions](permissions.md): er zijn geen rollen of clubmemberships en `UseAuthentication()` ontbreekt nu.

## Gedragsregels voor AI

- Onderzoek eerst bestaande code en volg aantoonbare patronen.
- Verzin geen functionaliteit, rollen, multi-tenancy of integraties. Label visie-/roadmapitems als toekomstig zolang code ze niet ondersteunt.
- Hergebruik componenten waar dat past; introduceer geen abstractions, packages of technologieën zonder duidelijke, taakgebonden reden.
- Breek geen bestaande routes, validatie, migraties of clubscoping. Meld onzekerheid of een product-/architectuurbeslissing aan de projectowner.
- Actualiseer documentatie als een wijziging de architectuur, database, regels, rechten of domeinstatus verandert.
