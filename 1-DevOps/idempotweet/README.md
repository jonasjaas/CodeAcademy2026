# Idempotweet

A demo application showcasing a Twitter-like feed of "idems" (idempotent tweets) built with Next.js 16, React 19, and TanStack Query.

## Features

- **Infinite scroll** - 200 idems loaded progressively as you scroll
- View a feed of idems sorted by newest first
- Relative timestamps (e.g., "5 minutes ago")
- Modern, smooth design with fade-in animations
- Loading, error, and empty states
- 5-minute client-side caching with TanStack Query
- **Comprehensive test suite** for CI/CD demos

## Tech Stack

- **Next.js 16** with App Router and React 19
- **TanStack React Query v5** with `useInfiniteQuery` for infinite scroll
- **PostgreSQL 17** for data storage
- **node-postgres (pg)** for database connectivity
- **Tailwind CSS v4** for styling
- **date-fns** for timestamp formatting
- **react-intersection-observer** for scroll detection
- **TypeScript 5.x** with strict mode
- **Vitest** + **React Testing Library** + **MSW** for testing

## Getting Started

### Prerequisites

- Node.js 20+
- Yarn (recommended)
- Docker and Docker Compose (for PostgreSQL)

### Quick Start with Docker Compose

```bash
# From project root, start PostgreSQL
docker compose up postgres -d

# Install dependencies
yarn install

# Seed the database with demo data
yarn seed

# Run development server
yarn dev
```

Open [http://localhost:3000](http://localhost:3000) to view the application.

The `yarn dev` and `yarn seed` commands automatically use the local database URL. To override, set `DATABASE_URL` before running.

### Environment Variables

| Variable       | Description                  | Default (dev)                                                     |
| -------------- | ---------------------------- | ----------------------------------------------------------------- |
| `DATABASE_URL` | PostgreSQL connection string | `postgresql://codeacademy:codeacademy@localhost:5432/codeacademy` |

### Database Commands

```bash
# Seed the database with 200 demo idems
yarn seed

# Use a custom database URL
DATABASE_URL=postgresql://user:pass@host:5432/db yarn seed
```

### Build

```bash
yarn build
yarn start
```

## Testing

```bash
# Run all tests
yarn test

# Run tests in watch mode
yarn test:watch

# Run tests with coverage report
yarn test:coverage
```

### Test Structure

```
__tests__/
├── unit/                    # Unit tests
│   ├── demo-data.test.ts
│   ├── generate-demo-data.test.ts
│   └── date-utils.test.ts
├── components/              # Component tests
│   ├── IdemCard.test.tsx
│   ├── IdemsFeed.test.tsx
│   ├── LoadingSpinner.test.tsx
│   ├── EmptyState.test.tsx
│   ├── ErrorState.test.tsx
│   └── EndOfFeed.test.tsx
├── integration/             # Integration tests
│   └── api-idems.test.ts
└── setup.ts                 # Test setup with MSW
```

## Project Structure

```
src/
├── app/
│   ├── api/idems/route.ts    # Paginated API endpoint (PostgreSQL)
│   ├── components/           # UI components
│   │   ├── EmptyState.tsx
│   │   ├── EndOfFeed.tsx
│   │   ├── ErrorState.tsx
│   │   ├── IdemCard.tsx
│   │   ├── IdemsFeed.tsx
│   │   └── LoadingSpinner.tsx
│   ├── globals.css           # Global styles with design tokens
│   ├── layout.tsx            # Root layout
│   ├── page.tsx              # Homepage
│   └── providers.tsx         # QueryClient provider
├── hooks/
│   └── useIdems.ts           # Infinite query hook
├── lib/
│   ├── db.ts                 # PostgreSQL connection pool & queries
│   ├── demo-data.ts          # 200 demo idems (legacy)
│   └── generate-demo-data.ts # Data generator utility
└── types/
    └── idem.ts               # TypeScript interfaces
scripts/
└── seed.ts                   # Database seed script
```

## API

### GET /api/idems

Returns a paginated response of idems sorted by `createdAt` descending.

**Query Parameters:**

- `page` (default: 1) - Page number (1-indexed)
- `pageSize` (default: 20) - Items per page (max: 100)

**Response:**

```typescript
interface PaginatedResponse {
  items: Idem[];
  page: number;
  pageSize: number;
  totalCount: number;
  hasMore: boolean;
  nextPage: number | null;
}

interface Idem {
  id: string;
  author: string;
  content: string; // max 280 characters
  createdAt: string; // ISO 8601 timestamp
}
```

**Example:**

```bash
curl http://localhost:3000/api/idems?page=1
```

## Database

### Schema

The application uses a single `idems` table:

```sql
CREATE TABLE idems (
  id VARCHAR(20) PRIMARY KEY,
  author VARCHAR(50) NOT NULL,
  content VARCHAR(280) NOT NULL,
  created_at TIMESTAMP WITH TIME ZONE NOT NULL
);

CREATE INDEX idx_idems_created_at ON idems(created_at DESC);
```

The table is auto-created on first API request if it doesn't exist.

### Connection

The database connection uses a pool with:

- Max 10 connections
- 30s idle timeout
- 2s connection timeout

Configure via `DATABASE_URL` environment variable.
