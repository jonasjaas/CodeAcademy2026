"use client";

import { STATUS_LABEL, type Order } from "@/types/domain";

const cups = (orders: Order[], status: Order["status"]) =>
  orders
    .filter((order) => order.status === status)
    .reduce((sum, order) => sum + order.quantity, 0);

/** Oversikt til baristaen: antall kopper kaffe laget (klare) og hvor mange som gjenstår. */
export function CoffeeSummary({ orders }: { orders: Order[] }) {
  const made = cups(orders, "READY");

  return (
    <section aria-label="Kaffeoversikt" className="flex flex-wrap items-center gap-6 rounded-lg border border-line bg-accent-soft px-4 py-3">
      <div>
        <p className="text-xs text-secondary">Kaffe laget</p>
        <p className="text-3xl font-bold" data-testid="coffee-made">
          {made}
        </p>
      </div>
      {(["PENDING", "BREWING"] as const).map((status) => (
        <div key={status}>
          <p className="text-xs text-secondary">{STATUS_LABEL[status]}</p>
          <p className="text-xl font-semibold" data-testid={`coffee-${status.toLowerCase()}`}>
            {cups(orders, status)}
          </p>
        </div>
      ))}
    </section>
  );
}
