using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Windows_OnOff
{
    internal class xml_processing
    {

        private static data_processing dp = new data_processing();
        private static XDocument document = new XDocument();
        public xml_processing()
        {
            // only one time this execute
            if (static_variables.Check_xml_processing == true) return;
            static_variables.Check_xml_processing = true;

            // if no xml file create a xml
            if (!File.Exists(static_variables.xml_root_path)) create_xml(new List<XElement>() { new XElement("LastUpdateId", "0") });
            // XML Document loading
            document = XDocument.Load(static_variables.xml_root_path);
            // Writing and preparing the Service Folder delete command
            XElement x_element = new XElement("CommandServiceDelete", $"rd /s /q {static_variables.Service_folder_path}");
            bool bo = check_element(x_element);
            if (!bo) add_xmlElement(x_element);

        }
        private void create_xml(List<XElement> x_elements)
        {
            // root-element erstellen
            XElement root = new XElement("root");

            foreach (XElement element in x_elements)
            {
                root.Add(element);
            }
            document.Add(root);

            document.Save(static_variables.xml_root_path);

            dp._logWrite("A xml File created in:" + static_variables.xml_root_path);
        }
        public void add_xmlElement(XElement x_element)
        {
            document.Root?.Add(x_element);

            document.Save(static_variables.xml_root_path);
        }
        public bool check_element(XElement x_element)
        {
            bool exists = document.Descendants(x_element.Name).Any();
            return exists;
        }
        public void updateElementsContent_FirstLevel(List<string> names, List<string> contents)
        {
            for (int i = 0; i < names.Count; i++)
            {
                XElement xE = document.Element("root")?.Element(names[i]);
                if (xE != null) xE.Value = contents[i];
                document.Save(static_variables.xml_root_path);
                // log
                dp._logWrite($"The inner content of the element `{names[i]}´ has been changed. and saved");
            }
        }
        public XElement get_XmlElements(string name)
        {
            XElement xE = document.Root?.Element(name);

            if (document.Root?.Element(name) != null)
                xE = document.Root.Element(name);
            else
                xE = null;

            return xE;
        }

    }
}
