import { describe, expect, it } from "vitest";
import { render, screen } from "@testing-library/react";
import { CoffeeSummary } from "@/components/CoffeeSummary";
import type { Order } from "@/types/domain";

const make = (i: number, status: Order["status"], quantity = 1): Order => ({
  orderId: `0de50001-0000-4000-8000-00000000000${i}`,
  coffeeId: "c0ffee01-0000-4000-8000-000000000007",
  coffeeName: "Kaffe Latte",
  customerName: "Ada",
  size: "MEDIUM",
  milkType: "OAT",
  quantity,
  status,
  createdAt: new Date().toISOString(),
  updatedAt: new Date().toISOString(),
});

describe("CoffeeSummary", () => {
  it("viser 0 når ingen ordrer finnes", () => {
    render(<CoffeeSummary orders={[]} />);
    expect(screen.getByTestId("coffee-made")).toHaveTextContent("0");
    expect(screen.getByTestId("coffee-pending")).toHaveTextContent("0");
    expect(screen.getByTestId("coffee-brewing")).toHaveTextContent("0");
  });

  it("teller kopper (antall) for ferdige ordrer", () => {
    render(<CoffeeSummary orders={[make(1, "READY", 2), make(2, "READY"), make(3, "PENDING")]} />);
    expect(screen.getByTestId("coffee-made")).toHaveTextContent("3");
  });

  it("viser kopper i kø og under arbeid separat", () => {
    render(<CoffeeSummary orders={[make(1, "PENDING", 3), make(2, "BREWING", 2), make(3, "READY")]} />);
    expect(screen.getByTestId("coffee-pending")).toHaveTextContent("3");
    expect(screen.getByTestId("coffee-brewing")).toHaveTextContent("2");
    expect(screen.getByTestId("coffee-made")).toHaveTextContent("1");
  });
});
