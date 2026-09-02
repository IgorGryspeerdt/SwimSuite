# Business rules

## Leeswijzer

Hieronder staan alleen regels die zichtbaar zijn in huidige modellen, services, controllers of views. **Toekomstig** betekent dat de regel uit [Vision](VISION.md) of [Roadmap](ROADMAP.md) komt en nog niet is geïmplementeerd.

## Algemeen en clubgrens

- Clubs kunnen worden aangemaakt en bekeken door geautoriseerde gebruikers.
- Een training group, training block, trainer, official, attendance record en official duty is gekoppeld aan precies één club (`ClubId`).
- De services accepteren een gerelateerd record alleen als het bestaat binnen dezelfde club: een block moet bij zijn gekozen group horen, een duty bij zijn official en attendance bij zijn trainer en block.
- Dit is geen autorisatieregel per gebruiker: huidige code koppelt geen Identity-gebruiker aan een club. Elke geautoriseerde gebruiker kan een bekende clubroute benaderen.

## Invoerregels

| Onderwerp | Huidige regel |
| --- | --- |
| Club | Naam verplicht, max. 160; registratie (40), e-mail (160, e-mailformaat), telefoon (80, telefoonformaat) en adres (240) optioneel. |
| Training group | Naam verplicht, max. 120; omschrijving optioneel, max. 400. |
| Training block | Gekozen group moet bij de routeclub horen; eindtijd moet later zijn dan begintijd; locatie max. 160 en notities max. 400. |
| Trainer | Voor- en achternaam verplicht/max. 120; e-mail en telefoon zijn optioneel maar worden gevalideerd als ze zijn ingevuld; status is actief/inactief. |
| Official | Zelfde persoons- en contactregels als trainer, plus optioneel licentienummer max. 80; status is actief/inactief. |
| Official duty | Official is verplicht en moet bij de club horen; datum, wedstrijdnaam/max. 160 en rol/max. 120 zijn verplicht; locatie/max. 160 en notities/max. 400 zijn optioneel. |
| Trainer attendance | Maximaal één record per combinatie trainingsblok en trainer; notities max. 400. |

Lege optionele tekst wordt bij creatie/opslaan opgeslagen als `null`; overige tekst wordt getrimd. De beschreven data annotations worden door MVC-modelvalidatie gebruikt. Niet elke GUID-property draagt een `[Required]`-annotatie; controllers vullen routewaarden in en services doen relatietoetsen.

## Aanwezigheid en statusgedrag

- Het aanwezigheidsformulier toont uitsluitend actieve trainers van de club. Ontbrekende attendance wordt daar als niet aanwezig voorgesteld.
- Bij opslaan worden records per aangeboden trainer gecreëerd of bijgewerkt; de unieke database-index voorkomt dubbelen per block/trainer.
- Het opslaan verifieert dat het trainingsblok bestaat en dat alle ingediende trainers tot de club behoren. De code sluit inactieve trainers niet uit bij een handmatig POST-verzoek; alleen de UI filtert ze uit.
- `IsActive` heeft geen statusovergang of deactiveringsscherm in de huidige UI. Het is een bewaarde boolean; bij creation staat de default op `true`.
- Een official duty mag volgens de UI alleen aan een actieve official worden toegekend, omdat de keuzelijst daarop filtert. De service valideert alleen clubtoebehoren, niet `IsActive`; een handmatig POST-verzoek kan dus een inactieve official gebruiken.

## Verwijderen en integriteit

Er bestaan geen delete-acties in de MVC-app. Het datamodel definieert wel cascadeverwijdering van club naar onderliggende records, cascade van training block naar attendance, en restrict voor group→block, trainer→attendance en official→duty. Verwijderfunctionaliteit moet deze gevolgen expliciet behandelen voordat zij wordt toegevoegd.

## Toekomstig volgens visie/roadmap

Rollen/gebaseerde rechten, vergoedingsberekeningen, maandelijkse overzichten, Excel-export, leden, competitiemanagement, rapportage, communicatie en notificaties zijn nog niet geïmplementeerd. De visie noemt onder meer hergebruik van attendance voor vergoedingen; dat is een productrichting, geen huidige berekening of regel.
