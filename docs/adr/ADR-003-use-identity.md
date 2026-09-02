# ADR-003: Use ASP.NET Core Identity

**Status:** Accepted (afgeleid uit huidige code)

## Context

De context erft van `IdentityDbContext`, Default Identity is geregistreerd en Identity Razor Pages zijn gemapt.

## Decision

De applicatie gebruikt ASP.NET Core Identity met `IdentityUser` en EF Core-opslag voor accounts.

## Motivation

Dit levert de bestaande registratie-, login-, logout- en accountbeheerflows.

## Consequences

Bevestigde accounts zijn vereist bij login. Er zijn nog geen app-rollen, policies of clubmemberships; bovendien ontbreekt momenteel `UseAuthentication()` in de pipeline. Zie [Permissions](../permissions.md).

## Future review

Rollen en multi-tenant identitymodellering moeten worden besloten vóór implementatie, niet afgeleid uit de aanwezige Identity-tabellen.
