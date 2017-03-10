using NitroBolt.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.CommandLine
{
    public class CommandLineManager
    {
        public static int Process(string[] args, System.Reflection.Assembly assembly)
        {
            var operations = CommandLineOperationAttribute.GetAllOperations(assembly).ToArray();

            return Process(args, operations);

        }

        public static int Process(string[] args, CommandLineOperation[] operations)
        {
            try
            {
                var multioperations = operations.GroupBy(_operation => _operation.Name)
                    .Where(group => group.Count() > 1);
                if (multioperations.Any())
                {
                    Console.Error.WriteLine("Warning: multi operations");
                    foreach (var multiOp in multioperations)
                    {
                        Console.Error.WriteLine("  {0}: {1}", multiOp.Key, multiOp.Select(op => op.Method != null ? op.Method.Name : null).JoinToString(", "));
                    }
                }


                var operation = operations.Where(op => "--" + op.Name == args.FirstOrDefault()).FirstOrDefault();
                if (operation != null)
                {
                    Console.WriteLine("execute '{0}'", operation.Name);
                    operation.Operation();
                    return 0;
                }
                else
                {
                    Console.Error.WriteLine("Unknown command: {0}", args.FirstOrDefault());
                    return 1;
                }
            }
            catch(Exception exc)
            {
                Console.Error.WriteLine(exc);
                return 1;
            }
        }
    }
}
