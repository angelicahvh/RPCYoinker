using System.Collections.Generic;

namespace RPCYoinker
{
    public class RpcInfo
    {
        public string TypeName { get; set; }
        public string MethodName { get; set; }

        public List<string> Parameters { get; set; }

        public override string ToString()
        {
            string parameters = string.Join(", ", Parameters);
            return $"{TypeName}.{MethodName}({parameters})";
        }
    }
}