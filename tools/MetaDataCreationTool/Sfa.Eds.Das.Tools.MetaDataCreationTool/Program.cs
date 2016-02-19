namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    class Program
    {
        static void Main(string[] args)
        {

            var cmdArgs = new CmdArgs() { UseLocalCsvFile = false };
            var container = ContainerBootstrapper.BootstrapStructureMap();
            var meta = container.GetInstance<IMetaDataCreation>();

            var stringFormater = container.GetInstance<IJsonStringFormater>();
            //meta.Run(cmdArgs);
            stringFormater.StartStatic();

            Console.ReadLine();
        }
    }
}
