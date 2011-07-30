using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Collections;

namespace Archimedes.CodeDOM
{

    public class CodeDOMTraveler
    {
        public StringComparison StringComparison = StringComparison.InvariantCultureIgnoreCase;




        public IEnumerable<CodeMemberMethod> GetMethodsFromType(CodeTypeDeclaration type){
            return from member in type.Members.OfType<CodeTypeMember>()
                   where member is CodeMemberMethod
                   select member as CodeMemberMethod;
        }

        public CodeMemberMethod FindBestMethod(string identifier, CodeTypeReference[] paramTypes, CodeTypeDeclaration type) {
            return FindBestMethod(identifier, paramTypes, GetMethodsFromType(type));
        }

        public CodeMemberMethod FindBestMethod(string identifier, CodeTypeReference[] paramTypes, IEnumerable<CodeMemberMethod> methodsInContext) {

            var methods = FindMatchingMethods(identifier, paramTypes, methodsInContext);
            if(methods.Any()) {
                var realMethods = methods.ToList();
                realMethods.Sort((a, b) => a.Name.CompareTo(identifier));
                return realMethods.First();
            } else
                return null;
        }

        public IEnumerable<CodeMemberMethod> FindMatchingMethods(string identifier, CodeTypeReference[] paramTypes, IEnumerable<CodeMemberMethod> methodsInContext) {

            int mustSupportParamCount = paramTypes.Count();

            var methods = from m in methodsInContext
                          where string.Equals(m.Name, identifier, StringComparison)
                          && m.Parameters.Count >= mustSupportParamCount
                          select m;

            // ToDo: check for type parameter and optional params

            return methods;
        }

    }
}
