using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formular.Notifications.Serializers
{
    public class CamelCaseSerializer : PocoJsonSerializerStrategy
    {
        protected override string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {
            //PascalCase to snake_case
            return $"{clrPropertyName.Substring(0, 1).ToLower()}{clrPropertyName.Substring(1)}";
        }
    }
}
