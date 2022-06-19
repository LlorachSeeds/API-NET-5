using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Api.Tests.Mocks
{
    public class ConfigurationMock:IConfiguration
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }

        public string this[string key]
        {
            get => key == "JwtKey" ? "Escuchas@4983#SabrinaMicaela@2009" : "";
            set => throw new NotImplementedException();
        }
    }
}