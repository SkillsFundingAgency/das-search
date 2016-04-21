namespace ConfigTransformTool
{
    using System;

    using Microsoft.Web.XmlTransform;

    internal class Program
    {
        private static void Main(string[] args)
        {
            TransformConfig(args[0], args[1], args[2]);
        }

        public static void TransformConfig(string configFileName, string transformFileName, string targetFileName)
        {
            var document = new XmlTransformableDocument();
            document.PreserveWhitespace = true;
            document.Load(configFileName);

            var transformation = new XmlTransformation(transformFileName);
            if (!transformation.Apply(document))
            {
                throw new Exception("Transformation Failed");
            }
            document.Save(targetFileName);
        }
    }
}