﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserAdressEntity
{
    [Key, ForeignKey("User")]
    public string UserId { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}