namespace CafeShop.Models.DTOs
{
    public class AccountDto : Account
    {
        public string ConfirmPassWord{ get; set; }
        public string RoleText { get; set; }
        public string GenderText { get; set; }
    }
}
