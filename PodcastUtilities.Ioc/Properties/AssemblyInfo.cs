﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PodcastUtilities.Ioc")]
[assembly: AssemblyDescription("Ioc containers for PodcastUtilities")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("19cbc8e3-ebb3-4614-b0e6-af46733763e9")]

[assembly: CLSCompliant(true)]

// we cannot have a strong name as we depend on LinFu Core and it does not have a string name
[module: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]

// Version information is in the shared AssemblyInfo
