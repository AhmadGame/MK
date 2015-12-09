// ReSharper disable InconsistentNaming
namespace Mk.ViewModels
{
    public class AccountViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
        public string error { get; set; }
    }
}