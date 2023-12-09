using System.Reflection;

namespace Enterprise.Reflection.Assemblies;

public static class AssemblyService
{
    /// <summary>
    /// Get all assemblies under the current application domain.
    /// NOTE: Assemblies are lazily loaded into the app domain.
    /// This means it is possible that not all assemblies will be returned depending on when this method is called.
    /// Assemblies will not be returned unless a call has been made to a method or class in the assembly.
    /// </summary>
    /// <returns></returns>
    public static Assembly[] GetFromCurrentAppDomain()
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        Assembly[] assemblies = currentDomain.GetAssemblies();
        return assemblies;
    }

    /// <summary>
    /// Add a handler to the event that fires when an assembly is loaded for the first time.
    /// Since assemblies are lazily loaded, this is a way to execute some behavior for a specific assembly at the time it is loaded.
    /// NOTE: Some assemblies may have already been loaded, depending on when this is called.
    /// </summary>
    /// <param name="eventHandler"></param>
    public static void HandleOnAssemblyLoad(AssemblyLoadEventHandler eventHandler)
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.AssemblyLoad += eventHandler;
    }

    /// <summary>
    /// Returns all assemblies referenced by the entry point assembly.
    /// Typically this is the main application (web project, API, windows service, etc.).
    /// NOTE: this only works with direct references. It doesn't pull in chained references.
    /// </summary>
    /// <returns></returns>
    public static AssemblyName[]? GetEntryAssemblyReferences()
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();
        AssemblyName[]? referencedAssemblies = entryAssembly?.GetReferencedAssemblies();
        return referencedAssemblies;
    }

    /// <summary>
    /// NOTE: This works at startup if you want to view ALL assemblies.
    /// Dynamically loaded assemblies are not returned.
    /// </summary>
    /// <returns></returns>
    public static List<Assembly> GetAllAssemblies()
    {
        bool FilterPredicate(AssemblyName assemblyName) => true;
        List<Assembly> allAssemblies = GetAllAssemblies(FilterPredicate);
        return allAssemblies;
    }

    /// <summary>
    /// NOTE: This works at startup if you want to view ALL assemblies.
    /// Dynamically loaded assemblies are not returned.
    /// The filter predicate is commonly used to apply a whitelist (ex: x => x.Name.StartsWith("MySolution"))
    /// or a blacklist (ex: x => !x.Name.StartsWith("System.").
    /// </summary>
    /// <param name="filterPredicate"></param>
    /// <returns></returns>
    public static List<Assembly> GetAllAssemblies(Func<AssemblyName, bool> filterPredicate)
    {
        Queue<Assembly> assembliesToCheck = new Queue<Assembly>();
        HashSet<string> loadedAssemblyNames = new HashSet<string>();
        List<Assembly> assemblies = new List<Assembly>();

        Assembly? entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly != null)
            assembliesToCheck.Enqueue(entryAssembly);

        while (assembliesToCheck.Any())
        {
            Assembly assemblyToCheck = assembliesToCheck.Dequeue();

            AssemblyName[] referencedAssemblyNames = assemblyToCheck.GetReferencedAssemblies();

            foreach (AssemblyName assemblyName in referencedAssemblyNames)
            {
                if (loadedAssemblyNames.Contains(assemblyName.FullName))
                    continue;

                bool doNotLoadAssembly = !filterPredicate?.Invoke(assemblyName) ?? false;

                if (doNotLoadAssembly)
                    continue;

                Assembly assembly = Assembly.Load(assemblyName);
                assembliesToCheck.Enqueue(assembly);
                loadedAssemblyNames.Add(assemblyName.FullName);
                assemblies.Add(assembly);
            }
        }

        return assemblies;
    }

    /// <summary>
    /// Load all assemblies in the base directory of the current app domain.
    /// This will return dynamically loaded assemblies as long as they are under the aforementioned directory.
    /// </summary>
    /// <returns></returns>
    public static Assembly[] GetSolutionAssemblies()
    {
        bool FilterPredicate(AssemblyName assemblyName) => true;
        Assembly[] assemblies = GetSolutionAssemblies(FilterPredicate);
        return assemblies;
    }

    /// <summary>
    /// Load all assemblies in the base directory of the current app domain.
    /// This will return dynamically loaded assemblies as long as they are under the aforementioned directory.
    /// The filter predicate is commonly used to apply a whitelist (ex: x => x.Name.StartsWith("MySolution"))
    /// or a blacklist (ex: x => !x.Name.StartsWith("System.").
    /// </summary>
    /// <param name="filterPredicate"></param>
    /// <returns></returns>
    public static Assembly[] GetSolutionAssemblies(Func<AssemblyName, bool> filterPredicate)
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        string baseDirectory = currentDomain.BaseDirectory;
        string[] dllFiles = Directory.GetFiles(baseDirectory, "*.dll");
        List<AssemblyName> assemblyNames = dllFiles.Select(AssemblyName.GetAssemblyName).ToList();
        List<AssemblyName> assemblyNamesToLoad = assemblyNames.Where(filterPredicate).ToList();
        List<Assembly> assemblies = assemblyNamesToLoad.Select(Assembly.Load).ToList();
        return assemblies.ToArray();
    }
}