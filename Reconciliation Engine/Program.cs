Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var systemLogs = new List<Transaction>
{
    new("TX01", 100_000m), 
    new("TX02", 200_000m), 
    new("TX03", 300_000m)  
};

var bankStatements = new List<Transaction>
{
    new("TX01", 100_000m), 
    new("TX02", 150_000m),
    new("TX04", 400_000m) 
};

var result = Reconcile(systemLogs, bankStatements);

Console.WriteLine("--- MissingInBank (App có, Bank thiếu) ---");
result.MissingInBank.ForEach(Console.WriteLine);

Console.WriteLine("\n--- MissingInSystem (Bank có, App thiếu) ---");
result.MissingInSystem.ForEach(Console.WriteLine);

Console.WriteLine("\n--- AmountMismatches (Lệch tiền) ---");
result.AmountMismatches.ForEach(Console.WriteLine);

ReconciliationResult Reconcile(
    List<Transaction> systemLogs,
    List<Transaction> bankStatements)
{
    var missingInBank = new List<Transaction>();
    var amountMismatches = new List<Transaction>();

    // 1. Đưa sao kê ngân hàng vào Dictionary (Key = TxId) để tra cứu trong O(1), tránh vòng lặp lồng nhau
    var bankDict = new Dictionary<string, Transaction>();
    foreach (var tx in bankStatements)
    {
        bankDict[tx.TxId] = tx;
    }

    // 2. Duyệt log hệ thống để đối chiếu với sao kê ngân hàng
    foreach (var log in systemLogs)
    {
        if (bankDict.TryGetValue(log.TxId, out var bankTx))
        {
            // Có ở cả 2 bên nhưng lệch số tiền
            if (bankTx.Amount != log.Amount)
            {
                amountMismatches.Add(log);
            }

            // Xóa khỏi Dictionary để những phần tử còn sót lại ở bước 3 chính là MissingInSystem
            bankDict.Remove(log.TxId);
        }
        else
        {
            // Có ở hệ thống nhưng ngân hàng không có
            missingInBank.Add(log);
        }
    }

    // 3. Toàn bộ giao dịch còn dư trong Dictionary là phần hệ thống bị thiếu
    var missingInSystem = bankDict.Values.ToList();

    return new ReconciliationResult(missingInBank, missingInSystem, amountMismatches);
}

// Model đại diện cho 1 giao dịch (Mã giao dịch & Số tiền)
public record Transaction(string TxId, decimal Amount);

// Kết quả đối soát gom 3 danh sách lỗi trả về
public record ReconciliationResult(
    List<Transaction> MissingInBank,    // Hệ thống có, ngân hàng thiếu
    List<Transaction> MissingInSystem,  // Ngân hàng có, hệ thống thiếu
    List<Transaction> AmountMismatches  // Trùng mã nhưng lệch số tiền
);