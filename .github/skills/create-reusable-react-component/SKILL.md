---
name: create-reusable-react-component
description: Lager en ny gjenbrukbar, compound React-komponent med tilhørende vitest-test i kaffebar-web (Next.js). Bruk når du skal opprette, legge til eller trekke ut en UI-komponent i 4-Frontend/kaffebar-web, f.eks. en oversikt, kort eller badge etter mønsteret til CoffeeSummary, men uten domenelogikk inni komponenten.
---

# Lag en gjenbrukbar compound-komponent

Utgangspunkt: `src/components/CoffeeSummary.tsx` og `__tests__/coffee-summary.test.tsx` i `4-Frontend/kaffebar-web`.

CoffeeSummary tar inn `orders` og teller/filtrerer selv. Det er det vi vil unngå: logikk skal ikke havne inne i komponentene. Komponentene skal være «dumme», sammensatte (compound) byggeklosser som bare viser det de får.

Merk: Next.js-versjonen har breaking changes. Les relevant guide i `node_modules/next/dist/docs/` før du bruker Next-spesifikke APIer (se `AGENTS.md`).

## Prinsipper

- **Compound:** del komponenten i små deler som settes sammen med `children` (f.eks. `Summary` + `Summary.Stat`) i stedet for én stor komponent med mange props og interne valg.
- **Primitive props:** send inn `label`, `value`, `name`, `status` osv., ikke hele domeneobjekter som `Order`/`Order[]` så langt det lar seg gjøre.
- **Ingen logikk inni:** filtrering, summering, mapping og formatering gjøres av kalleren (side, hook eller hjelpefunksjon utenfor komponenten) og sendes inn ferdig.

## Steg

1. **Avklar delene.** Skriv ned hvilke deler UI-et består av (rot + elementer) og hvilke enkle verdier hver del trenger. Eksempel: `CoffeeSummary` → rot `CoffeeSummary` + del `CoffeeSummary.Stat` med `label`, `value`, `testId`.
2. **Sjekk eksisterende komponenter** i `src/components/` (`StatusBadge`, `EmptyState`, `OrderCard` osv.) og gjenbruk dem før du lager noe nytt.
3. **Opprett `src/components/<Navn>.tsx`.**
   - PascalCase filnavn og navngitte eksporter (ikke default export).
   - `"use client";` bare hvis komponenten trenger hooks eller nettleser-APIer.
   - Importer med `@/`-aliaset; gjenbruk typer fra `@/types/domain` for enkeltfelt (f.eks. `Order["status"]`), ikke hele objekter.
   - Rotkomponenten rendrer `children`; delene er egne komponenter hentet fra samme fil og tilgjengeliggjort via `Object.assign(Root, { Stat })`.
   - Ingen `filter`/`reduce`/beregninger i komponenten. Eneste logikk er ren visning.
   - Kort JSDoc (norsk) over komponenten som forklarer hva den viser.
4. **Gjør markup tilgjengelig og testbar.** Semantiske elementer med `aria-label` på roten. La `data-testid` komme inn som prop (f.eks. `testId`) slik at kalleren styrer det. Style med Tailwind og prosjektets tokens (`border-line`, `bg-accent-soft`, `text-secondary`).
5. **Flytt logikken til kalleren.** Der komponenten brukes (f.eks. `BaristaBoard.tsx`) beregner du verdiene (gjerne med en hjelpefunksjon som `cups`, utenfor komponenten) og sender dem inn:
   ```tsx
   <CoffeeSummary>
     <CoffeeSummary.Stat label="Kaffe laget" value={made} testId="coffee-made" />
     <CoffeeSummary.Stat label={STATUS_LABEL.PENDING} value={pending} testId="coffee-pending" />
   </CoffeeSummary>
   ```
6. **Skriv test i `__tests__/<kebab-navn>.test.tsx`.**
   - Importer `describe, expect, it` fra `vitest` og `render, screen` fra `@testing-library/react`.
   - Test med enkle verdier direkte, uten domeneobjekter eller `make(...)`-hjelpere.
   - Dekk: standard/tomt tilfelle, at labels og verdier vises, og at flere deler kan settes sammen.
   - Bruk `getByTestId`/`getByRole`/`getByLabelText` og `toHaveTextContent`.
   - Testbeskrivelser på norsk. Logikken som ble flyttet ut testes separat der den bor.
7. **Kjør lint og testene** fra `4-Frontend/kaffebar-web` (kjør `corepack enable` først om yarn mangler):
   ```bash
   yarn lint
   yarn vitest run __tests__/<kebab-navn>.test.tsx   # den nye testen
   yarn test                                          # hele suiten
   ```
   Rett opp feil til alt er grønt.
