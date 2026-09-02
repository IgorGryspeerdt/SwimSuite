# Permissions and authentication

## Huidige toestand

SwimSuite gebruikt ASP.NET Core Default Identity met `IdentityUser` en EF Core-opslag. Registratie, login, logout en accountbeheer worden via Identity Razor Pages aangeboden. De configuratie vereist een bevestigd account om aan te melden (`SignIn.RequireConfirmedAccount = true`).

`ClubsController`, `TrainingScheduleController`, `TrainersController`, `TrainerAttendanceController`, `OfficialsController` en `OfficialDutiesController` zijn gemarkeerd met `[Authorize]`. `HomeController` is publiek. Binnen de bedrijfscontrollers bestaat geen verdere actieniveaubeperking.

| Gebruikerstoestand | Toegang volgens controllerattributen |
| --- | --- |
| Niet aangemeld | Home en privacy; bedrijfscontrollers vereisen autorisatie. |
| Aangemeld | Alle huidige club-, training-, trainer-, attendance-, official- en duty-acties. |

## Rollen, policies en tenantrechten

Er zijn momenteel geen geconfigureerde applicatierollen, authorization policies, claims, role checks of gebruiker-clubrelaties. Hoewel Identity-migraties `AspNetRoles` en `AspNetUserRoles` bevatten, registreert de applicatie `AddDefaultIdentity<IdentityUser>` zonder role services en gebruikt geen rollen in code. De `Role` op `OfficialDuty` is een taakomschrijving, geen Identity-rol.

De app autoriseert ook niet per club: een geautoriseerde gebruiker wordt niet gekoppeld aan `ClubId`. `ClubId` in services biedt domeinconsistentie, geen gebruikersrechten. Zie [Architecture](ARCHITECTURE.md#identity-authenticatie-en-autorisatie) voor de huidige pipeline-afwijking: `UseAuthentication()` ontbreekt.

## Toekomstige richting

De visie noemt boardleden, administrators, trainercoördinatoren, trainers, officials en vrijwilligers als doelgebruikers. Dat zijn doelgroepen, geen huidige rollen of rechten. Een toekomstige multi-tenant SaaS-inrichting zal vóór rolrechten minimaal een expliciete gebruiker-clubmembership en tenantresolutie moeten ontwerpen. Rollen, policies en matrixen worden pas als geïmplementeerd gedocumenteerd nadat ze in configuratie, data en endpoints zijn afgedwongen.
