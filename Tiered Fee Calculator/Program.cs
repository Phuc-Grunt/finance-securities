Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

Console.Write("Số tiền giao dịch (VNĐ): ");
decimal soTien = decimal.Parse(Console.ReadLine()!);

decimal phiGiaoDich = CalculateTradingFee(soTien);
Console.WriteLine($"Phí giao dịch: {phiGiaoDich:N2} VNĐ");

static decimal CalculateTradingFee(decimal totalOrderValue)
{
    if (totalOrderValue < 0)
    {
        Console.WriteLine("Số tiền giao dịch không hợp lệ. Vui lòng nhập số tiền lớn hơn hoặc bằng 0.");
        return 0;
    }

    decimal fee;

    if (totalOrderValue <= 100_000_000m)
    {
        fee = totalOrderValue * 0.0015m;
    }
    else if (totalOrderValue <= 500_000_000m)
    {
        fee = 100_000_000m * 0.0015m
            + (totalOrderValue - 100_000_000m) * 0.0012m;
    }
    else
    {
        fee = 100_000_000m * 0.0015m
            + 400_000_000m * 0.0012m
            + (totalOrderValue - 500_000_000m) * 0.001m;
    }

    return Math.Round(fee, 2, MidpointRounding.AwayFromZero);
}