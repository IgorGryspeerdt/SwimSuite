# Officials and official duties

## Doel en huidige functionaliteit

Een club kan officials aanmaken en weergeven, en official duties registreren. Een duty bevat een datum, wedstrijdnaam, taakrol, eventuele locatie/notities en verwijst naar één official.

## Regels en relaties

Officials zijn clubgebonden en hebben verplichte namen, optionele contact-/licentiegegevens en `IsActive`. Bij het aanmaken van een duty biedt de UI alleen actieve officials van die club aan. De service dwingt af dat de gekozen official bij dezelfde club hoort, maar toetst de actieve status niet bij directe POST. Een official met bestaande duties kan niet worden verwijderd vanwege restrict-deletegedrag.

De duty-`Role` is tekst over de taak bij de wedstrijd en staat los van ASP.NET Core Identity-rollen. Zie [Business rules](../business-rules.md).

## Toekomstige richting

Competitiemanagement en vergoedingen worden in de visie/README genoemd, maar zijn niet geïmplementeerd. Een official duty is nu alleen een registratie; er bestaat geen competition-entiteit of vergoedingsoverzicht.
