# Training and trainer attendance

## Doel en huidige functionaliteit

Een club kan training groups en losse training blocks aanmaken en in datum/tijd-volgorde bekijken. Voor ieder block kan een geautoriseerde gebruiker de aanwezigheid van actieve trainers registreren. Er is geen terugkerende planning, deelnemerslijst, sporterattendance of vergoedingsberekening.

## Entiteiten en relaties

- `TrainingGroup` hoort bij één club en groepeert blocks.
- `TrainingBlock` hoort bij één club én één training group, met datum, begin-/eindtijd, locatie en notities.
- `TrainerAttendance` koppelt één trainer aan één trainingsblok en bevat aanwezig/niet-aanwezig en notities.

Een group kan niet worden verwijderd zolang er blocks aan gekoppeld zijn; een verwijderd block cascadeert naar zijn attendance. Zie [Database](../DATABASE.md).

## Regels

Een block moet een group van dezelfde club gebruiken en eindigt later dan het begint. Het attendanceformulier toont alleen actieve trainers. Eén trainer heeft maximaal één attendance-record per block. De service verifieert bij opslaan clubtoebehoren van block en trainers. Volledige regels staan in [Business rules](../business-rules.md).

## Toekomstige richting

De visie noemt planning, leden en hergebruik van attendance voor vergoedingen en rapportage. Die functies bestaan nog niet; huidige attendance betreft uitsluitend trainers.
