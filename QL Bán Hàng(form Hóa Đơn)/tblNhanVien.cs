//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QL_Bán_Hàng_form_Hóa_Đơn_
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblNhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblNhanVien()
        {
            this.tblHDBans = new HashSet<tblHDBan>();
        }
    
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string GioiTinh { get; set; }
        public string NgayVaoLam { get; set; }
        public string ThamNien { get; set; }
        public string PhongBan { get; set; }
        public string MatKhau { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblHDBan> tblHDBans { get; set; }
    }
}
