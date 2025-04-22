using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    public string? Image { get; set; }
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;
    

    [ProtectedPersonalData]
    public string? JobTitle { get; set; }

    public virtual UserAdressEntity? Adress { get; set; }
    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}

