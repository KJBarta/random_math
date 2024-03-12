{Work in progress...}

"Summation Equations" directory is implemented to produce the equations for & solve for the function E(0 to N)[i^x] for any "x" in terms of N.  For example:
 >>  E(0 to N)[i^0] =
 >>  E(0 to N)[i^1] =
 >>  E(0 to N)[i^2] =
 >>  E(0 to N)[i^3] =

dotnet /usr/share/dotnet/sdk/8.0.200/Roslyn/bincore/csc.dll  -r:/usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.2/System.Private.CoreLib.dll  -r:/usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.2/System.Console.dll -r:/usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.2/System.Runtime.Numerics.dll  -r:/usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.2/System.Runtime.dll src/summations.cs tests_csc/test_simple.cs

dotnet  test_simple.exe
