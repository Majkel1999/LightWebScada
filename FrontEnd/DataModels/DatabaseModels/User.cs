namespace FrontEnd.DataModels.DatabaseModels
{
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Email { get; set; }
    }
}