using JoshMake;

namespace BuildSystem
{
    class Program
    {
        static public void Configuration()
        {
            Project zilchTest = new Project("ZilchAndFlagTest");

            // Or just some of it.
            zilchTest.AddDependency(new Zilch("Dependencies"));

            // Make and add msvc while turning on some flags.
            var msvc = new msvc();
            //msvc.AddCompilerFlag(msvc.CompilerFlag.AllWarnings);
            msvc.AddCompilerFlag(msvc.CompilerFlag.MaxOptimization);
            msvc.AddLinkerFlag(msvc.LinkerFlag.LinkTimeCodeGeneration);

            zilchTest.AddCompiler(msvc);

            zilchTest.AddFile("main.cpp");

            zilchTest.Compile();
        }
    }
}