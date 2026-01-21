namespace restaraunt.Core.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();

    }

}

