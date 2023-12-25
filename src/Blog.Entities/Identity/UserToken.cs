﻿using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Identity;

public class UserToken : IdentityUserToken<int>
{
    public virtual User User { get; set; }
}

