using System;
using System.Collections.Generic;

namespace TrieuTruongGiang.Models;

public partial class LoaiSanPham
{
    public string MaLoai { get; set; } = null!;

    public string? TenLoai { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; } = new List<SanPham>();
}
