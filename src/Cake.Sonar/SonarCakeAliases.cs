using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Sonar
{
    /// <summary>
    /// <para>
    /// Contains functionality for running a Sonar analysis on a c# project using the MSBuild SonarQube Runner.
    /// </para>
    /// <para>
    /// In order to use the commands for this addin, include the following in your build.cake file to download and
    /// reference from NuGet.org:
    /// <code>
    ///     #addin "nuget:?package=Cake.Sonar"
    ///     #tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"
    /// </code>
    /// </para>
    /// </summary>
    /// <example>
    /// Analysis is done in three phases. 
    /// In the first phase, init, msbuild targets are added for collecting information.
    /// In the second phase, you build your projects and optionally a create a test- and coverage-report.
    /// In the third phasesonar will analyse the collectived files and process the passed reports.
    /// 
    /// You can wrap the init and analysis phase around your existing tasks using dependencies with <see cref="SonarBegin"/> and <see cref="SonarEnd(ICakeContext)"/>:
    /// 
    /// <code>
    /// Task("Sonar")
    ///   .IsDependentOn("Sonar-Init") // should call SonarBegin()
    ///   .IsDependentOn("Build")
    ///   .IsDependentOn("Run-Unit-Test")
    ///   .IsDependentOn("Sonar-Analyse"); // should call SonarEnd()
    /// </code>
    /// 
    /// Or you can use a dedicated task that executed msbuild itself <see cref="Sonar"/>.
    /// 
    /// Tip: local testing of analysis can be done using sonarqube running in a docker container: 
    /// <code>
    /// docker run -d --name sonarqube -p 9000:9000 -p 9092:9092 sonarqube
    /// </code>
    /// </example>
    [CakeAliasCategory("Sonar")]
    public static class SonarCakeAliases
    {
        /// <summary>
        /// Initialise msbuild for sonar analysis.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("Initialise-Sonar")
        ///    .Does(() => {
        ///       SonarBegin(new SonarBeginSettings{
        ///          Name = "My.Project",
        ///          Key = "MP",
        ///          Url = "http://localhost:9000"     
        ///       });
        ///   });
        /// </code>
        /// </example>
        /// <param name="context"></param>
        /// <param name="settings">A required settings object.</param>
        [CakeMethodAlias]
        public static void SonarBegin(this ICakeContext context, SonarBeginSettings settings)
        {
            GetRunner(context).Run(settings);
        }

        /// <summary>
        /// Run the actual sonar analysis and push them to sonar. 
        /// This call should follow after a SonarBegin and MSBuild.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("Sonar-Analyse")
        ///   .Does(() => {
        ///       var user = EnvironmentVariable("SONAR_USER");
        ///       var pass = EnvironmentVariable("SONAR_PASS");
        ///       SonarEnd(new SonarEndSettings { Login = user, Password = pass });
        ///   });
        /// </code>
        /// </example>
        /// <param name="context"></param>
        /// <param name="settings">A settings object containing credentials.</param>
        [CakeMethodAlias]
        public static void SonarEnd(this ICakeContext context, SonarEndSettings settings)
        {
            GetRunner(context).Run(settings);
        }

        /// <summary>
        /// Run the actual sonar analysis (no credentials given).
        /// </summary>
        /// <example>
        /// <code>
        /// Task("Sonar-Analyse")
        ///   .Does(() => {
        ///       SonarEnd();
        ///   });
        /// </code>
        /// </example>
        /// <param name="context"></param>
        public static void SonarEnd(this ICakeContext context)
        {
            SonarEnd(context, new SonarEndSettings());
        }

        /// <summary>
        /// Run a self-contained analysis for the specified action. 
        /// The action should run msbuild.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("Sonar")
        ///   .Does(() => {
        ///      var settings = new SonarBeginSettings() {
        ///          ...
        ///      };
        ///      Sonar(ctx => ctx.MsBuild(solution), settings);
        ///   });
        /// </code>
        /// </example>
        /// <param name="context">The cake context</param>
        /// <param name="action"></param>
        /// <param name="settings"></param>
        [CakeMethodAlias]
        public static void Sonar(this ICakeContext context, Action<ICakeContext> action, SonarBeginSettings settings)
        {
            SonarBegin(context, settings);
            action(context);
            SonarEnd(context, settings.GetEndSettings());
        }

        private static SonarRunner GetRunner(ICakeContext context)
        {
            return new SonarRunner(context.Log, context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        }
    }
}