using System.Diagnostics;
using System.Reflection;

namespace SuperDoc.Shared.Helpers;

public static class AssemblyHelper
{
    public static Type? GetType(string name)
    {
        return GetTypes()?.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public static IEnumerable<Type>? GetTypes()
    {
        // Attempts to get entry assembly from the default application domain, however doesn't work with unmanaged code, so as a backup get it from the stack trace.
        Assembly? entryAssembly = Assembly.GetEntryAssembly() ?? GetEntryAssemblyFromStackTrace();

        IEnumerable<Assembly>? referencedAssemblies = entryAssembly?.GetReferencedAssembliesWithRelatedName().Select(x => Assembly.Load(x));
        IEnumerable<Type>? type = referencedAssemblies?.SelectMany(x => x.GetTypes());

        return type;
    }

    public static Assembly? GetEntryAssemblyFromStackTrace()
    {
        // https://www.codeproject.com/Tips/791878/Get-Calling-Assembly-from-StackTrace
        Assembly executingAssembly = Assembly.GetExecutingAssembly();

        StackTrace stackTrace = new StackTrace();
        foreach (StackFrame stackFrame in stackTrace.GetFrames())
        {
            Assembly? entryAssembly = stackFrame.GetMethod()?.DeclaringType?.Assembly;
            if (entryAssembly != executingAssembly)
            {
                return entryAssembly;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the <see cref="AssemblyName"/> objects for the current assembly and all assemblies that reference it using a similar root name.
    /// </summary>
    public static IEnumerable<AssemblyName> GetReferencedAssembliesWithRelatedName(this Assembly assembly)
    {
        List<AssemblyName> assemblies = new List<AssemblyName>() { assembly.GetName() };

        string rootAssemblyName = assembly.FullName?.Split('.').FirstOrDefault() ?? string.Empty;

        IEnumerable<AssemblyName> referencedAssemblies = assembly.GetReferencedAssemblies().Where(x => x.Name?.Contains(rootAssemblyName) ?? false);
        assemblies.AddRange(referencedAssemblies);

        return assemblies;
    }
}
