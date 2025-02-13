using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Site
{
    public class Recaptcha
    {
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public string Token { get; set; }
    }
}
