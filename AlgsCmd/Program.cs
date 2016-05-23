using Algs4Net;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AlgsCmd
{
  class Program
  {
    private static readonly Assembly asm = Assembly.GetAssembly(typeof(Algs4Net.TextInput));
    private static readonly string mainTest = "MainTest";
    private static readonly string nameSpace = "Algs4Net";

    private static SortedSet<string> GetAllTestClasses()
    {
      Type[] types = asm.GetTypes().Where(t => (t.IsPublic && t.GetMethod(mainTest) != null)).ToArray();
      Regex genericType = new Regex(@"\w+`\d");

      SortedSet<string> allNames = new SortedSet<string>();
      foreach (var t0 in types)
      {
        string name;
        if (genericType.IsMatch(t0.Name))
        {
          Match m = genericType.Match(t0.Name);
          name = m.Value;
          name = name.Substring(0, name.Length - 2);
        }
        else
          name = t0.Name;
        allNames.Add(name);
      }
      return allNames;
    }

    private static MethodInfo GetMainTest(string className)
    {
      string[] suffixes = { "", "`1", "`2" }; // currently support up to 2

      string fullName;
      if (!className.Contains(nameSpace))
        fullName = nameSpace + "." + className;
      else
        fullName = className;

      Type t1;
      MethodInfo testMethod = null;
      for (int i=0; i<suffixes.Length; i++)
      {
        t1 = asm.GetType(fullName + suffixes[i]);
        if (t1 != null)
        {
          // i > 0 means generic type, make t1 concrete
          if (i == 1)
            t1 = t1.MakeGenericType(typeof(string));
          else if (i == 2)
          {
            t1 = t1.MakeGenericType(typeof(int), typeof(string));
          }

          testMethod = t1.GetMethod(mainTest);
          break;
        }
      }
      return testMethod;
    }

    // helper methods for console demo
    private static void DisplayHelp()
    {
      string cmd = "algscmd";
      string[] helpText =
      {
        "\nUsage:\n",
        " " + cmd + " <Enter>: display this help text",
        " " + cmd + " l | list: list all the classes that has a demo test",
        " " + cmd + " h | help: display help for running a class's demo test. For instance,\n",
        "   C:\\> " + cmd + " h Bag\n"
      };

      foreach (var s in helpText)
      {
        Console.WriteLine(s);
      }
    }

    private static void DisplayHelpFor(string className)
    {
      try
      {
        MethodInfo testMethod = GetMainTest(className);

        if (testMethod != null)
        {
          Attribute attribute = testMethod.GetCustomAttribute(typeof(HelpTextAttribute), false);
          if (attribute != null)
          {
            HelpTextAttribute help = (HelpTextAttribute)attribute;
            Console.WriteLine("\nUsage:\n");
            Console.WriteLine("   " + help.Usage + "\n   " + help.Details);
          }
          else
            Console.WriteLine("No help text for: {0}", className);
        }
        else
          Console.WriteLine("No demo test for: {0}", className);
      }
      catch (Exception ex)
      {
        string inner = ex.InnerException.Message;
        Console.WriteLine("Error: {0}", inner);
      }
    }

    private static void ListSupportClasses(string pattern)
    {
      // TODO: support wild card and pattern matching for class names
      try
      {
        SortedSet<string> classNames = GetAllTestClasses();

        Console.WriteLine();
        foreach (var name in classNames)
        {
          Console.WriteLine("  " + name);
        }
      }
      catch (Exception ex)
      {
        string inner = ex.Message;
        Console.WriteLine("Error: {0}", inner);
      }
    }

    /// <summary>
    /// Interprets command line arguments to determine which class to test
    /// and inject the proper parameters to the test.
    /// </summary>
    /// <param name="args">Command line parameters to the program</param>
    /// 
    [STAThread]
    static void Main(string[] args)
    {
      // help options
      if (args.Length == 0)
      {
        DisplayHelp();
        return;
      }
      if (args[0].Equals("h", StringComparison.InvariantCultureIgnoreCase) ||
          args[0].Equals("help", StringComparison.InvariantCultureIgnoreCase))
      {
        if (args.Length < 2)
        {
          DisplayHelp();
          return;
        }

        DisplayHelpFor(args[1]);
        return;
      }
      else if (args[0].Equals("l", StringComparison.InvariantCultureIgnoreCase) ||
          args[0].Equals("list", StringComparison.InvariantCultureIgnoreCase))
      {
        if (args.Length >= 2)
          ListSupportClasses(args[1]);
        else
          ListSupportClasses("");

        return;
      }

      // test method invocation
      string className = args[0];
      try
      {
        MethodInfo testMethod = GetMainTest(className);
        if (testMethod != null)
        {
          string[] testArgs = new string[args.Length - 1];
          for (int i = 1; i < args.Length; i++)
            testArgs[i - 1] = args[i];
          testMethod.Invoke(null, new object[] { testArgs });
        }
        else
          Console.WriteLine("No demo test for: {0}", className);
      }
      catch (Exception ex)
      {
        string inner = ex.InnerException.Message;
        Console.WriteLine("Test run error: {0}", inner);
        DisplayHelpFor(className);
      }
    }
  }
}
