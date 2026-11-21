
KetQuaKiemTra kiemTra (List<Transaction> kho, List<Transaction> heThong)
{
    var heThongCoKhoThieu = new List<Transaction>();
    var lechSoLuong = new List<Transaction>();

    var kiemTraKho = new Dictionary<string, Transaction>();

    foreach (var x in kho)
    {
        kiemTraKho[x.maSp] = x;
    }

    foreach (var x in heThong)
    {
        if (kiemTraKho.TryGetValue(x.maSp, out var khoTx))
        {
            if (khoTx.soLuong != x.soLuong)
            {
                lechSoLuong.Add(x);
            }
            kiemTraKho.Remove(x.maSp);
        }
        else
        {
            heThongCoKhoThieu.Add(x);
        }
    }
    var khoCoHeThongThieu = kiemTraKho.Values.ToList();

    return new KetQuaKiemTra(khoCoHeThongThieu, heThongCoKhoThieu, lechSoLuong);

}

public record Transaction(string maSp, int soLuong);

public record KetQuaKiemTra(
    List<Transaction> khoCoHeThongThieu,   
    List<Transaction> heThongCoKhoThieu, 
    List<Transaction> lechSoLuong  
);