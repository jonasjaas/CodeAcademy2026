import { describe, expect, it } from "vitest";
import { render, screen } from "@testing-library/react";
import { OrderPreview } from "@/components/OrderPreview";
import { toPreviewItems } from "@/lib/orderDraft";

describe("OrderPreview", () => {
  it("viser label og verdi", () => {
    render(
      <OrderPreview>
        <OrderPreview.Item label="Kaffe" value="Latte" testId="preview-coffee" />
      </OrderPreview>,
    );
    expect(screen.getByLabelText("Din bestilling så langt")).toHaveTextContent("Kaffe");
    expect(screen.getByTestId("preview-coffee")).toHaveTextContent("Latte");
  });

  it("kan settes sammen av flere deler", () => {
    render(
      <OrderPreview>
        <OrderPreview.Item label="Navn" value="Ada" testId="a" />
        <OrderPreview.Item label="Antall" value="2" testId="b" />
      </OrderPreview>,
    );
    expect(screen.getByTestId("a")).toHaveTextContent("Ada");
    expect(screen.getByTestId("b")).toHaveTextContent("2");
  });

  it("rendrer tom oversikt uten deler", () => {
    render(<OrderPreview>{null}</OrderPreview>);
    expect(screen.getByRole("region", { name: "Din bestilling så langt" })).toBeInTheDocument();
  });
});

describe("toPreviewItems", () => {
  it("formaterer utkast", () => {
    const items = toPreviewItems({
      coffeeName: "Latte",
      customerName: " Ada ",
      size: "LARGE",
      milkType: "OAT",
      extraShot: true,
      quantity: 2,
    });
    expect(items.map((i) => i.value)).toEqual(["Latte", "Ada", "Stor", "Havre", "Ja", "2"]);
  });

  it("bruker fallback for tomme valg", () => {
    const items = toPreviewItems({
      coffeeName: "",
      customerName: "",
      size: "MEDIUM",
      milkType: "",
      extraShot: false,
      quantity: 1,
    });
    expect(items.map((i) => i.value)).toEqual(["–", "–", "Medium", "Ingen", "Nei", "1"]);
  });
});
