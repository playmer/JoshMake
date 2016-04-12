using JoshMake;

namespace BuildSystem
{
    class Program
    {
        static public void Configuration()
        {
            Project helloWorld = new Project("HelloWorld");

            // Make and add msvc while turning on some flags.
            var msvc = new msvc();
            msvc.AddCompilerFlag(msvc.CompilerFlag.Warnings1);
            //msvc.AddLinkerFlag(msvc.LinkerFlag.LinkTimeCodeGeneration);

            helloWorld.AddCompiler(msvc);

            helloWorld.AddFolder("TestFiles");

            helloWorld.Compile();
        }
    }
}