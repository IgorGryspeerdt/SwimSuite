# ADR-004: Use Entity Framework Core

**Status:** Accepted (afgeleid uit huidige code)

## Context

De code gebruikt `ApplicationDbContext`, `DbSet`s, LINQ, Fluent-relatieconfiguratie en migraties.

## Decision

SwimSuite gebruikt Entity Framework Core als ORM en migratievoorziening.

## Motivation

EF Core integreert met Identity en maakt het huidige relationele model en de code-firstmigraties mogelijk.

## Consequences

Services gebruiken de context rechtstreeks. Model- en relatieveranderingen vereisen een gecontroleerde migratie.

## Future review

Een repositorylaag of andere persistence-aanpak is niet gekozen en moet alleen worden overwogen bij een concrete behoefte.
