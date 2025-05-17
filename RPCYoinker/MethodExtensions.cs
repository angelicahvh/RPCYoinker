using dnlib.DotNet;

namespace RPCYoinker
{
    public static class MethodExtensions
    {
        public static bool HasAttribute(this MethodDef methodDef, params string[] attributeNames)
        {
            foreach (var attr in methodDef.CustomAttributes)
            foreach (var attrName in attributeNames)
                if (attr.AttributeType.FullName.EndsWith(attrName))
                    return true;

            return false;
        }
    }
}