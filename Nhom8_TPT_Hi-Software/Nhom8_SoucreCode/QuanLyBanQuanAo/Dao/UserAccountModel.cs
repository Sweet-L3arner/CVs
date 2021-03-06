using QuanLyBanQuanAo.Models;
using System.Data.SqlClient;
using System.Linq;

namespace Models.Dao
{
    public class UserAccountModel
    {
        private QuanLyBanQuanAoEntities _context = null;

        public UserAccountModel()
        {
            _context = new QuanLyBanQuanAoEntities();
        }

        // có tồn tại tài khoản
        public bool Login(string email, string matKhau)
        {
            object[] sqlParams =
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", matKhau)
            };

            var result =
                _context.Database.SqlQuery<bool>("Sp_KhachHang_Login @Email, @Password", sqlParams).SingleOrDefault();

            return result;
        }

        public bool Regis(string email)
        {
            object[] sqlParams =
            {
                new SqlParameter("@Email", email),
            };

            var result =
                _context.Database.SqlQuery<bool>("Sp_KhachHang_Regis @Email", sqlParams).SingleOrDefault();

            return result;
        }
    }
}
