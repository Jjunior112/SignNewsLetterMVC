using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class Subscribe
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Campo obrigatório")]
    [DisplayName("Nome")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Campo obrigatório")]
    [DisplayName("Email")]
    public string Email { get; set; } = "";

    public DateTime SubscribesSince { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
}