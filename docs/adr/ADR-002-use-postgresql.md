# ADR-002: Use PostgreSQL

**Status:** Accepted (afgeleid uit huidige code en configuratie)

## Context

`Program.cs` configureert `UseNpgsql` en het project refereert naar de Npgsql EF Core-provider.

## Decision

De huidige relationele database is PostgreSQL.

## Motivation

De bestaande EF Core-context, migraties en connection string zijn op PostgreSQL ingericht.

## Consequences

Schemawijzigingen lopen via Npgsql/EF Core-migraties; provider-specifieke migratie-uitvoer moet worden beoordeeld.

## Future review

Een verandering van databaseprovider is niet voorzien in de huidige visie en vraagt een expliciete migratiebeslissing.
