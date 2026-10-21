import type { ReactNode } from "react";

/** Foreløpig oversikt over hva kunden har valgt. Viser bare det den får inn. */
function OrderPreviewRoot({ children }: { children: ReactNode }) {
  return (
    <section aria-label="Din bestilling så langt" className="rounded-lg border border-line bg-accent-soft px-4 py-3">
      <h2 className="mb-2 text-sm font-semibold">Du har valgt</h2>
      <dl className="flex flex-col gap-1.5">{children}</dl>
    </section>
  );
}

function OrderPreviewItem({ label, value, testId }: { label: string; value: string; testId?: string }) {
  return (
    <div className="flex justify-between gap-4 text-sm">
      <dt className="text-secondary">{label}</dt>
      <dd className="font-medium" data-testid={testId}>
        {value}
      </dd>
    </div>
  );
}

export const OrderPreview = Object.assign(OrderPreviewRoot, { Item: OrderPreviewItem });
