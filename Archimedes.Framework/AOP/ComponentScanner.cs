using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Archimedes.DI.AOP;
using log4net;

namespace Archimedes.Framework.AOP
{
    /// <summary>
    /// Scans for components (types which are annotated by component attributes)
    /// </summary>
    public class ComponentScanner
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ComponentScanner()
        {
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyFilters"></param>
        /// <returns></returns>
        public IEnumerable<Type> ScanComponents(string[] assemblyFilters)
        {
            Log.Info("Assembly Component-Scanning restricted to: " + string.Join(", ", assemblyFilters));

            EnsureAssembliesAreLoadedForComponentScan();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var ignoredAssemblies = new List<string>();

            foreach (var assembly in assemblies)
            {
                if (ScanAssembly(assembly, assemblyFilters))
                {
                    Log.Debug("==  Scanning assembly " + assembly.GetName().Name + "  ==");
                    var components = FindComponentTypes(assembly);

                    foreach (var component in components)
                    {
                        Log.Debug("      * " + component.Name);
                        yield return component;
                    }
                    Log.Debug(" == == ");
                }
                else
                {
                    ignoredAssemblies.Add(assembly.GetName().Name);
                }
            }
            // Log ingnored
            Log.Debug(string.Format("Ignored assemblies: {0}", Environment.NewLine + string.Join(Environment.NewLine, ignoredAssemblies)));
        }

        private bool ScanAssembly(Assembly assembly, string[] assemblyFilters)
        {
            if (assemblyFilters.Length == 0) return true;

            foreach (var filter in assemblyFilters)
            {
                if (Regex.IsMatch(assembly.GetName().Name, filter, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }


        private IEnumerable<Type> FindComponentTypes(Assembly assembly)
        {
            var service = typeof(ServiceAttribute);             // TODO Refactor
            var component = typeof(ComponentAttribute);
            var controller = typeof(ControllerAttribute);

            return from t in assembly.GetTypes()
                   where t.IsDefined(service, false) || t.IsDefined(component, false) || t.IsDefined(controller, false)
                   select t;
        }

        private void EnsureAssembliesAreLoadedForComponentScan()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            loadedAssemblies
                .SelectMany(x => x.GetReferencedAssemblies())
                .Distinct()
                .Where(y => loadedAssemblies.Any((a) => a.FullName == y.FullName) == false)
                .ToList()
                .ForEach(x => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(x)));
        }
    }
}
