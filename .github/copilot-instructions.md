# Copilot instructions

Norwegian-language workshop monorepo: docs, comments and commit messages are in Norwegian. Each numbered folder is one session building on the same domain (RabbitMQ events → Postgres → API → frontend). No root build; each project is built on its own.

## Architecture

- `3-APIs/kaffebar-{java,dotnet}` is the **starter**, `*-solution` the **solution** (`FASIT.md`). Both implement the same contract, so the frontend works against either.
- Data flow in `4-Frontend/kaffebar-web`: API → RabbitMQ `kaffebar` exchange → `src/app/api/events/route.ts` → SSE → `useOrders` (SSE + TanStack Query). The browser only talks to Next (BFF); server config in `src/lib/config.ts` must not use `NEXT_PUBLIC_`.
- `src/types/kaffebar.ts` is generated from the API's OpenAPI spec (`yarn generate:types`, needs the API running).

## Starter vs. solution

Starters contain `// TODO` stubs; answers sit next to them with a `-solution` suffix (`useEventSource-solution.ts`, `Dockerfile-solution`, `*-solution/`). Never overwrite a stub with its solution or edit starter files unless asked. Keep starter and solution structurally in sync.

## Commands

- Idempotweet: root `justfile` (`just dev`, `just test`, …). `COMPOSE` in `.env` switches docker/podman.
- Next.js projects: `corepack enable` first (yarn). Single test: `yarn vitest run __tests__/<file>.test.ts` or `-t "<name>"`. `yarn test:coverage` is idempotweet only.
- `USE_MOCK_API=true yarn dev` runs kaffebar-web without Docker/API/RabbitMQ.
- Java: `mvn clean verify` in `kaffebar-java-solution` generates code from the OpenAPI yaml and runs tests.
- .NET: starter listens on :5035; tests via `dotnet test` in `kaffebar-dotnet-solution`.
- Full stack: `docker compose --profile java|dotnet up --build` (only one, both bind :8080); optional `--profile frontend|consumer|idempotweet`.

## Gotchas

- **Contract-first:** change `src/main/resources/api/kaffebar-api.yaml` before the Java implementation.
- Events/auto-barista are off by default; compose enables them via env vars (`KAFFEBAR_EVENTS_ENABLED`, .NET: `Kaffebar__Events__Enabled`).
- **Next.js has breaking changes:** read the relevant guide in `node_modules/next/dist/docs/` before writing Next code in `4-Frontend/kaffebar-web` (see its `AGENTS.md`).
- `NEXT_PUBLIC_*` vars are baked in at build time; Dockerfiles take them as build args.
- Frontend follows `4-Frontend/BEST-PRACTICE.md` (no data fetching in `useEffect`).
- Never read `.env` files or anything outside the repository.
- Components must be reusable and have vitest browser tests.
