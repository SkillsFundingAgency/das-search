namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    class Program
    {
        static void Main(string[] args)
        {

            var cmdArgs = new CmdArgs() { UseLocalCsvFile = false };
            var container = ContainerBootstrapper.BootstrapStructureMap();
            var meta = container.GetInstance<IMetaDataCreation>();
            //meta.Run(cmdArgs);

            var stringFormater = container.GetInstance<IJsonStringFormater>();
            //stringFormater.StartStatic();

            var vstsService= container.GetInstance<IVstsService>();
            vstsService.Start();

            Console.WriteLine(".....-O-O-o-o-0-0 ThE eNd 0-0-o-o-O-O-.....");
            Console.ReadLine();
        }
    }
}
