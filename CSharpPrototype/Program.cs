using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace ConsoleApplication1
{
    class Dependency
    {
        public void PathHint(string aHint) { mPathHint = aHint; }

        protected List<string> mPathsToLibraries = new List<string>();
        protected List<string> mPathsToIncludes = new List<string>();
        protected string mPathHint = "";
    }
    
    namespace Dependencies
    {
        class Boost : Dependency
        { 
            public class Asio : Dependency
            {
            }

        }

        class Dropbox : Dependency
        {
            public class Json11 : Dependency
            {
            }
        }

        class Facebook : Dependency
        {
            public class Folly : Dependency
            {
            }
        }
    }

    public abstract class Compiler
    {
        public void AddLinkerFlag(int aFlag)
        {
            mLinkerFlags.Add(aFlag);
        }

        public void AddCompilerFlag(int aFlag)
        {
            mCompilerFlags.Add(aFlag);
        }

        public abstract string Compile(string aOutName, string[]aFiles);

        public abstract string Link(string aOutName, string[]aFiles);

        protected List<int> mLinkerFlags = new List<int>();
        protected List<int> mCompilerFlags = new List<int>();
        protected List<string> mLinkerIncludeDirectories = new List<string>()
        {
            "\"C:/Program Files (x86)/Microsoft Visual Studio 14.0/VC/lib\"",
            "\"C:/Program Files (x86)/Microsoft Visual Studio 14.0/VC/atlmfc/lib\"",
            "\"C:/Program Files (x86)/Windows Kits/10/lib/10.0.10240.0/ucrt/x86\"",
            "\"C:/Program Files (x86)/Windows Kits/8.1/lib/winv6.3/um\"",
            "\"C:/Program Files (x86)/Windows Kits/NETFXSDK/4.6.1/Lib/um\"",
            "\"C:/Program Files (x86)/Microsoft SDKs/Windows/v7.0A/Lib\""
        };
        protected List<string> mCompilerIncludeDirectories = new List<string>()
        {
            "\"C:/Program Files (x86)/Microsoft Visual Studio 14.0/VC/include\"",
            "\"C:/Program Files (x86)/Microsoft Visual Studio 14.0/VC/atlmfc/include\"",
            "\"C:/Program Files (x86)/Windows Kits/10/Include/10.0.10240.0/ucrt\"",
            "\"C:/Program Files (x86)/Windows Kits/8.1/Include/um\"",
            "\"C:/Program Files (x86)/Windows Kits/8.1/Include/shared\"",
            "\"C:/Program Files (x86)/Windows Kits/8.1/Include/winrt\"",
        };
        protected string mPathHint = "";
    }

    namespace Compilers
    {
        //class gcc : Compiler
        //{
        //}
        //
        //class clang : Compiler
        //{
        //}

        class msvc : Compiler
        {
            public void AddLinkerFlag(LinkerFlag aFlag)
            {
                mLinkerFlags.Add((int)aFlag);
            }

            public void AddCompilerFlag(CompilerFlag aFlag)
            {
                mCompilerFlags.Add((int)aFlag);
            }

            public enum LinkerFlag
            {
                LinkTimeCodeGeneration
            }
            public enum CompilerFlag
            {
                Warnings1,
                Warnings2,
                Warnings3,
                Warnings4,
                AllWarnings,
                WarningsAsErrors
            }

            string CompilerFlags()
            {
                StringBuilder compilerFlags = new StringBuilder();
                compilerFlags.Append(" /nologo /EHsc ");

                foreach(CompilerFlag flag in mCompilerFlags)
                {
                    switch (flag)
                    {
                        case CompilerFlag.Warnings1: compilerFlags.Append(" /W1 "); break;
                        case CompilerFlag.Warnings2: compilerFlags.Append(" /W2 "); break;
                        case CompilerFlag.Warnings3: compilerFlags.Append(" /W3 "); break;
                        case CompilerFlag.Warnings4: compilerFlags.Append(" /W4 "); break;
                        case CompilerFlag.AllWarnings: compilerFlags.Append(" /Wall "); break;
                        case CompilerFlag.WarningsAsErrors: compilerFlags.Append(" /Werror "); break;
                    }
                }

                return compilerFlags.ToString();
            }

            string CompilerIncludePaths()
            {
                StringBuilder builder = new StringBuilder();

                foreach (var path in mCompilerIncludeDirectories)
                {
                    builder.Append(" /I");
                    builder.Append(path);
                    builder.Append(" ");
                }

                return builder.ToString();
            }


            string LinkerIncludePaths()
            {
                StringBuilder builder = new StringBuilder();

                foreach (var path in mLinkerIncludeDirectories)
                {
                    builder.Append(" /LIBPATH:");
                    builder.Append(path);
                    builder.Append(" ");
                }

                return builder.ToString();
            }

            string LinkerFlags()
            {
                StringBuilder linkerFlags = new StringBuilder();
                linkerFlags.Append(" /NOLOGO ");

                foreach (LinkerFlag flag in mLinkerFlags)
                {
                    switch (flag)
                    {
                        case LinkerFlag.LinkTimeCodeGeneration: linkerFlags.Append(" /LTCG "); break;
                    }
                }

                return linkerFlags.ToString();
            }

            public override string Compile(string aOutName, string[] aFiles)
            {
                StringBuilder compileCommandBuilder = new StringBuilder();
                compileCommandBuilder.Append("/c ");
                compileCommandBuilder.Append(CompilerFlags());

                foreach (string file in aFiles)
                {
                    compileCommandBuilder.Append(file.Replace('\\', '/'));
                    compileCommandBuilder.Append(' ');
                }

                compileCommandBuilder.Append(CompilerIncludePaths());

                compileCommandBuilder.Append(" /Fo");
                compileCommandBuilder.Append(aOutName);

                ProcessStartInfo info = new ProcessStartInfo("\"C:/Program Files (x86)/Microsoft Visual Studio 14.0/VC/bin/cl.exe\"");
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.CreateNoWindow = false;

                info.UseShellExecute = false;
                info.Arguments = compileCommandBuilder.ToString();

                var compiling = Process.Start(info);

                string output = compiling.StandardOutput.ReadToEnd();
                string errors = compiling.StandardError.ReadToEnd();
                compiling.WaitForExit();

                Console.WriteLine("Output Reported by CL for Files {0}:", aFiles);
                Console.WriteLine(output);
                Console.WriteLine("Errors Reported by CL for Files {0}:", aFiles);
                Console.WriteLine(errors);

                if (compiling.ExitCode == 0)
                {
                    return aOutName;
                }
                else
                {
                    return "";
                }
            }

            public override string Link(string aOutName, string[] aFiles)
            {
                StringBuilder linkCommandBuilder = new StringBuilder();
                linkCommandBuilder.Append(LinkerFlags());

                foreach (string file in aFiles)
                {
                    linkCommandBuilder.Append(file.Replace('\\', '/'));
                    linkCommandBuilder.Append(' ');
                }


                linkCommandBuilder.Append(LinkerIncludePaths());

                linkCommandBuilder.Append(" /OUT:");
                linkCommandBuilder.Append(aOutName);


                ProcessStartInfo info = new ProcessStartInfo("\"C:/Program Files (x86)/Microsoft Visual Studio 14.0/VC/bin/link.exe\"");
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.CreateNoWindow = false;

                info.UseShellExecute = false;
                info.Arguments = linkCommandBuilder.ToString();

                var linking = Process.Start(info);

                string output = linking.StandardOutput.ReadToEnd();
                string errors = linking.StandardError.ReadToEnd();
                linking.WaitForExit();

                Console.WriteLine("Output Reported by LINK for Files {0}:", aFiles);
                Console.WriteLine(output);
                Console.WriteLine("Errors Reported by LINK for Files {0}:", aFiles);
                Console.WriteLine(errors);

                if (linking.ExitCode == 0)
                {
                    return aOutName;
                }
                else
                {
                    return "";
                }
            }
        }
    }

    class Project
    {
        public Project(string aProjectName) { mProjectName = aProjectName; }
        public void AddDependency(Dependency aDependency)
        {
            mDependencies.Add(aDependency);
        }

        public void AddCompiler(Compiler aCompiler)
        {
            mCompilers.Add(aCompiler);
        }

        public void AddDependencyPathHint(string aHint) { mPathHints.Add(aHint); }


        public void AddFolder(string aFolder, string[] aExtensions = null)
        {
            aExtensions = aExtensions ?? new string[] { "*.cpp", "*.c" };

            foreach (string extension in aExtensions)
            {
                string[] files = System.IO.Directory.GetFiles(aFolder, extension);

                foreach (string file in files)
                {
                    mFiles.Add(file);
                }
            }
        }

        public void Compile()
        {
            foreach (var compiler in mCompilers)
            {
                var mFilesToLink = new List<string>();

                foreach (var file in mFiles)
                {
                    string[] files = { file };
                    mFilesToLink.Add(compiler.Compile(Path.ChangeExtension(file, ".obj"), files));
                }

                compiler.Link(mProjectName + ".exe", mFilesToLink.ToArray());
            }
        }

        string mProjectName;
        protected List<string> mPathHints = new List<string>();
        List<string> mFiles = new List<string>();
        List<Dependency> mDependencies = new List<Dependency>();
        List<Compiler> mCompilers = new List<Compiler>();
    }




    
    class Program
    {
        static public void UserScript()
        {
            Project helloWorld = new Project("HelloWorld");

            // We can get all of boost
            //helloWorld.AddDependency(new Dependencies.Boost());

            // Or just some of it.
            //helloWorld.AddDependency(new Dependencies.Boost.Asio());

            // Make and add msvc while turning on some flags.
            var msvc = new Compilers.msvc();
            msvc.AddCompilerFlag(Compilers.msvc.CompilerFlag.Warnings1);
            msvc.AddLinkerFlag(Compilers.msvc.LinkerFlag.LinkTimeCodeGeneration);

            helloWorld.AddCompiler(msvc);

            helloWorld.AddFolder("TestFiles");

            helloWorld.Compile();
        }

        static void Main(string[] args) 
        {
            UserScript();
        }
    }
}
