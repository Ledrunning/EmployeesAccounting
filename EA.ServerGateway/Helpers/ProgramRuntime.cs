using System.Globalization;
using System.Reflection;

namespace EA.ServerGateway.Helpers;

internal static class ProgramRuntime
{
    public static string ProgramName => typeof(ProgramRuntime).Assembly
        .GetCustomAttributes(typeof(AssemblyTitleAttribute), false)
        .OfType<AssemblyTitleAttribute>()
        .First().Title;

    public static Version? ProgramVersion => typeof(ProgramRuntime).Assembly
        .GetName()
        .Version;

    public static string ProgramRevision => typeof(ProgramRuntime).Assembly
        .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)
        .OfType<AssemblyInformationalVersionAttribute>().First().InformationalVersion;

    public static Version ProgramFileVersion => new(typeof(ProgramRuntime).Assembly
        .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
        .OfType<AssemblyFileVersionAttribute>().First().Version);

    public static string ProgramPublicationDate => File.GetCreationTime(Assembly.GetExecutingAssembly().Location)
        .ToString(new CultureInfo("en-EN"));
}