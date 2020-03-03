using System.ComponentModel.DataAnnotations;

namespace IIT.Client.ViewModels
{
    public class AddStudentViewModel
    {
        [Required]
        public string Roll { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
