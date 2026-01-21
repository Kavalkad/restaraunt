using System.Security;

namespace restaraunt.Core.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<PermissionEntity> Permissions { get; set; } = new List<PermissionEntity>();
        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}