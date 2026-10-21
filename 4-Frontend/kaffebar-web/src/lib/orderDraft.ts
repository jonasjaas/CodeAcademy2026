import { MILK_LABEL, SIZE_LABEL, type CoffeeSize, type MilkType } from "@/types/domain";

/** Kundens pågående valg i bestillingsskjemaet. */
export type OrderDraft = {
  coffeeName: string;
  customerName: string;
  size: CoffeeSize;
  milkType: MilkType | "";
  extraShot: boolean;
  quantity: number;
};

export type PreviewItem = { key: string; label: string; value: string };

/** Gjør et utkast om til ferdig formaterte label/verdi-par for visning. */
export function toPreviewItems(draft: OrderDraft): PreviewItem[] {
  return [
    { key: "coffee", label: "Kaffe", value: draft.coffeeName || "–" },
    { key: "name", label: "Navn", value: draft.customerName.trim() || "–" },
    { key: "size", label: "Størrelse", value: SIZE_LABEL[draft.size] },
    { key: "milk", label: "Melk", value: draft.milkType ? MILK_LABEL[draft.milkType] : "Ingen" },
    { key: "extra-shot", label: "Ekstra shot", value: draft.extraShot ? "Ja" : "Nei" },
    { key: "quantity", label: "Antall", value: String(draft.quantity) },
  ];
}
