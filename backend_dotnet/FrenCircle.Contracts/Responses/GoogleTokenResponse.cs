﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Responses
{
    public record GoogleTokenResponse(
         string AccessToken,
         string RefreshToken,
         int ExpiresIn,
         string TokenType
     );
}
