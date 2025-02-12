using System;
using uni.learn.core.Entity;

namespace uni.learn.core.Interfaces;

public interface ITokenService
{

    string CreateToken(Usuario usuario, IList<string> roles);

}
