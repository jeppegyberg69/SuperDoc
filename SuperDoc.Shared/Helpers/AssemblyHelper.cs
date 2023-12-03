using System.Diagnostics;
using System.Reflection;

namespace SuperDoc.Shared.Helpers;

/// <summary>
/// Helper class for working with assemblies and retrieving types from related referenced assemblies.
/// </summary>
public static class AssemblyHelper
{
    /// <summary>
    /// Attempt to retrieve a <see cref="Type"/> by its name from the assemblies referenced by the entry assembly.
    /// </summary>
    /// <param name="name">The name of the type to retrieve.</param>
    /// <returns>The <see cref="Type"/> if found, or <see langword="null"/> if not found.</returns>
    public static Type? GetType(string name)
    {
        // Get the collection of types using the GetTypes method.
        IEnumerable<Type>? types = GetTypes();

        // Attempt to find the type with the specified name.
        Type? type = types?.FirstOrDefault(type => type.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return type;
    }

    /// <summary>
    /// Attempt to retrieve a collection of types from the assemblies referenced by the entry assembly.
    /// </summary>
    /// <returns>An enumerable collection of types or <see langword="null"/> if not found.</returns>
    public static IEnumerable<Type>? GetTypes()
    {
        // Attempts to get the entry assembly from the default application domain, however that doesn't work with unmanaged code, so as a backup we get it from the stack trace.
        Assembly? entryAssembly = Assembly.GetEntryAssembly() ?? GetEntryAssemblyFromStackTrace();

        // Get the referenced assemblies of the entry assembly and load them, so that we can access the types.
        IEnumerable<Assembly>? referencedAssemblies = entryAssembly?.GetReferencedAssembliesWithRelatedName().Select(Assembly.Load);

        // Retrieve the types from the loaded assemblies.
        IEnumerable<Type>? type = referencedAssemblies?.SelectMany(assembly => assembly.GetTypes());

        return type;
    }

    /// <summary>
    /// Attempt to retrieve the entry assembly by inspecting the call stack.
    /// </summary>
    /// <returns>The entry assembly or null if not found.</returns>
    public static Assembly? GetEntryAssemblyFromStackTrace()
    {
        // This method was shamelessly stolen from: https://www.codeproject.com/Tips/791878/Get-Calling-Assembly-from-StackTrace

        // Get the assembly of the currently executing code.
        Assembly executingAssembly = Assembly.GetExecutingAssembly();

        // Create a stack trace to examine the call stack and iterate through the stack frames, so we can find the entry assembly.
        StackTrace stackTrace = new StackTrace();
        foreach (StackFrame stackFrame in stackTrace.GetFrames())
        {
            // Get the assembly associated with the method on the stack frame.
            Assembly? entryAssembly = stackFrame.GetMethod()?.DeclaringType?.Assembly;

            // If the entry assembly is different from the executing assembly, consider it the entry assembly.
            if (entryAssembly != executingAssembly)
            {
                return entryAssembly;
            }
        }

        return null;
    }

    /// <summary>
    /// Get a collection of assembly names that includes the given assembly and its referenced assemblies with a related root name.
    /// </summary>
    /// <param name="assembly">The assembly for which to retrieve referenced assemblies.</param>
    /// <returns>A collection of <see cref="AssemblyName"/> which repesentens the given assembly and its related referenced assemblies.</returns>
    public static IEnumerable<AssemblyName> GetReferencedAssembliesWithRelatedName(this Assembly assembly)
    {
        // Initialize the return list with the AssemblyName of the current assembly.
        List<AssemblyName> assemblies = new List<AssemblyName>() { assembly.GetName() };

        // Extract the root assembly name from the full name of the assembly.
        string rootAssemblyName = assembly.FullName?.Split('.').FirstOrDefault() ?? string.Empty;

        // Retrieve referenced assemblies that starts with the root assembly name and add the referenced assemblies to the assembly list.
        IEnumerable<AssemblyName> referencedAssemblies = assembly.GetReferencedAssemblies().Where(assembly => assembly.Name?.StartsWith(rootAssemblyName) ?? false);
        assemblies.AddRange(referencedAssemblies);

        return assemblies;
    }
}
