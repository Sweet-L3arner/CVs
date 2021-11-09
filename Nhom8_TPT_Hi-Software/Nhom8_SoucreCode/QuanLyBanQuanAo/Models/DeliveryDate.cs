using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyBanQuanAo.Models
{
    public class DeliveryDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var donDatHang = (DonDatHang)validationContext.ObjectInstance;

            if (donDatHang.NgayGiaoHang == null)
                return ValidationResult.Success;

            if(donDatHang.NgayGiaoHang >= donDatHang.NgayDatHang)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Ngày giao hàng phải lớn hơn hoặc bằng ngày ngày đặt hàng.");
        }
    }
}