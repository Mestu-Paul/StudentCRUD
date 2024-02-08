namespace A.Contracts.DataTransferObjects
{
    public class UserDTO
    {
        public UserDTO(string username, string role)
        {
            this.username = username;
            this.role = role;
        }
        public string username;
        public string role;
    }
}
