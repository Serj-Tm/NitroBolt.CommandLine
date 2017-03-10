using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.CommandLine
{
    public class CommandLineOperationAttribute : Attribute
    {
        public CommandLineOperationAttribute(string name)
        {
            this.Name = name;
        }
        public readonly string Name;

        public static IEnumerable<CommandLineOperation> GetAllOperations(System.Reflection.Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
                {
                    var attr = method.GetCustomAttributes(typeof(CommandLineOperationAttribute), false).FirstOrDefault() as CommandLineOperationAttribute;
                    if (attr == null)
                        continue;
                    if (typeof(Task).IsAssignableFrom(method.ReturnType))
                    {
                        yield return new CommandLineOperation(attr.Name, () => Task.Run(() => (Task)method.Invoke(null, null)).Wait(), method);
                    }
                    else
                    {
                        yield return new CommandLineOperation(attr.Name, () => method.Invoke(null, null), method);
                    }
                }
            }
        }
    }
    public class CommandLineOperation
    {
        public CommandLineOperation(string name, Action operation, System.Reflection.MethodInfo method = null)
        {
            this.Name = name;
            this.Operation = operation;
            this.Method = method;
        }
        public readonly string Name;
        public readonly Action Operation;
        public readonly System.Reflection.MethodInfo Method;
    }
}
