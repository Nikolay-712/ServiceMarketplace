﻿using Microsoft.AspNetCore.Identity;

namespace ServiceMarketplace.Data.Configurations;

public class IdentityOptionsProvider
{
    public static void GetIdentityOptions(IdentityOptions options)
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    }
}
