# ADR-001: Use ASP.NET Core MVC

**Status:** Accepted (afgeleid uit huidige code)

## Context

De app bevat MVC-controllers, Razor Views, tag helpers en een conventionele plus attribuutroute-inrichting.

## Decision

SwimSuite gebruikt ASP.NET Core MVC met Razor Views als huidige webpresentatielaag.

## Motivation

Dit ondersteunt server-rendered formulieren, modelvalidatie en de bestaande Identity UI zonder aparte frontendapplicatie.

## Consequences

Nieuwe schermen volgen controller → service → viewmodel/view. Dit is geen huidige REST API-architectuur.

## Future review

Een API of ander clientkanaal is toekomstig volgens de visie en vereist een afzonderlijke beslissing wanneer het concreet wordt.
