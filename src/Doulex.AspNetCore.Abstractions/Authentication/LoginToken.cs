﻿using Doulex.Collections;

namespace Doulex.AspNetCore.Authentication;

/// <summary>
/// 当前登录 Token
/// </summary>
public record LoginToken
{
    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public LoginToken(
        string   id,
        string   name,
        string   screenName,
        string   phoneNumber,
        string   email,
        string   avatarUrl,
        string[] roles)
    {
        Id          = id;
        PhoneNumber = phoneNumber;
        Email       = email;
        Name        = name;
        Roles       = new EquatableHashSet<string>(roles);
        ScreenName  = screenName;
        AvatarUrl   = avatarUrl;
    }

    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// 头像 Url
    /// </summary>
    public string AvatarUrl { get; }

    /// <summary>
    /// 微信昵称
    /// </summary>
    public string ScreenName { get; }

    /// <summary>
    /// 电话号码
    /// </summary>
    public string PhoneNumber { get; }

    /// <summary>
    /// 名字 
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// 角色
    /// </summary>
    public EquatableHashSet<string> Roles { get; }

    /// <summary>
    /// 如果从微信认证的用户, 提供微信OpenId
    /// </summary>
    public string? WechatOpenId { get; set; }
}
