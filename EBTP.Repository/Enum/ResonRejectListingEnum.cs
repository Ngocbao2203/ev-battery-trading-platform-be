using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Enum
{
    public enum ResonRejectListingEnum
    {
        CATEGORY_MISMATCH,      // Đăng sai danh mục (ô tô/xe máy/pin rời)
        INFORMATION_MISSING,    // Thiếu thông tin cụ thể (đời xe, số km, tình trạng...)

        PRICE_UNREALISTIC,      // Giá bán không hợp lý (quá thấp/quá cao bất thường)
        IMAGE_VIOLATION,        // Ảnh mờ/không đúng xe/thêm chữ số điện thoại, quảng cáo
        CONTACT_INVALID,        // Số điện thoại/địa chỉ liên hệ không hợp lệ
        DOCUMENT_INVALID,       // Thiếu hoặc sai thông tin giấy tờ xe (đăng ký, đăng kiểm...)
        VEHICLE_CONDITION_FALSE,// Mô tả tình trạng xe không đúng thực tế / gây hiểu lầm
        DUPLICATE_LISTING,      // Trùng lặp với một tin đã đăng trước đó
        POLICY_VIOLATION,       // Nội dung vi phạm quy định/điều khoản của sàn
        SUSPICIOUS_FRAUD        // Có dấu hiệu lừa đảo, yêu cầu thanh toán bất thường
    }

}
