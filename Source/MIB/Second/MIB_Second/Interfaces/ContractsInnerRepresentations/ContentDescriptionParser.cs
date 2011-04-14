using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Interfaces.ContractsInnerRepresentations
{
    public static class ContentDescriptionParser
    {

        public static List<ContentDescriptionAttribute> Parse(string contentDescription)
        {

            List<ContentDescriptionAttribute> parseResult = new List<ContentDescriptionAttribute>();
        
            XDocument doc = XDocument.Parse(contentDescription);

            foreach( XElement element in doc.Root.Descendants() )
            {

                parseResult.Add(new ContentDescriptionAttribute()
                                    {Name = element.Name.ToString(), Value = element.Value});

            }

            return parseResult;

        }

    }
}
