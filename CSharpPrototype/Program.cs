using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Dependency
    {
        public void PathHint(string aHint) { mPathHint = aHint; }

        protected List<string> mPathsToLibraries;
        protected List<string> mPathsToIncludes;
        protected string mPathHint;
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

        protected List<int> mLinkerFlags;
        protected List<int> mCompilerFlags;
        protected string mPathHint;
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
                compilerFlags.Append(" /nologo ");

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

            string LinkerFlags()
            {
                StringBuilder linkerFlags = new StringBuilder();
                linkerFlags.Append(" /NOLOGO ");

                foreach (LinkerFlag flag in mLinkerFlags)
                {
                    switch (flag)
                    {
                        case LinkerFlag.LinkTimeCodeGeneration: linkerFlags.Append("/W1"); break;
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
                    compileCommandBuilder.Append(file);
                    compileCommandBuilder.Append(' ');
                }

                compileCommandBuilder.Append("/OUT:");
                compileCommandBuilder.Append(aOutName);

                var compiling = Process.Start("CL", compileCommandBuilder.ToString());
                compiling.WaitForExit();

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
                    linkCommandBuilder.Append(file);
                    linkCommandBuilder.Append(' ');
                }

                linkCommandBuilder.Append("/OUT:");
                linkCommandBuilder.Append(aOutName);

                var linking = Process.Start("LINK", linkCommandBuilder.ToString());
                linking.WaitForExit();

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
            aExtensions = aExtensions ?? new string[0];

            foreach (string extension in aExtensions)
            {
                string[] files = System.IO.Directory.GetFiles(aFolder, extension);

                foreach (string file in files)
                {
                    mFiles.Add(file);
                }
            }
        }


        protected List<string> mPathHints;
        List<string> mFiles;
        List<Dependency> mDependencies;
        List<Compiler> mCompilers;
    }




    
    class Program
    {
        public void UserScript()
        {
            Project helloWorld = new Project();

            // We can get all of boost
            helloWorld.AddDependency(new Dependencies.Boost());

            // Or just some of it.
            helloWorld.AddDependency(new Dependencies.Boost.Asio());

            // Make and add msvc while turning on some flags.
            var msvc = new Compilers.msvc();
            msvc.AddCompilerFlag(Compilers.msvc.CompilerFlag.AllWarnings);
            msvc.AddLinkerFlag(Compilers.msvc.LinkerFlag.LinkTimeCodeGeneration);

            helloWorld.AddCompiler(msvc);
        }

        static void Main(string[] args) 
        {
        }
    }
}
