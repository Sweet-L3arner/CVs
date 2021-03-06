using QuanLyBanQuanAo.Models;
using System.Data.SqlClient;
using System.Linq;

namespace Models.Dao
{
    public class AccountModel
    {
        private QuanLyBanQuanAoEntities _context = null;

        public AccountModel()
        {
            _context = new QuanLyBanQuanAoEntities();
        }

        // có tồn tại tài khoản
        public bool Login(string email, string matKhau, bool quanLy)
        {
            object[] sqlParams =
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", matKhau),
                new SqlParameter("@QuanLy", quanLy)
            };

            var result =
                _context.Database.SqlQuery<bool>("Sp_QuanLy_Login @Email, @Password, @QuanLy", sqlParams).SingleOrDefault();

            return result;
        }
    }
}
