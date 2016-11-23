using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Sonar
{
    [CakeAliasCategory("Sonar")]
    public static class SonarCakeAliases
    {
        [CakeMethodAlias]
        public static void SonarBegin(this ICakeContext context, SonarBeginSettings settings)
        {
            new SonarCake(context).Begin(settings);
        }

        [CakeMethodAlias]
        public static void SonarEnd(this ICakeContext context, SonarEndSettings settings = null)
        {
            new SonarCake(context).End(settings ?? new SonarEndSettings());
        }

        [CakeMethodAlias]
        public static void Sonar(this ICakeContext context, Action<ICakeContext> action, SonarBeginSettings settings)
        {
            var cake = new SonarCake(context);
            cake.Begin(settings);
            action(context);
            cake.End(new SonarEndSettings()
            {
                Login = settings.Login,
                Password = settings.Password
            });
        }
    }
}