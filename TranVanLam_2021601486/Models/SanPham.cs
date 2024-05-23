﻿using System;
using System.Collections.Generic;

namespace TranVanLam_2021601486.Models;

public partial class SanPham
{
    public string MaSp { get; set; } = null!;

    public string? TenSp { get; set; }

    public double? DonGia { get; set; }

    public int? SoLuong { get; set; }

    public string? MaLoai { get; set; }

    public virtual LoaiSanPham? MaLoaiNavigation { get; set; }
}