# ADR-005: Use a service layer for application logic

**Status:** Accepted (afgeleid uit huidige code en `docs/AGENTS.md`)

## Context

De app heeft serviceinterfaces en scoped implementaties voor elk huidig bedrijfsdomein. Controllers delegeren hun hoofdhandelingen aan die services.

## Decision

Business- en data-toeganglogica hoort primair in services achter interfaces; controllers blijven dun.

## Motivation

Dit houdt MVC-acties klein, centraliseert club-/relatievalidatie en ondersteunt testbaarheid en hergebruik.

## Consequences

Nieuwe use cases krijgen normaliter een service. `OfficialDutiesController` bevat huidige directe contextqueries voor UI-keuzelijsten; behandel dat als bestaande uitzondering, niet als patroon.

## Future review

De grens kan worden aangescherpt wanneer de applicatie groeit, maar een extra laag is nu niet gerechtvaardigd door de codebasis.
