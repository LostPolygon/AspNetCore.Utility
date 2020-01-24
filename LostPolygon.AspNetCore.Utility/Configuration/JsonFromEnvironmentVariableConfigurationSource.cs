using Microsoft.Extensions.Configuration;

namespace Ballast.Atlantis.Utility {
    public class JsonFromEnvironmentVariableConfigurationSource : IConfigurationSource {
        public string EnvVariable { get; }
        public bool ClearEnvVariable { get; }

        public JsonFromEnvironmentVariableConfigurationSource(string envVariable, bool clearEnvVariable) {
            EnvVariable = envVariable;
            ClearEnvVariable = clearEnvVariable;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) {
            return new JsonFromEnvironmentVariableConfigurationProvider(this);
        }
    }
}
