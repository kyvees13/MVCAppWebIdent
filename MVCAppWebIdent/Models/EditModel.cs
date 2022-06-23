namespace MVCAppWebIdent.Models
{
    public class EditModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public string Password { get; set; }

        public string Phonenumber { get; set; }
        public bool PhonenumberConfirmed { get; set; }
    }
}