# ADR-006: Prepare for SaaS and multi-tenancy

**Status:** Proposed — future direction

## Context

[Vision](../VISION.md) noemt meerdere clubs en SaaS als langetermijnrichting. Huidige bedrijfsentiteiten hebben veelal `ClubId` en services filteren daarop.

## Decision

De huidige code bereidt clubgebonden data voor, maar implementeert nog geen volledige multi-tenancy. Volledige tenantisolatie wordt uitgesteld totdat requirements voor membership, rollen en tenantresolutie zijn besloten.

## Motivation

Dit respecteert de visie zonder onvolwassen tenantarchitectuur te presenteren als bestaand.

## Consequences

`ClubId` moet bij nieuwe clubgebonden data bewust worden behandeld. Een geautoriseerde gebruiker krijgt nu geen clubgebonden rechten; routes kunnen niet als beveiligingsgrens dienen.

## Future review

Beslis later over gebruiker-clubmembership, tenantresolutie, queryfilters, rollen/policies, migratie van bestaande data en beheerflows.
