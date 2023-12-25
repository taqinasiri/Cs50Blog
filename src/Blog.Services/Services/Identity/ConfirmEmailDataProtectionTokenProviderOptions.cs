﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog.Services.Services.Identity;
public class ConfirmEmailDataProtectionTokenProviderOptions : DataProtectionTokenProviderOptions { }
public class ConfirmEmailDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
{
    public ConfirmEmailDataProtectorTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<ConfirmEmailDataProtectionTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
    {
    }
}
