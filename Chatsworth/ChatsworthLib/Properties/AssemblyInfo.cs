using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ChatsworthLib")]
[assembly: AssemblyDescription("Google Talk Chatbot Library")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Kevin McMahon")]
[assembly: AssemblyProduct("ChatsworthLib")]
[assembly: AssemblyCopyright("Copyright © Kevin McMahon 2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("42dbfe35-4f32-4419-9521-a76e8b735739")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.0.0.2")]
[assembly: AssemblyFileVersion("0.0.0.2")]
[assembly: log4net.Config.XmlConfigurator
  (ConfigFile = "log4net.config", Watch = true)]