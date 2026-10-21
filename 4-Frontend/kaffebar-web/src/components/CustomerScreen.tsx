"use client";

import { useState } from "react";
import { OrderForm } from "./OrderForm";
import { MyOrderTracker } from "./MyOrderTracker";
import { OrderPreview } from "./OrderPreview";
import { toPreviewItems, type OrderDraft } from "@/lib/orderDraft";
import type { Coffee, Order } from "@/types/domain";

/** Limet mellom skjemaet og «din bestilling»-kortet. Utlevert. */
export function CustomerScreen({
  menu,
  initialOrders,
}: {
  menu: Coffee[];
  initialOrders: Order[];
}) {
  const [myOrderId, setMyOrderId] = useState<string | null>(null);
  const [draft, setDraft] = useState<OrderDraft>({
    coffeeName: menu[0]?.name ?? "",
    customerName: "",
    size: "MEDIUM",
    milkType: "",
    extraShot: false,
    quantity: 1,
  });

  return (
    <div className="grid gap-8 lg:grid-cols-[1fr_20rem]">
      <OrderForm
        menu={menu}
        onOrdered={(order) => setMyOrderId(order.orderId)}
        onSelectionChange={setDraft}
      />
      <aside className="flex flex-col gap-4 lg:sticky lg:top-6 lg:self-start">
        <OrderPreview>
          {toPreviewItems(draft).map((item) => (
            <OrderPreview.Item
              key={item.key}
              label={item.label}
              value={item.value}
              testId={`preview-${item.key}`}
            />
          ))}
        </OrderPreview>
        <MyOrderTracker orderId={myOrderId} initialOrders={initialOrders} />
      </aside>
    </div>
  );
}
