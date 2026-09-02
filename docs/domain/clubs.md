# Clubs

## Doel en huidige functionaliteit

`Club` is de huidige organisatorische root. Geautoriseerde gebruikers kunnen clubs weergeven, een detailpagina openen en een club aanmaken. De detailpagina linkt naar training, trainers, officials en official duties van die club.

## Entiteit en relaties

`Club` heeft naam, optionele registratie-/contactgegevens en `CreatedAtUtc`. Het heeft collecties voor training groups, training blocks, trainers, officials en official duties. Alle huidige subdomeinen dragen een `ClubId`; het verwijderen van een club cascadeert op database-niveau naar die records.

## Regels en beperkingen

De naam is verplicht en maximaal 160 tekens. Zie [Business rules](../business-rules.md) en [Database](../DATABASE.md). `ClubId` organiseert de data, maar is nog geen autorisatie- of tenantgrens voor Identity-gebruikers.

## Toekomstige richting

De visie ondersteunt meerdere clubs en configureerbare clubinstellingen. Membership, tenantisolatie en clubbeheerrechten zijn nog niet geïmplementeerd.
